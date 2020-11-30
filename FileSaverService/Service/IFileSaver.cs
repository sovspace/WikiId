using FileSaverService.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileSaverService.Service
{
    public interface IFileSaver
    {
        public Task<FileSaverResult> SaveArticleTitleImage(string root, IFormFile image);
        public Task<FileSaverResult> SaveCategoryTitleImage(string root, IFormFile image);
        public Task<FileSaverResult> SaveArticleGalleryFile(string root, IFormFile file);
    }
}
