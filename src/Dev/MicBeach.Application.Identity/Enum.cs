using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Application.Identity
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public enum IdentityGroup
    {
        角色 = 110,
        用户 = 111,
        权限分组 = 112,
        权限 = 113,
        授权操作分组 = 114,
        授权操作 = 115
    }
}
