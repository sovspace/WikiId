using Application.Features.SubarticleFeatures.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebApp.Controllers.Common;
using WebApp.ViewModels.SubarticleViewModels;

namespace WebApp.Controllers
{
    public class SubarticleController : BaseController
    {
        public SubarticleController(UserManager<IdentityUser> userManager) : base(userManager)
        {
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int articleId)
        {
            CreateSubarticleViewModel model = new CreateSubarticleViewModel
            {
                ArticleId = articleId,
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(int articleId, CreateSubarticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var userRoles = await _userManager.GetRolesAsync(identityUser);
                var result = await Mediator.Send(new CreateSubarticleCommand()
                {
                    ArticleId = articleId,
                    Content = model.Content,
                    Title = model.Title,
                    UserRoles = userRoles,

                });

                if (result.IsSuccessful)
                {
                    return RedirectToAction("GetArticle", "Article", new { id = articleId });
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Update(int subarticleId)
        {
            UpdateSubarticleViewModel model = new UpdateSubarticleViewModel
            {
                SubarticleId = subarticleId,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(int subarticleId, UpdateSubarticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var userRoles = await _userManager.GetRolesAsync(identityUser);
                var result = await Mediator.Send(new UpdateSubarticleCommand()
                {
                    SubarticleId = subarticleId,
                    Content = model.Content,
                    Title = model.Title,
                    UserRoles = userRoles,
                });

                if (result.IsSuccessful)
                {
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Delete(int subarticleId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var mediatorResult = await Mediator.Send(new DeleteSubarticleCommand
            {
                SubarticleId = subarticleId,
                UserRoles = await _userManager.GetRolesAsync(currentUser)
            });
            HttpStatusCode statusCode = mediatorResult.IsSuccessful ? HttpStatusCode.OK : HttpStatusCode.MethodNotAllowed;
            return new JsonResult(new { mediatorResult.Message })
            {
                StatusCode = (int)statusCode,
            };
        }
    }
}
