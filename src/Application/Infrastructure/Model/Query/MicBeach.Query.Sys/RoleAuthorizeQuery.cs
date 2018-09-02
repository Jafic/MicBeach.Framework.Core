using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RoleAuthorizeQuery : IQueryModel<RoleAuthorizeQuery>
    {
        #region	属性

        /// <summary>
        /// 角色
        /// </summary>
        public long Role
        {
            get;
            set;
        }

        /// <summary>
        /// 权限
        /// </summary>
        public string Authority
        {
            get;
            set;
        }

        #endregion
    }
}