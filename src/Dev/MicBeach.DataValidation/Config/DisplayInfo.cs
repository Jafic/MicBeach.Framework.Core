using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 显示信息
    /// </summary>
    public class DisplayInfo
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        [JsonProperty(PropertyName = "displayTxt")]
        public string DisplayName
        {
            get;set;
        }
    }
}
