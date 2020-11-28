using Application;
using EmailService;
using FileSaverService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using WebApp.Hubs;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddPersistence(Configuration);
            services.AddApplicaton();
            services.AddEmail(Configuration);
            services.AddFileSaver(Configuration);
            services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    opts.ClientId = Configuration.GetSection("GoogleAuthentication")["ClientId"];
                    opts.ClientSecret = Configuration.GetSection("GoogleAuthentication")["ClientSecret"];
                    opts.SignInScheme = IdentityConstants.ExternalScheme;
                });


            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "home-error-message",
                    pattern: "{controller=Home}/{action=ErrorMessage}");

                endpoints.MapControllerRoute(
                    name: "account-registrate",
                    pattern: "{controller=Account}/{action=Register}");

                endpoints.MapControllerRoute(
                    name: "account-login",
                    pattern: "{controller=Account}/{action=Login}");

                endpoints.MapControllerRoute(
                    name: "account-confirm",
                    pattern: "{controller=Account}/{action=Confirm}");

                endpoints.MapControllerRoute(
                    name: "article-create",
                    pattern: "{controller=Article}/{action=Create}");

                endpoints.MapControllerRoute(
                    name: "article-index",
                    pattern: "{controller=Article}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "article-created",
                    pattern: "{controller=Article}/{action=Created}");

                endpoints.MapControllerRoute(
                    name: "article-viewable",
                    pattern: "{controller=Article}/{action=Viewable}");

                endpoints.MapControllerRoute(
                    name: "article-editable",
                    pattern: "{controller=Article}/{action=Editable}");

                endpoints.MapControllerRoute(
                    name: "category-index",
                    pattern: "{controller=Category}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "category-create",
                    pattern: "{controller=Category}/{action=Create}");

                endpoints.MapHub<NotificationHub>("/notification");

            });
        }
    }
}
