using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 属性字段规则
    /// </summary>
    public class PropertyRule
    {
        #region 属性

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;set;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        [JsonProperty(PropertyName = "rules")]
        public List<ValidatorRule> Rules
        {
            get;set;
        }

        #endregion
    }
}
