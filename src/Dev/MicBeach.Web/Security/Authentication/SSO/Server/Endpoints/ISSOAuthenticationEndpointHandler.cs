using MicBeach.Web.Security.Authentication.SSO.Server.Endpoints.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Endpoints
{
    public interface ISSOAuthenticationEndpointHandler
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        Task<ISSOAuthenticationEndpointResult> ProcessAsync(HttpContext context);
    }
}
