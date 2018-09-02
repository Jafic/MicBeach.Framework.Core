using MicBeach.Develop.CQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Query.Sys
{
    /// <summary>
    /// 用户角色查询
    /// </summary>
    public class UserRoleQuery: IQueryModel<UserRoleQuery>
    {
        #region 属性

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserSysNo
        {
            get; set;
        }

        /// <summary>
        /// 角色编号
        /// </summary>
        public long RoleSysNo
        {
            get;set;
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public static string ObjectName
        {
            get
            {
                return "Sys_UserRole";
            }
        }

        #endregion
    }
}
