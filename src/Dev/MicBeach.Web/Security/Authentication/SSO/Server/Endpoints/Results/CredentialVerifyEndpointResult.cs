using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Web.Security.Authentication.SSO.Server.Results;
using Microsoft.AspNetCore.Http;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Endpoints.Results
{
    /// <summary>
    /// 凭据验证结果
    /// </summary>
    public class CredentialVerifyEndpointResult : ISSOAuthenticationEndpointResult
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public CredentialVerifyResult VerifyResult
        {
            get; set;
        }

        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(HttpContext context)
        {
            if (VerifyResult?.VerifySuccess ?? false)
            {
                await ProcessSuccessAsync(context).ConfigureAwait(false);
            }
            else
            {
                await ProcessErrorAsync(context).ConfigureAwait(false);
            }
        }

        async Task ProcessSuccessAsync(HttpContext context)
        {
            context.Response.SetNoCache();
            await context.Response.WriteJsonAsync(VerifyResult);
        }

        async Task ProcessErrorAsync(HttpContext context)
        {
            context.Response.SetNoCache();
            await context.Response.WriteJsonAsync(VerifyResult);
        }
    }
}
