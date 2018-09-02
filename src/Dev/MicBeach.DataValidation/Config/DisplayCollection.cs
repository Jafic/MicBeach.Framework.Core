using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 显示集合
    /// </summary>
    public class DisplayCollection
    {
        #region 属性

        /// <summary>
        /// 类型信息
        /// </summary>
        [JsonProperty(PropertyName = "types")]
        public List<TypeDisplay> Types
        {
            get; set;
        }

        #endregion
    }
}
