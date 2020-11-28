using FileSaverService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileSaverService
{
    public static class DependencyInjection
    {
        public static void AddFileSaver(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileSaverResult>(configuration.GetSection("FileSaverSettings"));
            services.AddTransient<IlFileSaver, LocalFileSaver>();
        }
    }
}
