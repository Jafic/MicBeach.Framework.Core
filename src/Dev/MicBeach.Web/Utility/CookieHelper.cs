using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MicBeach.Util.Security;
using Microsoft.AspNetCore.Http;

namespace MicBeach.Web.Utility
{
    /// <summary>
    /// Cookie操作帮助类
    /// </summary>
    public static class CookieHelper
    {
        #region 保存Cookie

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookie">cookie对象</param>
        public static void SaveCookie(CookieItem cookie)
        {
            if (cookie == null)
            {
                return;
            }
            cookie.Options = cookie.Options ?? new CookieOptions();
            cookie.Options.HttpOnly = true;

            HttpContextHelper.Current.Response.Cookies.Append(cookie.Key, cookie.Value, cookie.Options);
        }

        #endregion

        #region 根据指定的cookie名称返回一个Cookie对象

        /// <summary>
        /// 根据指定的cookie名称返回一个Cookie对象
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <returns>返回一个Cookie对象,找不到指定的Cookie返回一个null</returns>
        public static CookieItem GetCookie(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return null;
            }
            var cookieCollection = HttpContextHelper.Current.Request.Cookies;
            if (cookieCollection.Keys.Contains(cookieName))
            {
                return new CookieItem()
                {
                    Key = cookieName,
                    Value = cookieCollection[cookieName]
                };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 返回输出对象指定键值的Cookie的值

        /// <summary>
        /// 返回输出对象指定名称的Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie键值</param>
        /// <returns>返回Cookie的值</returns>
        public static string GetCookieValue(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return string.Empty;
            }
            var nowCookie = GetCookie(cookieName);
            if (nowCookie == null)
            {
                return string.Empty;
            }
            return nowCookie.Value;
        }

        #endregion

        #region 设置Cookie的值

        /// <summary>
        /// 设置Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie对象名称</param>
        /// <param name="value">Cookie值</param>
        /// <returns>执行结果</returns>
        public static bool SetCookieValue(string cookieName, string value, DateTime? expiresTime = null)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return false;
            }
            var nowCookie = GetCookie(cookieName);
            if (nowCookie == null)
            {
                nowCookie = new CookieItem()
                {
                    Key = cookieName
                };
            }
            if (!expiresTime.HasValue)
            {
                expiresTime = DateTime.Now.AddHours(2);
            }
            var options = nowCookie.Options ?? new CookieOptions();
            nowCookie.Value = value;
            options.Expires = expiresTime.Value;
            nowCookie.Options = options;
            SaveCookie(nowCookie);
            return true;
        }

        #endregion

        #region 移除指定名称的Cookie值

        /// <summary>
        /// 移除指定名称的Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>操作结果</returns>
        public static bool RemoveCookie(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return false;
            }
            HttpContextHelper.Current.Response.Cookies.Delete(cookieName);
            return true;
        }

        #endregion
    }
}
