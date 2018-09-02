using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 类型规则
    /// </summary>
    public class TypeRule
    {
        #region 属性

        /// <summary>
        /// 类型全称
        /// </summary>
        [JsonProperty(PropertyName = "typeName")]
        public string TypeFullName
        {
            get;set;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        [JsonProperty(PropertyName = "rules")]
        public List<PropertyRule> Rules
        {
            get;set;
        }

        #endregion
    }
}
