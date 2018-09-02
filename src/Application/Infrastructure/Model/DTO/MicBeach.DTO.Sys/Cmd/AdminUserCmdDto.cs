using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Cmd
{
    /// <summary>
    /// 管理用户信息
    /// </summary>
    public class AdminUserCmdDto:UserCmdDto
    {
        #region 属性

        /// <summary>
        /// 账户角色
        /// </summary>
        public List<RoleCmdDto> Roles
        {
            get; set;
        }

        #endregion
    }
}
