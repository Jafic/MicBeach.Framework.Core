using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace MicBeach.Web.Mvc
{
    /// <summary>
    /// 地址扩展
    /// </summary>
    /// <remarks>2015-2-12 李定濱 创建</remarks>
    public static class UrlExtensions
    {
        #region url参数修改

        /// <summary>
        /// 复制一个参数集合
        /// </summary>
        /// <param name="routeValueDictionary">参数集合</param>
        /// <returns>返回一个新的参数集合</returns>
        public static RouteValueDictionary CopyRouteValueDictionary(RouteValueDictionary routeValueDictionary)
        {
            if (routeValueDictionary == null)
            {
                return new RouteValueDictionary();
            }
            RouteValueDictionary newValue = new RouteValueDictionary();
            foreach (KeyValuePair<string, object> rvdItem in routeValueDictionary)
            {
                newValue.Add(rvdItem.Key.TooString(), rvdItem.Value);
            }
            return newValue;
        }

        /// <summary>
        /// 修改地址中参数的值，若原地址中不存在该参数则添加该参数
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="parameterName">要修改的参数名</param>
        /// <param name="newValue">要修改的值</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string AlterParameterValue(this IUrlHelper url, string parameterName, string newValue)
        {
            string newUrl = string.Empty;
            //获取地址中参数集合
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);

            //获取地址中QueryString集合
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys?.ToArray();
            if (routeValueDictionary.Keys.Contains(parameterName))
            {
                routeValueDictionary[parameterName] = newValue;
            }

            foreach (string qk in allQueryKeys)
            {
                if (qk == parameterName)
                {
                    routeValueDictionary.Add(qk, newValue);
                }
                else
                {
                    routeValueDictionary.Add(qk, collection[qk].TooString());
                }
            }

            if (!routeValueDictionary.ContainsKey(parameterName) && !string.IsNullOrEmpty(parameterName))
            {
                routeValueDictionary.Add(parameterName, newValue);
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 修改地址中指定参数的值并重定向到指定的action和controller，若地址中不存在该参数则添加该参数
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="action">重定向到的Action</param>
        /// <param name="controller">重定向到的Controller</param>
        /// <param name="parameterName">要修改的参数名</param>
        /// <param name="newValue">修改后的参数值</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string AlterParameterValue(this IUrlHelper url, string action, string controller, string parameterName, string newValue)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            if (routeValueDictionary.Keys.Contains(parameterName))
            {
                routeValueDictionary[parameterName] = newValue;
            }
            if (!string.IsNullOrEmpty(action))
            {
                routeValueDictionary["action"] = action;
            }
            if (!string.IsNullOrEmpty(controller))
            {
                routeValueDictionary["controller"] = controller;
            }
            foreach (string qk in allQueryKeys)
            {
                if (qk == parameterName)
                {
                    routeValueDictionary.Add(qk, parameterName);
                }
                else
                {
                    routeValueDictionary.Add(qk, collection[qk].TooString());
                }
            }
            if (!routeValueDictionary.ContainsKey(parameterName) && !string.IsNullOrEmpty(parameterName))
            {
                routeValueDictionary.Add(parameterName, newValue);
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 重定向到指定的Action和Controller,地址中的参数将保留
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="action">重定向的Action</param>
        /// <param name="controller">重定向的Controller</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string SaveParameterAction(this IUrlHelper url, string action, string controller)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            if (!string.IsNullOrEmpty(action))
            {
                routeValueDictionary["action"] = action;
            }
            if (!string.IsNullOrEmpty(controller))
            {
                routeValueDictionary["controller"] = action;
            }
            foreach (string qk in allQueryKeys)
            {
                routeValueDictionary.Add(qk, collection[qk].TooString());
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 修改指定参数的值并删除指定的参数，若要修改的参数不存在则添加该参数
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="alterParameterName">需要修改值的参数</param>
        /// <param name="newValue">要修改的值</param>
        /// <param name="deleteParameterNames">需要删除参数名集合</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string AlterAndDeleteParameter(this IUrlHelper url, string alterParameterName, string newValue, params string[] deleteParameterNames)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            if (routeValueDictionary.Keys.Contains(alterParameterName))
            {
                routeValueDictionary[alterParameterName] = newValue;
            }
            foreach (string qk in allQueryKeys)
            {
                if (qk == alterParameterName)
                {
                    routeValueDictionary.Add(qk, newValue);
                }
                else
                {
                    routeValueDictionary.Add(qk, collection[qk].TooString());
                }
            }
            if (deleteParameterNames != null && deleteParameterNames.Length > 0)
            {
                foreach (string deleteKey in deleteParameterNames)
                {
                    if (routeValueDictionary.Keys.Contains(deleteKey))
                    {
                        routeValueDictionary.Remove(deleteKey);
                    }
                }
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 修改指定参数并删除指定的参数，重定向指定的action和controller，若要修改的参数不存在则添加该参数
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="action">重定向的Action</param>
        /// <param name="controller">修改的Controller</param>
        /// <param name="alterParameterName">需要修改值的参数</param>
        /// <param name="newValue">要修改的值</param>
        /// <param name="deleteParameterNames">需要删除参数名集合</param>
        /// <returns>返回修改后的参数</returns>
        public static string AlterAndDeleteParameter(this IUrlHelper url, string action, string controller, string alterParameterName, string newValue, params string[] deleteParameterNames)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            if (routeValueDictionary.Keys.Contains(alterParameterName))
            {
                routeValueDictionary[alterParameterName] = newValue;
            }
            if (!string.IsNullOrEmpty(action))
            {
                routeValueDictionary["action"] = action;
            }
            if (!string.IsNullOrEmpty(controller))
            {
                routeValueDictionary["controller"] = controller;
            }
            foreach (string qk in allQueryKeys)
            {
                if (qk == alterParameterName)
                {
                    routeValueDictionary.Add(qk, newValue);
                }
                else
                {
                    routeValueDictionary.Add(qk, collection[qk].TooString());
                }
            }
            if (!routeValueDictionary.ContainsKey(alterParameterName) && !string.IsNullOrEmpty(alterParameterName))
            {
                routeValueDictionary.Add(alterParameterName, newValue);
            }

            if (deleteParameterNames != null && deleteParameterNames.Length > 0)
            {
                foreach (string deleteKey in deleteParameterNames)
                {
                    if (routeValueDictionary.Keys.Contains(deleteKey))
                    {
                        routeValueDictionary.Remove(deleteKey);
                    }
                }
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 向地址中的参数添加新值，并删除指定的原始值，若已经存在要添加的值将不在添加
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="parameterName">要修改值的参数</param>
        /// <param name="addValue">要添加的值</param>
        /// <param name="deleteValue">要删除的值</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string AddAndDeleteParameterValue(this IUrlHelper url, string parameterName, string addValue, string deleteValue)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            foreach (string qk in allQueryKeys)
            {
                routeValueDictionary.Add(qk, collection[qk].TooString());
            }
            string nowValue = string.Empty;
            if (routeValueDictionary.ContainsKey(parameterName))
            {
                nowValue = routeValueDictionary[parameterName].TooString();
                if (!deleteValue.IsNullOrEmpty())
                {
                    nowValue = nowValue.Replace(deleteValue, string.Empty);
                }
                if (nowValue.IndexOf(addValue) < 0)
                {
                    nowValue += addValue;
                }
                routeValueDictionary[parameterName] = nowValue;
            }
            else
            {
                routeValueDictionary.Add(parameterName, addValue);
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        /// <summary>
        /// 向地址中的参数添加新值，并删除指定的原始值,重定向到指定的Action和Controller，若已经存在要添加的值将不在添加
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="parameterName">要修改值的参数</param>
        /// <param name="addValue">要添加的值</param>
        /// <param name="deleteValue">要删除的值</param>
        /// <returns>返回修改后的Url地址</returns>
        public static string AddAndDeleteParameterValue(this IUrlHelper url, string action, string controller, string parameterName, string addValue, string deleteValue)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            foreach (string qk in allQueryKeys)
            {
                routeValueDictionary.Add(qk, collection[qk].TooString());
            }
            if (!string.IsNullOrEmpty(action))
            {
                routeValueDictionary["action"] = action;
            }
            if (!string.IsNullOrEmpty(controller))
            {
                routeValueDictionary["controller"] = controller;
            }
            string nowValue = string.Empty;
            if (routeValueDictionary.ContainsKey(parameterName))
            {
                nowValue = routeValueDictionary[parameterName].TooString();
            }
            if (!string.IsNullOrEmpty(nowValue))
            {
                nowValue.Replace(deleteValue, string.Empty);
                if (nowValue.IndexOf(addValue) < 0)
                {
                    nowValue += addValue;
                }
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            newUrl = newUrl.Trim(new char[] { '&', '?' });
            return newUrl;
        }

        /// <summary>
        /// 获取地址中参数的值
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="parameterName">要获取值的参数</param>
        /// <returns>返回参数值</returns>
        public static string ParameterValue(this IUrlHelper url, string parameterName)
        {
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            var collection = url.ActionContext.HttpContext.Request.Query;
            if (routeValueDictionary.ContainsKey(parameterName))
            {
                return routeValueDictionary[parameterName].TooString();
            }
            else if (collection.Keys.Contains(parameterName))
            {
                return collection[parameterName].TooString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 删除地址中的参数
        /// </summary>
        /// <param name="url">IUrlHelper</param>
        /// <param name="parameterName">要删除的参数名</param>
        /// <returns>返回修改后的地址</returns>
        public static string DeleteParameter(this IUrlHelper url, string parameterName)
        {
            string newUrl = string.Empty;
            RouteValueDictionary routeValueDictionary = url.ActionContext.RouteData.Values;
            routeValueDictionary = CopyRouteValueDictionary(routeValueDictionary);
            var collection = url.ActionContext.HttpContext.Request.Query;
            string[] allQueryKeys = collection.Keys.ToArray();
            foreach (string qk in allQueryKeys)
            {
                routeValueDictionary.Add(qk, collection[qk].TooString());
            }
            if (routeValueDictionary.Keys.Contains(parameterName))
            {
                routeValueDictionary.Remove(parameterName);
            }
            newUrl = url.Action(routeValueDictionary["action"].TooString(), routeValueDictionary["controller"].TooString(), routeValueDictionary);
            return newUrl;
        }

        #endregion
    }
}
