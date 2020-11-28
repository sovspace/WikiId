using Application.Features.ProfileFeatures.Commands;
using EmailService.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Controllers.Common;
using WebApp.ViewModels.UserViewModels;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _sender;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender sender) : base(userManager)
        {
            _signInManager = signInManager;
            _sender = sender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.UserName };
                var resultManager = await _userManager.CreateAsync(user, model.Password);

                if (resultManager.Succeeded)
                {
                    string confirmationLink = Url.ActionLink("Confirm", "Account", new { userId = user.Id, token = await _userManager.GenerateEmailConfirmationTokenAsync(user) });
                    var resultMediator = await Mediator.Send(new CreateProfileCommand { IdentityUserId = user.Id });
                    if (resultMediator.IsSuccessful)
                    {
                        var emailResult = await _sender.SendEmailAsync(user.Email, "Confirmation", $"Confirmation link {confirmationLink}");

                        if (emailResult.IsSuccessful)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            await _userManager.DeleteAsync(user);
                            await Mediator.Send(new DeleteProfileCommand { IdentityUserId = user.Id });
                            ModelState.AddModelError("", emailResult.Message);
                        }

                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError("", resultMediator.Message);
                    }
                }
                else
                {
                    foreach (var error in resultManager.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or password");
                }

            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (confirmationResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("ErrorMesage", "Home", new { Message = "Wrong redirection link" });
            }

        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }


        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                IdentityUser user = new IdentityUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        var mediatorResult = await Mediator.Send(new CreateProfileCommand { IdentityUserId = user.Id });
                        if (mediatorResult.IsSuccessful)
                        {
                            await _signInManager.SignInAsync(user, false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            await _userManager.DeleteAsync(user);
                            return RedirectToAction("Login");
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
