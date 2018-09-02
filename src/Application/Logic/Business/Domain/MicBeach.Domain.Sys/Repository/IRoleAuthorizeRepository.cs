using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Sys.Repository
{
    /// <summary>
    /// 角色权限存储
    /// </summary>
    public interface IRoleAuthorizeRepository
    {
        #region 保存角色授权

        /// <summary>
        /// 保存角色授权
        /// </summary>
        /// <param name="roleAuths">角色权限信息</param>
        void Save(IEnumerable<Tuple<Role, Authority>> roleAuths);

        #endregion

        #region 移除角色授权

        /// <summary>
        /// 移除角色授权
        /// </summary>
        /// <param name="roleAuths">角色权限信息</param>
        void Remove(IEnumerable<Tuple<Role, Authority>> roleAuths);

        #endregion

        #region 删除权限数据并删除相应的角色授权

        /// <summary>
        /// 删除权限数据并删除相应的角色授权
        /// </summary>
        /// <param name="authoritys">授权信息</param>
        void RemoveRoleAuthorizeByAuthority(IEnumerable<Authority> authoritys);

        #endregion

        #region 删除角色数据删除相应的角色授权

        /// <summary>
        /// 删除角色数据删除相应的角色授权
        /// </summary>
        /// <param name="roles">用户信息</param>
        void RemoveRoleAuthorizeByRole(IEnumerable<Role> roles);

        #endregion
    }
}
