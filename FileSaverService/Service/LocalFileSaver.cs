using FileSaverService.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileSaverService.Service
{
    class LocalFileSaver : IlFileSaver
    {
        public readonly FileSaverSettings _fileSaverSettings;

        public LocalFileSaver(IOptions<FileSaverSettings> fileSaverSettings)
        {
            _fileSaverSettings = fileSaverSettings.Value;
        }

        public async Task<FileSaverResult> SaveArticleGalleryFile(string root, IFormFile file)
        {
            try
            {
                string path = root + _fileSaverSettings.ArticleGalleryFileDirectory + file.FileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return new FileSaverResult
                {
                    IsSuccessful = true,
                    Path = path,
                };
            }
            catch (Exception)
            {
                return new FileSaverResult
                {
                    IsSuccessful = false,
                };
            }
        }

        public async Task<FileSaverResult> SaveArticleTitleImage(string root, IFormFile image)
        {
            try
            {
                string path = root + _fileSaverSettings.ArticleTitleImageDirectory + image.FileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return new FileSaverResult
                {
                    IsSuccessful = true,
                    Path = path,
                };
            }
            catch (Exception)
            {
                return new FileSaverResult
                {
                    IsSuccessful = false,
                };
            }
        }

        public async Task<FileSaverResult> SaveCategoryTitleImage(string root, IFormFile image)
        {
            try
            {
                string path = root + _fileSaverSettings.CategoryTitleImageDirectory + image.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return new FileSaverResult
                {
                    IsSuccessful = true,
                    Path = path,
                };
            }
            catch (Exception)
            {
                return new FileSaverResult
                {
                    IsSuccessful = false,
                };
            }
        }
    }
}
