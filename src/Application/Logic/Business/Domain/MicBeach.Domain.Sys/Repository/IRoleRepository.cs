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
    /// 角色存储
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        #region 获取用户绑定的角色

        /// <summary>
        /// 获取用户绑定的角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        List<Role> GetUserBindRole(long userId);

        #endregion
    }
}
