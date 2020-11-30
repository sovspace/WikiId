using Application.Features.ArticleFeatures.Commands;
using Application.Features.ArticleFeatures.Queries;
using Application.Features.CategoryFeatures.Queries;
using FileSaverService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Controllers.Common;
using WebApp.ViewModels.AccessRequestViewModels;
using WebApp.ViewModels.ArticleViewModels;
using WebApp.ViewModels.CategoryViewModels;
using WebApp.ViewModels.UserViewModels;

namespace WebApp.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileSaver _fileSaver;
        private readonly IWebHostEnvironment _appEnvironment;

        public ArticleController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IFileSaver fileSaver,
            IWebHostEnvironment appEnvironment) : base(userManager)
        {
            _roleManager = roleManager;
            _fileSaver = fileSaver;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articlesResponse = await Mediator.Send(new GetPublicArticlesQuery());
            List<ArticleViewModel> articles = new List<ArticleViewModel>();
            foreach (var articleResponse in articlesResponse)
            {
                articles.Add(new ArticleViewModel(articleResponse,
                    (await _userManager.FindByIdAsync(articleResponse.Creator.IdentityUserId)).UserName));
            }
            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetArticle(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var result = await Mediator.Send(new GetArticleByIdQuery { Id = id, UserRoles = userRoles });
            if (result != null)
            {
                DetailedArticleViewModel viewModel = new DetailedArticleViewModel(result, (await _userManager.FindByIdAsync(result.Creator.IdentityUserId)).UserName);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Home", new { message = "No article found" });
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Created()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var articlesResponse = await Mediator.Send(new GetCreatedArticlesQuery { IdentityUserId = currentUser.Id });

            List<CreatedArticleViewModel> articles = new List<CreatedArticleViewModel>();
            foreach (var articleResponse in articlesResponse)
            {
                List<AccessRequestViewModel> accessRequestViewModels = new List<AccessRequestViewModel>();
                List<UserViewModel> canEditUsersViewModels = new List<UserViewModel>();
                List<UserViewModel> canViewUsersViewModels = new List<UserViewModel>();
                foreach (var accessRequest in articleResponse.AccessRequests)
                {
                    accessRequestViewModels.Add(new AccessRequestViewModel(accessRequest,
                        new UserViewModel(await _userManager.FindByIdAsync(accessRequest.Profile.IdentityUserId))));
                }
                foreach (var user in await _userManager.GetUsersInRoleAsync(articleResponse.EditRoleString))
                {
                    canEditUsersViewModels.Add(new UserViewModel(user));
                }
                foreach (var user in await _userManager.GetUsersInRoleAsync(articleResponse.ViewRoleString))
                {
                    canViewUsersViewModels.Add(new UserViewModel(user));
                }

                articles.Add(new CreatedArticleViewModel(articleResponse,
                    accessRequestViewModels,
                    canEditUsersViewModels,
                    canViewUsersViewModels,
                    new UserViewModel(await _userManager.FindByIdAsync(articleResponse.Creator.IdentityUserId))));
            }
            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Editable()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var articlesResponse = await Mediator.Send(new GetEditableArticlesQuery { UserRoles = userRoles });

            List<ArticleViewModel> articles = new List<ArticleViewModel>();
            foreach (var articleResponse in articlesResponse)
            {
                articles.Add(new ArticleViewModel(articleResponse, (await _userManager.FindByIdAsync(articleResponse.Creator.IdentityUserId)).UserName));
            }
            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Viewable()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var articlesResponse = await Mediator.Send(new GetViewableArticlesQuery { UserRoles = userRoles });
            List<ArticleViewModel> articles = new List<ArticleViewModel>();
            foreach (var articleResponse in articlesResponse)
            {
                articles.Add(new ArticleViewModel(articleResponse, (await _userManager.FindByIdAsync(articleResponse.Creator.IdentityUserId)).UserName));
            }
            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allCategories = await Mediator.Send(new GetAllCategoriesQuery());
            CreateArticleViewModel viewModel = new CreateArticleViewModel();
            if (allCategories.Any())
            {
                viewModel.Categories = new SelectList(allCategories.Select(c => new CategoryTitleViewModel(c, c.Title)), "Id", "Title");
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Home", new { message = "No categories found" });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string path = "/img/no_image.jpg";
                if (model.TitleImage != null)
                {
                    FileSaverResult fileSaverResult = await _fileSaver.SaveArticleTitleImage(_appEnvironment.WebRootPath, model.TitleImage);
                    if (fileSaverResult.IsSuccessful)
                    {
                        path = fileSaverResult.Path;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't save image");
                    }
                }

                var result = await Mediator.Send(new CreateArticleCommand
                {
                    CategoryId = model.CategoryId,
                    IsPublic = model.IsPublic,
                    Title = model.Title,
                    TitleImagePath = path,
                    IdentityUserId = currentUser.Id
                });

                if (result.IsSuccessful)
                {
                    var editArticleRoleResult = await _roleManager.CreateAsync(new IdentityRole(result.EditArticleRoleName));
                    var viewArticleRoleResult = await _roleManager.CreateAsync(new IdentityRole(result.ViewArticleRoleName));

                    if (editArticleRoleResult.Succeeded && viewArticleRoleResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(currentUser, result.EditArticleRoleName);
                        await _userManager.AddToRoleAsync(currentUser, result.ViewArticleRoleName);

                        return RedirectToAction("Index", "Article");
                    }
                    else
                    {
                        await Mediator.Send(new DeleteArticleCommand { Id = result.ArticleId, IdentityUserId = currentUser.Id });
                        ModelState.AddModelError("", "Error save");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            var allCategories = await Mediator.Send(new GetAllCategoriesQuery());
            if (allCategories.Any())
            {
                model.Categories = new SelectList(allCategories.Select(c => new CategoryTitleViewModel(c, c.Title)), "Id", "Title");
                return View(model);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Home", new { message = "No categories found" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int articleId)
        {
            UpdateArticleViewModel model = new UpdateArticleViewModel
            {
                Categories = new SelectList((await Mediator.Send(new GetAllCategoriesQuery())).Select(c => new CategoryTitleViewModel(c, c.Title)), "Id", "Title"),
                ArticleId = articleId
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(int articleId, UpdateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var userRoles = await _userManager.GetRolesAsync(currentUser);
                string path = null;
                if (model.TitleImage != null)
                {
                    FileSaverResult fileSaverResult = await _fileSaver.SaveArticleTitleImage(_appEnvironment.WebRootPath, model.TitleImage);
                    if (fileSaverResult.IsSuccessful)
                    {
                        path = fileSaverResult.Path;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't save image");
                    }
                }

                var result = await Mediator.Send(new UpdateArticleCommand
                {
                    Title = model.Title,
                    IsPublic = model.IsPublic,
                    CategoryId = model.CategoryId,
                    TitleImagePath = path,
                    UserRoles = userRoles,
                    ArticleId = articleId,
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
            var allCategories = await Mediator.Send(new GetAllCategoriesQuery());
            if (allCategories.Any())
            {
                model.Categories = new SelectList(allCategories.Select(c => new CategoryTitleViewModel(c, c.Title)), "Id", "Title");
                return View(model);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Home", new { message = "No categories found" });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Delete(int articleId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var mediatorResult = await Mediator.Send(new DeleteArticleCommand { Id = articleId, IdentityUserId = currentUser.Id });
            if (mediatorResult.IsSuccessful)
            {
                await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(mediatorResult.EditRole));
                await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(mediatorResult.ViewRole));
            }
            HttpStatusCode statusCode = mediatorResult.IsSuccessful ? HttpStatusCode.OK : HttpStatusCode.MethodNotAllowed;
            return new JsonResult(new { mediatorResult.Message }) { StatusCode = (int)statusCode };
        }
    }
}
