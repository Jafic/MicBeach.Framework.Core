using MicBeach.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Develop.DataAccess;
using MicBeach.DataAccessContract.Sys;

namespace MicBeach.DataAccess.Sys
{
    /// <summary>
    /// 权限&授权操作关联数据访问
    /// </summary>
    public class AuthorityBindOperationDataAccess : RdbDataAccess<AuthorityBindOperationEntity>, IAuthorityBindOperationDataAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "AuthorithOperation", "AuthorityCode" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "AuthorithOperation", "AuthorityCode" };
        }

        #endregion
    }
}
