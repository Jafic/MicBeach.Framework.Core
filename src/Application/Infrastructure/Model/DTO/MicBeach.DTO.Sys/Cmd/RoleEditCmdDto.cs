
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Cmd
{
    /// <summary>
    /// 角色编辑信息
    /// </summary>
    public class RoMicBeachditCmdDto
    {
        #region 属性

        /// <summary>
        /// 角色基础信息
        /// </summary>
        public RoleCmdDto Role
        {
            get;set;
        }

        #endregion
    }
}
