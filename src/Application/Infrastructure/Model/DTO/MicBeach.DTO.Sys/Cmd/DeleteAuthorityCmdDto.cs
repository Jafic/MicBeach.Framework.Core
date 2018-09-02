using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Cmd
{
    /// <summary>
    /// 权限删除命令信息
    /// </summary>
    public class DeleteAuthorityCmdDto
    {
        #region 属性

        /// <summary>
        /// 权限编码
        /// </summary>
        public IEnumerable<string> AuthorityCodes
        {
            get;set;
        }

        #endregion
    }
}
