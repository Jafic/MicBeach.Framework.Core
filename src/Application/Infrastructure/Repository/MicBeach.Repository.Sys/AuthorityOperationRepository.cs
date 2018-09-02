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
using MicBeach.Query.Sys;
using MicBeach.Develop.CQuery;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 授权操作存储
    /// </summary>
    public class AuthorityOperationRepository : DefaultRepository<AuthorityOperation, AuthorityOperationEntity, IAuthorityOperationDataAccess>, IAuthorityOperationRepository
    {
        #region 根据操作分组删除分组下的授权操作

        /// <summary>
        /// 根据操作分组删除分组下的授权操作
        /// </summary>
        /// <param name="groups">要移除操作的分组</param>
        public void RemoveOperationByGroup(IEnumerable<AuthorityOperationGroup> groups)
        {
            if (groups.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> groupIds = groups.Select(c => c.SysNo).Distinct().ToList();
            IQuery query = QueryFactory.Create<AuthorityOperationQuery>(c => groupIds.Contains(c.Group));
            Remove(query);
        }

        #endregion

        #region 绑定事件

        protected override void BindEvent()
        {
            base.BindEvent();

            #region 操作权限绑定

            RemoveEvent += this.Instance<IAuthorityBindOperationRepository>().DeleteBindAuthorityByOperation;//删除操作时删除对应的权限绑定关系

            #endregion
        }

        #endregion
    }
}
