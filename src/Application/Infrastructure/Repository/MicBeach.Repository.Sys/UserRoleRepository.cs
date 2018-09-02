using MicBeach.DataAccessContract.Sys;
using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Query.Sys;
using MicBeach.Entity.Sys;
using MicBeach.Develop.CQuery;
using MicBeach.Develop.UnitOfWork;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 用户绑定角色存储
    /// </summary>
    public class UserRoleRepository : IUserRoleRepository
    {
        IUserRoleDataAccess userRoleDal = null;
        public UserRoleRepository()
        {
            userRoleDal = this.Instance<IUserRoleDataAccess>();
        }

        #region 保存管理用户

        /// <summary>
        /// 保存管理用户
        /// </summary>
        /// <param name="userList">用户列表</param>
        public void SaveUserRoleFromUser(IEnumerable<User> userList)
        {
            if (userList.IsNullOrEmpty())
            {
                return;
            }

            #region 管理账户角色

            List<UserRoleEntity> userRoleList = new List<UserRoleEntity>();
            List<AdminUser> adminUserList = userList.Where(c => (c is AdminUser) && c != null).Select(c => (AdminUser)c).ToList();
            List<long> userIds = new List<long>();
            adminUserList.ForEach(a =>
            {
                userIds.Add(a.SysNo);
                if (a.Roles.IsNullOrEmpty())
                {
                    return;
                }
                userRoleList.AddRange(a.Roles.Select(c => new UserRoleEntity()
                {
                    UserSysNo = a.SysNo,
                    RoleSysNo = c.SysNo
                }));
            });

            #endregion

            //移除当前用户绑定的角色
            IQuery removeQuery = QueryFactory.Create<UserRoleQuery>(c => userIds.Contains(c.UserSysNo));
            UnitOfWork.RegisterCommand(userRoleDal.Delete(removeQuery));
            if (!userRoleList.IsNullOrEmpty())
            {
                UnitOfWork.RegisterCommand(userRoleDal.Add(userRoleList).ToArray());
            }
        }

        #endregion

        #region 绑定用户角色

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        public void Save(IEnumerable<Tuple<User, Role>> userRoleBinds)
        {
            if (userRoleBinds.IsNullOrEmpty())
            {
                return;
            }
            List<UserRoleEntity> UserRoleEntitys = new List<UserRoleEntity>();
            IQuery removeQuery = QueryFactory.Create<UserRoleQuery>();
            foreach (var bind in userRoleBinds)
            {
                if (bind.Item1 == null || bind.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<UserRoleQuery>(c => c.UserSysNo == bind.Item1.SysNo && c.RoleSysNo == bind.Item2.SysNo);
                UserRoleEntitys.Add(new UserRoleEntity()
                {
                    UserSysNo = bind.Item1.SysNo,
                    RoleSysNo = bind.Item2.SysNo
                });
            }
            UnitOfWork.RegisterCommand(userRoleDal.Delete(removeQuery));//移除当前
            UnitOfWork.RegisterCommand(userRoleDal.Add(UserRoleEntitys).ToArray());//保存
        }

        #endregion

        #region 解绑用户角色

        /// <summary>
        /// 解绑用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        public void Remove(IEnumerable<Tuple<User, Role>> userRoleBinds)
        {
            if (userRoleBinds.IsNullOrEmpty())
            {
                return;
            }
            IQuery removeQuery = QueryFactory.Create<UserRoleQuery>();
            foreach (var bind in userRoleBinds)
            {
                if (bind.Item1 == null || bind.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<UserRoleQuery>(c => c.UserSysNo == bind.Item1.SysNo && c.RoleSysNo == bind.Item2.SysNo);
            }
            UnitOfWork.RegisterCommand(userRoleDal.Delete(removeQuery));
        }

        #endregion

        #region 根据账户删除账户角色绑定

        /// <summary>
        /// 根据账户删除账户角色绑定
        /// </summary>
        /// <param name="userList">用户信息</param>
        public void RemoveUserRoleByUser(IEnumerable<User> userList)
        {
            if (userList.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> userIds = userList.Select(c => c.SysNo).Distinct();
            IQuery query = QueryFactory.Create<UserRoleQuery>(c => userIds.Contains(c.UserSysNo));
            UnitOfWork.RegisterCommand(userRoleDal.Delete(query));
        }

        #endregion

        #region 根据角色删除账户角色绑定

        /// <summary>
        /// 根据角色删除账户角色绑定
        /// </summary>
        /// <param name="roleList">用户信息</param>
        public void RemoveUserRoleByRole(IEnumerable<Role> roleList)
        {
            if (roleList.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> roleIds = roleList.Select(c => c.SysNo).Distinct();
            IQuery query = QueryFactory.Create<UserRoleQuery>(c => roleIds.Contains(c.RoleSysNo));
            UnitOfWork.RegisterCommand(userRoleDal.Delete(query));
        }

        #endregion
    }
}
