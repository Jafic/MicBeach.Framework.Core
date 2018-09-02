using MicBeach.Web.Security.Authentication.SSO.Server.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class SSOAuthenticationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSSO(this IApplicationBuilder app)
        {
            app.UseMiddleware<SSOAuthenticationMiddleware>();
            app.UseIdentityServer();
            return app;
        }
    }
}
