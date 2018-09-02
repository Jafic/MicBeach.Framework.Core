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
using MicBeach.Develop.Domain.Repository;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 权限存储
    /// </summary>
    public class AuthorityRepository : DefaultRepository<Authority, AuthorityEntity, IAuthorityDataAccess>, IAuthorityRepository
    {
        #region 根据分组删除权限

        /// <summary>
        /// 根据分组删除权限
        /// </summary>
        /// <param name="groups">分组信息</param>
        public void RemoveAuthorityByGroup(IEnumerable<AuthorityGroup> groups)
        {
            if (groups.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> groupIds = groups.Select(c => c.SysNo).Distinct().ToList();
            IQuery query = QueryFactory.Create<AuthorityQuery>(c => groupIds.Contains(c.AuthGroup));
            Remove(query);
        }
        #endregion

        #region 事件绑定

        protected override void BindEvent()
        {
            base.BindEvent();

            #region 权限操作绑定

            RemoveEvent += this.Instance<IAuthorityBindOperationRepository>().DeleteBindOperationByAuthority;//删除权限时删除对应的操作绑定关系

            #endregion

            #region 角色授权

            RemoveEvent += this.Instance<IRoleAuthorizeRepository>().RemoveRoleAuthorizeByAuthority;//移除权限时移除角色授权

            #endregion

            #region 用户授权

            RemoveEvent += this.Instance<IUserAuthorizeRepository>().RemoveUserAuthorizeByAuthority;//移除权限时移除用户授权

            #endregion
        }

        #endregion
    }
}
