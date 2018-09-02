using MicBeach.Web.Security.Authentication.SSO.Server;
using MicBeach.Web.Security.Authentication.SSO.Server.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SSOServerServiceCollectionExtensions
    {
        public static void AddSSOAuthentication(this IServiceCollection services, Action<SSOServerOption> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }
            services.Configure(configureOptions);
            services.AddDefaultsEndpoints();
        }
    }
}
