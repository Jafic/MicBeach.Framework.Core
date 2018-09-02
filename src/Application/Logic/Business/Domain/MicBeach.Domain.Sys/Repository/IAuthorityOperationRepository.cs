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
    /// 授权操作存储
    /// </summary>
    public interface IAuthorityOperationRepository : IRepository<AuthorityOperation>
    {
        #region 根据操作分组删除分组下的授权操作

        /// <summary>
        /// 根据操作分组删除分组下的授权操作
        /// </summary>
        /// <param name="groups">要移除操作的分组</param>
        void RemoveOperationByGroup(IEnumerable<AuthorityOperationGroup> groups);

        #endregion
    }
}
