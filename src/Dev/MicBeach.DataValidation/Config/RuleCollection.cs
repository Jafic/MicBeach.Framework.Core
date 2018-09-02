using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 验证规则集合
    /// </summary>
    public class RuleCollection
    {
        #region 属性

        /// <summary>
        /// 验证规则
        /// </summary>
        [JsonProperty(PropertyName ="rules")]
        public List<TypeRule> Rules
        {
            get;set;
        }

        #endregion
    }
}
