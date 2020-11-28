using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService
{
    public static class DependencyInjection
    {
        public static void AddNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();
        }

        public static void AddNotificationEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints
        }
    }
}
