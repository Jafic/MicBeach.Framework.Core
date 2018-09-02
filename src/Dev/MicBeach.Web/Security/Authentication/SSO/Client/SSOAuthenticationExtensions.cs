using MicBeach.Web.Security.Authentication.Cookie;
using MicBeach.Web.Security.Authentication.SSO;
using MicBeach.Web.Security.Authentication.SSO.Client;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SSOAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSSO(this AuthenticationBuilder builder, SSOAuthenticationOption ssoOption)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (ssoOption == null)
            {
                throw new ArgumentNullException(nameof(ssoOption));
            }
            builder.AddCookie(new CustomCookieOptions()
            {
                ForceValidatePrincipal = true,
                ValidatePrincipalAsync = SSOUtil.VerifyCredentialAsync,
                CookieConfiguration = ssoOption.CookieConfiguration,
                StorageModel = ssoOption.StorageModel
            });
            if (ssoOption.OpenIdConnectConfigureOptions == null)
            {
                builder.AddOpenIdConnect();
            }
            else
            {
                builder.AddOpenIdConnect(ssoOption.OpenIdConnectConfigureOptions);
            }
            builder.Services.Configure<SSOAuthenticationOption>(Constants.SSOAuthenticationScheme, options =>
            {
                options.CredentialVerifyUrl = ssoOption.CredentialVerifyUrl;
            });
            return builder;
        }
    }
}
