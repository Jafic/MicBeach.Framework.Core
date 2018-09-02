using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace MicBeach.Web.Config
{
    /// <summary>
    /// 上传配置
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// 配置选项
        /// </summary>
        public Dictionary<string, UploadConfigOption> Options
        {
            get; set;
        } = new Dictionary<string, UploadConfigOption>();

        /// <summary>
        /// 获取默认第一条配置
        /// </summary>
        /// <returns></returns>
        public UploadConfigOption GetFirstDefaultOption()
        {
            if (Options == null || Options.Count <= 0)
            {
                return null;
            }
            return Options.ElementAt(0).Value;
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public UploadConfigOption GetOption(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || Options == null || !Options.ContainsKey(key))
            {
                return null;
            }
            return Options[key];
        }
    }
}
