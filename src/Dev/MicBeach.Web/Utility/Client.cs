using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MicBeach.Web.Utility
{
    /// <summary>
    /// Web客户端帮助类
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public static string Host
        {
            get
            {
                return HttpContextHelper.Current.Request.Host.Host;
            }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public static int Port
        {
            get
            {
                return HttpContextHelper.Current.Request.Host.Port??80;
            }
        }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public static string IP
        {
            get
            {
                return HttpContextHelper.Current.Connection.RemoteIpAddress.ToString();
            }
        }

        /// <summary>
        /// 客户端请求的原始URL
        /// </summary>
        public static string RawUrl
        {
            get
            {
                return HttpContextHelper.Current.Request.GetEncodedUrl();
            }
        }

        /// <summary>
        /// 客户端请求的URL
        /// </summary>
        public static string Url
        {
            get
            {
                return HttpContextHelper.Current.Request.GetDisplayUrl();
            }
        }

        /// <summary>
        /// 请求协议
        /// </summary>
        public static string Protocol
        {
            get
            {
                return HttpContextHelper.Current.Request.Protocol;
            }
        }
    }
}
