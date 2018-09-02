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

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 授权操作组存储
    /// </summary>
    public class AuthorityOperationGroupRepository : DefaultRepository<AuthorityOperationGroup, AuthorityOperationGroupEntity, IAuthorityOperationGroupDataAccess>, IAuthorityOperationGroupRepository
    {
        #region 事件绑定

        protected override void BindEvent()
        {
            base.BindEvent();

            #region 授权操作

            RemoveEvent += this.Instance<IAuthorityOperationRepository>().RemoveOperationByGroup;//删除分组时删除下面操作信息

            #endregion
        }

        #endregion
    }
}
