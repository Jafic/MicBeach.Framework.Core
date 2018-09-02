using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Query
{
    /// <summary>
    /// 管理账户数据传输对象
    /// </summary>
    public class AdminUserDto:UserDto
    {
        #region 属性

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<RoleDto> Roles
        {
            get; set;
        }

        #endregion
    }
}
