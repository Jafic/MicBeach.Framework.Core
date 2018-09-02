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
using MicBeach.Query.Sys;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Develop.CQuery;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 角色存储
    /// </summary>
    public class RoleRepository : DefaultRepository<Role, RoleEntity, IRoleDataAccess>, IRoleRepository
    {
        #region 获取用户绑定的角色

        /// <summary>
        /// 获取用户绑定的角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public List<Role> GetUserBindRole(long userId)
        {
            if (userId <= 0)
            {
                return new List<Role>(0);
            }
            var userRoleDal = this.Instance<IUserRoleDataAccess>();
            List<UserRoleEntity> userRoleBindList = userRoleDal.GetList(QueryFactory.Create<UserRoleQuery>(u => u.UserSysNo == userId));
            if (userRoleBindList.IsNullOrEmpty())
            {
                return new List<Role>(0);
            }
            IEnumerable<long> roleIds = userRoleBindList.Select(c => c.RoleSysNo).Distinct().ToList();
            IQuery roleQuery = QueryFactory.Create<RoleQuery>(r => roleIds.Contains(r.SysNo));
            return GetList(roleQuery);
        }

        #endregion

        #region 事件绑定

        protected override void BindEvent()
        {
            #region 角色用户

            IUserRoleRepository userRoleRepository = this.Instance<IUserRoleRepository>();
            RemoveEvent += userRoleRepository.RemoveUserRoleByRole;//移除用户角色绑定

            #endregion

            #region 角色授权

            IRoleAuthorizeRepository roleAuthorizeRepository = this.Instance<IRoleAuthorizeRepository>();
            RemoveEvent += roleAuthorizeRepository.RemoveRoleAuthorizeByRole;//移除角色授权

            #endregion
        }

        #endregion
    }
}
