using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Web.Security.Authentication.SSO.Server.Endpoints.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Endpoints
{
    public abstract class SSOAuthenticationEndpointBase : ISSOAuthenticationEndpointHandler
    {
        public SSOAuthenticationEndpointBase(ILogger<SSOAuthenticationEndpointBase> logger,IOptions<SSOServerOption> ssoOption)
        {
            Logger = logger;
            SSOOption = ssoOption.Value;
        }

        public abstract Task<ISSOAuthenticationEndpointResult> ProcessAsync(HttpContext context);

        protected ILogger Logger { get; private set; }

        protected SSOServerOption SSOOption
        {
            get;set;
        }
    }
}
