using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Entity.Sys;
using MicBeach.Util.Extension;
using MicBeach.DataAccessContract.Sys;
using MicBeach.Application.Identity.Auth;
using MicBeach.Develop.CQuery;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Query.Sys;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 角色权限存储
    /// </summary>
    public class RoleAuthorizeRepository : IRoleAuthorizeRepository
    {
        IRoleAuthorizeDataAccess roleAuthorityDataAccess = null;

        public RoleAuthorizeRepository()
        {
            roleAuthorityDataAccess = this.Instance<IRoleAuthorizeDataAccess>();
        }

        #region 保存角色授权

        /// <summary>
        /// 保存角色授权
        /// </summary>
        /// <param name="roleAuths">角色权限信息</param>
        public void Save(IEnumerable<Tuple<Role, Authority>> roleAuths)
        {
            if (roleAuths.IsNullOrEmpty())
            {
                return;
            }
            List<RoleAuthorizeEntity> roleAuthList = new List<RoleAuthorizeEntity>();
            IQuery removeQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            foreach (var roleAuth in roleAuths)
            {
                if (roleAuth == null || roleAuth.Item1 == null || roleAuth.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<RoleAuthorizeQuery>(c => c.Role == roleAuth.Item1.SysNo && c.Authority == roleAuth.Item2.Code);
                roleAuthList.Add(new RoleAuthorizeEntity()
                {
                    Role=roleAuth.Item1.SysNo,
                    Authority=roleAuth.Item2.Code
                });
            }
            UnitOfWork.RegisterCommand(roleAuthorityDataAccess.Delete(removeQuery));//移除当前
            UnitOfWork.RegisterCommand(roleAuthorityDataAccess.Add(roleAuthList).ToArray());//添加
        }

        #endregion

        #region 移除角色授权

        /// <summary>
        /// 移除角色授权
        /// </summary>
        /// <param name="roleAuths">角色权限信息</param>
        public void Remove(IEnumerable<Tuple<Role, Authority>> roleAuths)
        {
            if (roleAuths.IsNullOrEmpty())
            {
                return;
            }
            IQuery removeQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            foreach (var roleAuth in roleAuths)
            {
                if (roleAuth == null || roleAuth.Item1 == null || roleAuth.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<RoleAuthorizeQuery>(c => c.Role == roleAuth.Item1.SysNo && c.Authority == roleAuth.Item2.Code);
            }
            UnitOfWork.RegisterCommand(roleAuthorityDataAccess.Delete(removeQuery));//移除当前
        }

        #endregion

        #region 删除权限数据并删除相应的角色授权

        /// <summary>
        /// 删除权限数据并删除相应的角色授权
        /// </summary>
        /// <param name="authoritys">授权信息</param>
        public void RemoveRoleAuthorizeByAuthority(IEnumerable<Authority> authoritys)
        {
            if (authoritys.IsNullOrEmpty())
            {
                return;
            }
            var authCodes = authoritys.Select(c => c.Code).Distinct();
            IQuery removeQuery = QueryFactory.Create<RoleAuthorizeQuery>(a =>authCodes.Contains(a.Authority));
            UnitOfWork.RegisterCommand(roleAuthorityDataAccess.Delete(removeQuery));
        }

        #endregion

        #region 删除角色数据删除相应的角色授权

        /// <summary>
        /// 删除角色数据删除相应的角色授权
        /// </summary>
        /// <param name="roles">用户信息</param>
        public void RemoveRoleAuthorizeByRole(IEnumerable<Role> roles)
        {
            if (roles.IsNullOrEmpty())
            {
                return;
            }
            var roleIds = roles.Select(c => c.SysNo).Distinct();
            IQuery removeQuery = QueryFactory.Create<RoleAuthorizeQuery>(a => roleIds.Contains(a.Role));
            UnitOfWork.RegisterCommand(roleAuthorityDataAccess.Delete(removeQuery));
        }

        #endregion
    }
}
