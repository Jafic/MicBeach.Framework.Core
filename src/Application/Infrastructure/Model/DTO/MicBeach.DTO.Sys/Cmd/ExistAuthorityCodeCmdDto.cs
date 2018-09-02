using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Cmd
{
    /// <summary>
    /// 验证权限编码是否存在
    /// </summary>
    public class ExistAuthorityCodeCmdDto
    {
        /// <summary>
        /// 权限编码
        /// </summary>
        public string AuthCode
        {
            get;set;
        }
    }
}
