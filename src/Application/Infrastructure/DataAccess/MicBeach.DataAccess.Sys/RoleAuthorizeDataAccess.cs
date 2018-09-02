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
    /// 角色权限数据访问
    /// </summary>
    public class RoleAuthorizeDataAccess : RdbDataAccess<RoleAuthorizeEntity>, IRoleAuthorizeDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Role", "Authority" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Role", "Authority" };
        }

        #endregion
    }
}
