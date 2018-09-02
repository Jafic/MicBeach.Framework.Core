using MicBeach.Develop.DataValidation;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicBeach.Web.Mvc
{
    public static class ValidationExtensions
    {
        static string validationTipIcoClass = "tip";

        /// <summary>
        /// 图标样式类名
        /// </summary>
        public static string TipIco
        {
            get
            {
                return validationTipIcoClass;
            }
            set
            {
                validationTipIcoClass = value;
            }
        }

        public static IHtmlContent DefaultValidationMessageFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
        {
            if (string.IsNullOrWhiteSpace(validationMessage))
            {
                validationMessage = ValidationManager.GetValidationTipMessage<TModel, TProperty>(expression);
            }
            IDictionary<string, object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            string attrName = "class";
            if (attributes != null && string.IsNullOrWhiteSpace(validationMessage) && attributes.ContainsKey(attrName))
            {
                var attrVal = attributes[attrName];
                if (attrVal != null)
                {
                    attributes[attrName] = attrVal.ToString().Replace(validationTipIcoClass, "");
                }
            }
            return htmlHelper.ValidationMessageFor<TModel, TProperty>(expression, validationMessage, attributes);
        }
    }
}
