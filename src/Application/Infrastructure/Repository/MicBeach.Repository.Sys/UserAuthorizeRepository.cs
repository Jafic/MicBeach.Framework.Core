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
using MicBeach.Application.Identity.Auth;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Develop.CQuery;
using MicBeach.Develop.UnitOfWork;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 用户授权存储
    /// </summary>
    public class UserAuthorityRepository : DefaultRepository<UserAuthorize, UserAuthorizeEntity, IUserAuthorizeDataAccess>, IUserAuthorizeRepository
    {
        IUserAuthorizeDataAccess userAuthorityDataAccess = null;

        public UserAuthorityRepository()
        {
            userAuthorityDataAccess = this.Instance<IUserAuthorizeDataAccess>();
        }

        #region 删除权限数据并删除相应的用户授权

        /// <summary>
        /// 删除权限数据并删除相应的用户授权
        /// </summary>
        /// <param name="authoritys">授权信息</param>
        public void RemoveUserAuthorizeByAuthority(IEnumerable<Authority> authoritys)
        {
            if (authoritys.IsNullOrEmpty())
            {
                return;
            }
            var authCodes = authoritys.Select(c => c.Code).Distinct();
            IQuery removeQuery = QueryFactory.Create<UserAuthorizeQuery>(a => authCodes.Contains(a.Authority));
            UnitOfWork.RegisterCommand(userAuthorityDataAccess.Delete(removeQuery));
        }

        #endregion

        #region 删除用户数据删除相应的用户授权

        /// <summary>
        /// 删除用户数据删除相应的用户授权
        /// </summary>
        /// <param name="users">用户信息</param>
        public void RemoveUserAuthorizeByUser(IEnumerable<User> users)
        {
            if (users.IsNullOrEmpty())
            {
                return;
            }
            var userIds = users.Select(c => c.SysNo).Distinct();
            IQuery removeQuery = QueryFactory.Create<UserAuthorizeQuery>(a => userIds.Contains(a.User));
            UnitOfWork.RegisterCommand(userAuthorityDataAccess.Delete(removeQuery));
        }

        #endregion
    }
}
