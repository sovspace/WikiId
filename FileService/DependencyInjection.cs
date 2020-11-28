using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileService
{
    public static class DependencyInjection
    {
        public static void AddApplicaton(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("FailSection"));
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
