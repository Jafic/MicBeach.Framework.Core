using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MicBeach.Util.Extension;
using Microsoft.AspNetCore.Http;

namespace MicBeach.Web.Utility
{
    public static class UrlHelper
    {
        #region Url Encode/Decode

        public static string UrlEncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            return HttpUtility.UrlEncode(url);
        }

        public static string UrlDecode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            return HttpUtility.UrlDecode(url);
        }

        #endregion

        /// <summary>
        /// 去除参数Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlWithOutParameter(string url)
        {
            if (url.IsNullOrEmpty())
            {
                return url;
            }
            string[] urlArray = url.LSplit("?");
            if (urlArray.Length <= 0)
            {
                return string.Empty;
            }
            return urlArray[0];
        }

        /// <summary>
        /// 移除地址中指定的参数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameters">参数信息</param>
        /// <param name="parameterNames">要移除的参数名</param>
        /// <returns></returns>
        public static string RemoveUrlParameters(string url, IDictionary<string, string> parameters, IEnumerable<string> parameterNames)
        {
            if (parameterNames.IsNullOrEmpty() || url.IsNullOrEmpty() || parameters == null || parameters.Count <= 0)
            {
                return url;
            }
            List<string> removeNames = parameterNames.Select(c => c.ToLower()).ToList();
            url = GetUrlWithOutParameter(url).Trim('/', '?', '&');
            List<string> parameterValues = new List<string>(parameters.Count);
            foreach (var parameterItem in parameters)
            {
                string parameterName = parameterItem.Key.ToLower();
                if (removeNames.Contains(parameterName))
                {
                    continue;
                }
                parameterValues.Add(string.Format("{0}={1}", parameterName, UrlEncode(parameterItem.Value)));
            }
            if (parameterValues.IsNullOrEmpty())
            {
                return url;
            }
            return string.Format("{0}?{1}", url, string.Join("&", parameterValues));
        }

        /// <summary>
        /// 移除地址中指定的参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parameterNames"></param>
        /// <returns></returns>
        public static string RemoveUrlParameters(HttpRequest request, IEnumerable<string> parameterNames)
        {
            if (request == null)
            {
                return string.Empty;
            }
            string[] queryParameterNames = request.Query.Keys.ToArray();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (string parameterKey in parameterNames)
            {
                if (parameterKey.IsNullOrEmpty())
                {
                    continue;
                }
                string parameterValue = request.Query[parameterKey];
                if (parameterValue.IsNullOrEmpty())
                {
                    continue;
                }
                parameters.Add(parameterKey, parameterValue);
            }
            return RemoveUrlParameters(request.Path, parameters, parameterNames);
        }

        /// <summary>
        /// 向地址中添加参数
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameters">要添加的参数</param>
        /// <returns></returns>
        public static string AppendParameters(string url, IDictionary<string, string> parameters)
        {
            if (url.IsNullOrEmpty() || parameters == null || parameters.Count <= 0)
            {
                return string.Empty;
            }
            Dictionary<string, string> nowParameters = new Dictionary<string, string>();
            string[] urlArray = url.LSplit("?");
            if (urlArray.Length > 1)
            {
                string urlParameterString = urlArray[1];
                var urlParameterValues = HttpUtility.ParseQueryString(urlParameterString);
                string[] parameterKeys = urlParameterValues.AllKeys;
                foreach (string key in parameterKeys)
                {
                    nowParameters.Add(key.ToLower(), urlParameterValues[key]);
                }
            }
            foreach (var newParameter in parameters)
            {
                string keyName = newParameter.Key.ToLower();
                if (nowParameters.ContainsKey(keyName))
                {
                    nowParameters[keyName] = newParameter.Value;
                }
                else
                {
                    nowParameters.Add(keyName, newParameter.Value);
                }
            }
            url = GetUrlWithOutParameter(url);
            if (nowParameters == null || nowParameters.Count <= 0)
            {
                return url;
            }
            List<string> parameterValueString = new List<string>(nowParameters.Count);
            foreach (var parameter in nowParameters)
            {
                parameterValueString.Add(string.Format("{0}={1}", parameter.Key, UrlEncode(parameter.Value)));
            }
            return string.Format("{0}?{1}", url, string.Join("&", parameterValueString));
        }
    }
}
