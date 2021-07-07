using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Project.Services;
using Project.Interfaces;

namespace Project
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(@"Server=DESKTOP-5TV2V3H\SQLEXPRESS;Database=Project;Trusted_Connection=True;");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(cookie =>
                {
                    cookie.LoginPath = new Microsoft.AspNetCore.Http.PathString("/authorize/login");
                    cookie.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/authorize/login");
                });

            services.AddSingleton<CsharpBuilder>();
            services.AddSingleton<PythonBuilder>();

            services.AddTransient<AuthorizeService>();
            services.AddTransient<DiscussService>();
            services.AddTransient<ChallengeService>();

            services.AddTransient<UserActivityService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
