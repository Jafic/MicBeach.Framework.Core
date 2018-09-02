using IdentityServer4.Configuration;
using MicBeach.Web.Security.Authentication.SSO.Server.Endpoints.Results;
using MicBeach.Web.Security.Authentication.SSO.Server.Request;
using MicBeach.Web.Security.Authentication.SSO.Server.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Web.Security.Authentication.SSO.Server
{
    /// <summary>
    /// SSO验证服务器配置
    /// </summary>
    public class SSOServerOption
    {
        /// <summary>
        /// 凭据验证方式
        /// </summary>
        public Func<CredentialVerifyRequest,Task<CredentialVerifyResult>> CredentialVerifyMethodAsync
        {
            get; set;
        }
    }
}
