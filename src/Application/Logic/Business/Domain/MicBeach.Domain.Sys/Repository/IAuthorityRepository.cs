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
    /// 权限存储
    /// </summary>
    public interface IAuthorityRepository : IRepository<Authority>
    {
        #region 根据分组删除权限

        /// <summary>
        /// 根据分组删除权限
        /// </summary>
        /// <param name="groups">分组信息</param>
        void RemoveAuthorityByGroup(IEnumerable<AuthorityGroup> groups);

        #endregion
    }
}
