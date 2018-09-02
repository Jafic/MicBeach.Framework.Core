using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Sys
{
    /// <summary>
    /// 用户授权
    /// </summary>
    public class UserAuthorizeQuery : IQueryModel<UserAuthorizeQuery>
    {
        #region	属性

        /// <summary>
        /// 用户
        /// </summary>
        public long User
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

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disable
        {
            get;
            set;
        }

        #endregion
    }
}