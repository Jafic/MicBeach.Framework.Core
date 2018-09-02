using MicBeach.Web.Security.Authentication.Cookie;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Security.Authentication.SSO.Client
{
    public class SSOAuthenticationOption
    {
        /// <summary>
        /// 凭据验证地址
        /// </summary>
        public string CredentialVerifyUrl
        {
            get; set;
        }

        /// <summary>
        /// OpenId 验证配置
        /// </summary>
        public Action<OpenIdConnectOptions> OpenIdConnectConfigureOptions
        {
            get; set;
        }

        /// <summary>
        /// Cookie配置
        /// </summary>
        public Action<CookieAuthenticationOptions> CookieConfiguration
        {
            get; set;
        }

        /// <summary>
        /// Cookie数据存取方式
        /// </summary>
        public CookieStorageModel StorageModel
        {
            get; set;
        } = CookieStorageModel.Default;
    }
}
