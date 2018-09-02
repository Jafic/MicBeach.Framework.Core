using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Application.Identity.User
{
    #region 账户

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum UserType
    {
        管理账户 = 210
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        正常 = 310,
        锁定 = 320
    }

    #endregion

    #region 角色

    /// <summary>
    /// 角色状态
    /// </summary>
    public enum RoleStatus
    {
        正常 = 310,
        禁用 = 320
    }

    #endregion
}
