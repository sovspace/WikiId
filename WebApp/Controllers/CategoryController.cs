using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries;
using FileSaverService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApp.Controllers.Common;
using WebApp.ViewModels.ArticleViewModels;
using WebApp.ViewModels.CategoryViewModels;

namespace WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IFileSaver _fileSaver;

        public CategoryController(UserManager<IdentityUser> userManager,
            IWebHostEnvironment appEnvironment,
            IFileSaver fileSaver) : base(userManager)
        {
            _appEnvironment = appEnvironment;
            _fileSaver = fileSaver;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoriesResponse = await Mediator.Send(new GetAllCategoriesQuery());
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            foreach (var categoryResponse in categoriesResponse)
            {
                categories.Add(new CategoryViewModel(categoryResponse, (await _userManager.FindByIdAsync(categoryResponse.Creator.IdentityUserId)).UserName));
            }
            return View(categories);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCategory(int id)
        {
            var categoryResponse = await Mediator.Send(new GetCategoryByIdQuery { CategoryId = id });
            List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            foreach (var articleResponse in categoryResponse?.Articles)
            {
                articleViewModels.Add(new ArticleViewModel(articleResponse, (await _userManager.FindByIdAsync(articleResponse.Creator.IdentityUserId)).UserName));
            }

            DetailedCategoryViewModel viewModel = new DetailedCategoryViewModel(categoryResponse,
                (await _userManager.FindByIdAsync(categoryResponse.Creator.IdentityUserId)).UserName,
                articleViewModels);
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                string path = "/img/no_image.jpg";
                if (model.TitleImage != null)
                {
                    FileSaverResult fileSaverResult = await _fileSaver.SaveCategoryTitleImage(_appEnvironment.WebRootPath, model.TitleImage);
                    if (fileSaverResult.IsSuccessful)
                    {
                        path = fileSaverResult.Path;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't save image");
                    }
                }

                var result = await Mediator.Send(new CreateCategoryCommand
                {
                    Title = model.Title,
                    IdentityUserId = identityUser.Id,
                    TitleImagePath = path,

                });
                if (result.IsSuccessful)
                {
                    if (model.TitleImage != null)
                    {
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await model.TitleImage.CopyToAsync(fileStream);
                        }
                    }
                    return RedirectToAction("Index", "Category");

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
        public IActionResult Update(int categoryId)
        {
            UpdateCategoryViewModel model = new UpdateCategoryViewModel
            {
                CategoryId = categoryId
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(int categoryId, UpdateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                string path = null;
                if (model.TitleImage != null)
                {
                    FileSaverResult fileSaverResult = await _fileSaver.SaveCategoryTitleImage(_appEnvironment.WebRootPath, model.TitleImage);
                    if (fileSaverResult.IsSuccessful)
                    {
                        path = fileSaverResult.Path;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't save image");
                    }
                }

                var result = await Mediator.Send(new UpdateCategoryCommand
                {
                    CategoryId = categoryId,
                    Title = model.Title,
                    IdentityUserId = identityUser.Id,
                    TitleImagePath = path,

                });
                if (result.IsSuccessful)
                {
                    if (path != null)
                    {
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await model.TitleImage.CopyToAsync(fileStream);
                        }
                    }
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View();
        }
    }
}
