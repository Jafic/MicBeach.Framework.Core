using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.IoC;
using MicBeach.Web.Mvc;
using MicBeach.Web.Security.Authentication.Cookie;
using MicBeach.Web.Security.Authorization;
using MicBeach.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Site.Cms.Config;
using Site.Cms.Helper;

namespace Site.Cms
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Mvc 配置

            //添加MVC
            services.AddMvc(options =>
            {
                OperationAuthorizeFilter.AuthorizeVerifyFuncAsync = UserHelper.AuthenticationAsync;
                options.Filters.Add<OperationAuthorizeFilter>();//授权验证
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(new CustomCookieOptions()
            {
                CookieConfiguration = options =>
                {
                    options.LoginPath = "/login";
                }
            });
            //启用Cookie身份认证
            //services.AddCookie(options =>
            //{
            //    options.AccessDeniedPath = "/noauth";
            //    options.LoginPath = "/login";
            //},CookieStorageModel.InMemory);

            #endregion

            services.AddHttpContextAccessor();

            return ContainerFactory.GetServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "home", action = "login" });

                routes.MapRoute(
                    name: "noauth",
                    template: "noauth",
                    defaults: new { controller = "home", action = "authentication" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            AppConfig.Init();
        }
    }
}
