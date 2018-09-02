using MicBeach.DataAccessContract.Sys;
using MicBeach.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.DataAccess;

namespace MicBeach.DataAccess.Sys
{
    /// <summary>
    /// 权限数据访问
    /// </summary>
    public class AuthorityDataAccess : RdbDataAccess<AuthorityEntity>, IAuthorityDataAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Code", "Name", "AuthType", "Status", "Sort", "AuthGroup", "Remark", "CreateDate" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Code", "Name", "AuthType", "Status", "Sort", "AuthGroup", "Remark", "CreateDate" };
        }

        #endregion
    }
}
