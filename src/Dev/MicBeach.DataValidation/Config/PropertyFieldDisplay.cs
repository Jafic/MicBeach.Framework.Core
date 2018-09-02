using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 属性字段显示
    /// </summary>
    public class PropertyFieldDisplay
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
        /// 显示信息
        /// </summary>
        [JsonProperty(PropertyName = "display")]
        public DisplayInfo Display
        {
            get;set;
        }

        #endregion
    }
}
