using MicBeach.Develop.DataAccess;
using MicBeach.DataAccessContract.Sys;
using MicBeach.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;

namespace MicBeach.DataAccess.Sys
{
    /// <summary>
    /// 用户授权数据访问
    /// </summary>
    public class UserAuthorizeDataAccess : RdbDataAccess<UserAuthorizeEntity>, IUserAuthorizeDataAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "User", "Authority", "Disable" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "User", "Authority", "Disable" };
        }

        #endregion
    }
}
