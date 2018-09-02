using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Request
{
    /// <summary>
    /// 凭据验证请求数据
    /// </summary>
    public class CredentialVerifyRequest
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public IdentityServer4.Models.Client Client
        {
            get; set;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        public CredentialUser User
        {
            get; set;
        }
    }
}
