using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Web.Security.Authentication.SSO.Server.Endpoints.Results;
using MicBeach.Web.Security.Authentication.SSO.Server.Request;
using MicBeach.Web.Security.Authentication.SSO.Server.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using MicBeach.Util.Serialize;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Endpoints
{
    public class CredentialVerifyEndpoint : SSOAuthenticationEndpointBase
    {
        public CredentialVerifyEndpoint(ILogger<CredentialVerifyEndpoint> logger, IOptions<SSOServerOption> ssoOption) : base(logger, ssoOption)
        {

        }

        public override async Task<ISSOAuthenticationEndpointResult> ProcessAsync(HttpContext context)
        {
            Logger.LogDebug("Start Credential Verify");
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            var credentialVerifyRequest = BuildCredentialVerifyRequest(context);
            if (credentialVerifyRequest == null)
            {
                Logger.LogDebug("Build CredentialVeriryRequest object error");
                return new StatusCodeResult(HttpStatusCode.InternalServerError);
            }
            if (SSOOption.CredentialVerifyMethodAsync == null)
            {
                Logger.LogError("haven't configured any CredentialVerifyMethod value");
            }
            var result = await SSOOption.CredentialVerifyMethodAsync(credentialVerifyRequest).ConfigureAwait(false);
            return BuildCredentialVerifyEndpointResult(result);
        }

        CredentialVerifyRequest BuildCredentialVerifyRequest(HttpContext context)
        {
            string requestValue = string.Empty;
            StreamReader reader = new StreamReader(context.Request.Body);
            requestValue = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(requestValue))
            {
                return null;
            }
            try
            {
                var request = JsonSerialize.JsonToObject<CredentialVerifyRequest>(requestValue);
                return request;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return null;
            }
        }

        CredentialVerifyEndpointResult BuildCredentialVerifyEndpointResult(CredentialVerifyResult verifyResult)
        {
            return new CredentialVerifyEndpointResult()
            {
                VerifyResult = verifyResult
            };
        }
    }
}
