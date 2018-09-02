using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Text;
using MicBeach.Web.Security.Authentication.Cookie;
using MicBeach.Web.Security.Authentication.Cookie.Ticket;
using MicBeach.Util.IoC;
using MicBeach.Util.Extension;
using MicBeach.Web.Utility;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CookieAuthenticationExtensions
    {
        public static void AddCookie(this AuthenticationBuilder builder, CustomCookieOptions cookieOptions)
        {
            cookieOptions = cookieOptions ?? new CustomCookieOptions();
            var configureOptions = cookieOptions?.CookieConfiguration;
            Action<CookieAuthenticationOptions> customDefaultConfigure = options =>
            {
                CookieAuthenticationEventHandler.ForceValidatePrincipal = cookieOptions.ForceValidatePrincipal;
                CookieAuthenticationEventHandler.OnValidatePrincipalAsync = cookieOptions.ValidatePrincipalAsync;
                options.EventsType = typeof(CookieAuthenticationEventHandler);
                options.Cookie.HttpOnly = true;
                var storageModel = cookieOptions?.StorageModel ?? CookieStorageModel.Default;
                switch (storageModel)
                {
                    case CookieStorageModel.Default:
                    default:
                        options.SessionStore = null;
                        break;
                    case CookieStorageModel.Distributed:
                        options.SessionStore = ContainerManager.Container.Instance<ITicketDistributedStore>();
                        break;
                    case CookieStorageModel.InMemory:
                        options.SessionStore = new CookieMemoryCacheTicketStore();
                        break;
                }
                if (options.Cookie.Name.IsNullOrEmpty())
                {
                    options.Cookie.Name = string.Format("{0}_{1}_{2}", Client.Host, Client.Port, "authenticationkey_~!@#$%^&*").Encrypt().ReplaceByRegex("[^0-9a-zA-Z]", "");
                }
            };
            if (configureOptions != null)
            {
                configureOptions += customDefaultConfigure;
            }
            else
            {
                configureOptions = customDefaultConfigure;
            }
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(configureOptions);
            builder.Services.AddSingleton<CookieAuthenticationEventHandler>();//注册Cookie事件类型
        }
    }
}
