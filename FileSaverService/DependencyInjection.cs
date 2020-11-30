using FileSaverService.Service;
using FileSaverService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileSaverService
{
    public static class DependencyInjection
    {
        public static void AddFileSaver(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileSaverSettings>(configuration.GetSection("FileSaverSettings"));
            services.AddTransient<IFileSaver, LocalFileSaver>();
        }
    }
}
