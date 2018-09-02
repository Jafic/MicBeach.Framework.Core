using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 类型显示
    /// </summary>
    public class TypeDisplay
    {
        #region 属性

        /// <summary>
        /// 类型名称
        /// </summary>
        [JsonProperty(PropertyName = "typeName")]
        public string TypeFullName
        {
            get;set;
        }

        /// <summary>
        /// 字段或属性
        /// </summary>
        [JsonProperty(PropertyName = "displays")]
        public List<PropertyFieldDisplay> Propertys
        {
            get;set;
        }

        #endregion
    }
}
