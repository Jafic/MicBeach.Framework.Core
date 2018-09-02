using IdentityModel;
using IdentityServer4.Models;
using MicBeach.Web.Security.Authentication.SSO.Server.Request;
using MicBeach.Web.Security.Authentication.SSO.Server.Results;
using MicBeach.Util.Serialize;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using MicBeach.Web.Utility;

namespace MicBeach.Web.Security.Authentication.SSO.Client
{
    /// <summary>
    /// 工具
    /// </summary>
    public static class SSOUtil
    {
        /// <summary>
        /// 验证登陆凭据
        /// </summary>
        /// <param name="principal">登陆凭据</param>
        /// <returns></returns>
        public static async Task<bool> VerifyCredentialAsync(ClaimsPrincipal principal,AuthenticationProperties properties)
        {
            IConfiguration configuration = HttpContextHelper.Current.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            if (configuration == null)
            {
                throw new Exception("get IConfiguration fail");
            }
            var ssoOptions = HttpContextHelper.Current.RequestServices.GetService<IOptionsMonitor<SSOAuthenticationOption>>().Get(Constants.SSOAuthenticationScheme); //configuration.Get<SSOAuthenticationOption>();//(HttpContextHelper.Current.RequestServices.GetService(typeof(IOptions<SSOAuthenticationOption>)) as IOptions<SSOAuthenticationOption>)?.Value;
            if (ssoOptions == null)
            {
                throw new Exception("get SSOAuthenticationOption fail");
            }
            var openIdOption = HttpContextHelper.Current.RequestServices.GetService<IOptionsMonitor<OpenIdConnectOptions>>().Get(OpenIdConnectDefaults.AuthenticationScheme);
            if (openIdOption == null)
            {
                throw new Exception("get OpenIdConnectOptins fail");
            }
            var subjectClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (subjectClaim == null)
            {
                subjectClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
            }
            if (subjectClaim == null || string.IsNullOrWhiteSpace(subjectClaim.Value))
            {
                //await HttpContextHelper.Current.SignOutAsync();
                return false;
            }
            CredentialVerifyRequest request = new CredentialVerifyRequest()
            {
                Client = new IdentityServer4.Models.Client()
                {
                    ClientId = openIdOption.ClientId,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret(openIdOption.ClientSecret.Sha256())
                    }
                },
                User = new CredentialUser()
                {
                    Id = subjectClaim.Value
                }
            };
            string url = ssoOptions.CredentialVerifyUrl;
            if (string.IsNullOrWhiteSpace(url))
            {
                url = openIdOption.Authority + "/" + Constants.RoutePaths.CredentialVerify;
            }
            try
            {
                HttpClient client = new HttpClient();
                var result = await client.PostAsJsonAsync(url, request).ConfigureAwait(false);
                var stringValue = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                CredentialVerifyResult verifyResult = JsonSerialize.JsonToObject<CredentialVerifyResult>(stringValue);
                var loginSuccess = verifyResult?.VerifySuccess ?? false;
                //if (!loginSuccess)
                //{
                //    await HttpContextHelper.Current.SignOutAsync();
                //}
                return loginSuccess;
            }
            catch (Exception ex)
            {
                //await HttpContextHelper.Current.SignOutAsync();
                throw ex;
            }
        }

        /// <summary>
        /// 验证Cookie登陆凭据
        /// </summary>
        /// <param name="principalContext"></param>
        /// <returns></returns>
        public static async Task<bool> VerifyCredentialAsync(CookieValidatePrincipalContext principalContext)
        {
            if (principalContext == null)
            {
                return false;
            }
            return await VerifyCredentialAsync(principalContext.Principal,principalContext.Properties).ConfigureAwait(false);
        }
    }
}
