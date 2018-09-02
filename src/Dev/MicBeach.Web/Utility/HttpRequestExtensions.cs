using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Utility
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 是否为ajax请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string GetValue(this HttpRequest request,string key)
        {
            if (string.IsNullOrWhiteSpace(key)||request==null)
            {
                return string.Empty;
            }
            try
            {
                if (request.Query.ContainsKey(key))
                {
                    return request.Query[key];
                }
                if (request.Form.ContainsKey(key))
                {
                    return request.Form[key];
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
