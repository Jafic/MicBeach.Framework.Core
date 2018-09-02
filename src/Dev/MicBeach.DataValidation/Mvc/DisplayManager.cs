using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Mvc
{
    /// <summary>
    /// 显示管理器
    /// </summary>
    public static class DisplayManager
    {
        static Dictionary<string, DisplayText> displayDic = new Dictionary<string, DisplayText>();

        #region 方法

        /// <summary>
        /// 设置显示名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="displayName">显示名称</param>
        public static void Add(Type type, string propertyName, string displayName)
        {
            if (type == null)
            {
                return;
            }
            SetDisplay(type.FullName,propertyName,displayName);
        }

        /// <summary>
        /// 设置显示名称
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="property">属性</param>
        /// <param name="displayName">显示名称</param>
        public static void Add<T>(Expression<Func<T, dynamic>> property, string displayName)
        {
            Add(typeof(T), ExpressionHelper.GetExpressionText(property),displayName);
        }

        /// <summary>
        /// 设置显示名称
        /// </summary>
        /// <param name="typeFullName">类型名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="displayName">显示名称</param>
        static void SetDisplay(string typeFullName, string propertyName, string displayName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(displayName))
            {
                return;
            }
            string displayKey = GetDisplayKey(typeFullName, propertyName);
            if (displayDic.ContainsKey(displayKey))
            {
                var nowDisplayText = displayDic[displayKey];
                if (nowDisplayText == null)
                {
                    nowDisplayText = new DisplayText();
                }
                nowDisplayText.DisplayName = displayName;
            }
            else
            {
                displayDic.Add(displayKey, new DisplayText()
                {
                    DisplayName = displayName
                });
            }
        }

        static string GetDisplayKey(string typeFullName, string propertyName)
        {
            string displayKey = string.Format("{0}_{1}", typeFullName, propertyName);
            return displayKey;
        }

        /// <summary>
        /// 获取显示
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static DisplayText GetDisplay(Type type, string propertyName)
        {
            if (type == null)
            {
                return null;
            }
            return GetDisplay(type.FullName, propertyName);
        }

        /// <summary>
        /// 获取显示
        /// </summary>
        /// <param name="typeFullname">类型全名</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static DisplayText GetDisplay(string typeFullname, string propertyName)
        {
            string displayKey = GetDisplayKey(typeFullname, propertyName);
            if (displayDic.ContainsKey(displayKey))
            {
                return displayDic[displayKey];
            }
            return null;
        }

        public static Dictionary<string, DisplayText> GetAllDisplay()
        {
            return displayDic;
        }

        #endregion
    }
}
