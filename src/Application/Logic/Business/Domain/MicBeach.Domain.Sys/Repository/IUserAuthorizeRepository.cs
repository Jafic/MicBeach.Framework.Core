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
    /// 用户授权存储
    /// </summary>
    public interface IUserAuthorizeRepository: IRepository<UserAuthorize>
    {
        #region 删除权限数据并删除相应的用户授权

        /// <summary>
        /// 删除权限数据并删除相应的用户授权
        /// </summary>
        /// <param name="authoritys">授权信息</param>
        void RemoveUserAuthorizeByAuthority(IEnumerable<Authority> authoritys);

        #endregion

        #region 删除用户数据删除相应的用户授权

        /// <summary>
        /// 删除用户数据删除相应的用户授权
        /// </summary>
        /// <param name="users">用户信息</param>
        void RemoveUserAuthorizeByUser(IEnumerable<User> users);

        #endregion
    }
}
