using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Sys.Repository
{
    /// <summary>
    /// 用户角色绑定存储管理
    /// </summary>
    public interface IUserRoleRepository
    {
        #region 保存用户绑定的角色

        /// <summary>
        /// 保存用户绑定的角色
        /// </summary>
        /// <param name="userList">用户列表</param>
        void SaveUserRoleFromUser(IEnumerable<User> userList);

        #endregion

        #region 绑定用户角色

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        void Save(IEnumerable<Tuple<User, Role>> userRoleBinds);

        #endregion

        #region 解绑用户角色

        /// <summary>
        /// 解绑用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        void Remove(IEnumerable<Tuple<User, Role>> userRoleBinds);

        #endregion

        #region 删除用户绑定的用户角色

        /// <summary>
        /// 删除用户绑定的用户角色
        /// </summary>
        /// <param name="userList">用户信息</param>
        void RemoveUserRoleByUser(IEnumerable<User> userList);

        #endregion

        #region 根据角色删除账户角色绑定

        /// <summary>
        /// 根据角色删除账户角色绑定
        /// </summary>
        /// <param name="roleList">用户信息</param>
        void RemoveUserRoleByRole(IEnumerable<Role> roleList);

        #endregion
    }
}
