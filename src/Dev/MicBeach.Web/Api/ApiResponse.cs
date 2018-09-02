using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Api
{
    /// <summary>
    /// Api响应信息
    /// </summary>
    public class ApiResponse
    {
        #region 属性

        /// <summary>
        /// 响应编码
        /// </summary>
        public string Code
        {
            get; set;
        }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message
        {
            get; set;
        }

        #endregion
    }
}
