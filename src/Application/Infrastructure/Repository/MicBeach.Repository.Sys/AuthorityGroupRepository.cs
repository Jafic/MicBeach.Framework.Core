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
    /// 权限分组存储
    /// </summary>
    public class AuthorityGroupRepository : DefaultRepository<AuthorityGroup, AuthorityGroupEntity, IAuthorityGroupDataAccess>, IAuthorityGroupRepository
    {
        #region 事件绑定

        protected override void BindEvent()
        {
            base.BindEvent();

            #region 权限信息

            RemoveEvent += this.Instance<IAuthorityRepository>().RemoveAuthorityByGroup;//删除分组时删除下面的权限

            #endregion
        }

        #endregion
    }
}
