using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 权限&授权操作关联
    /// </summary>
    [Serializable]
    public class AuthorityBindOperationEntity : CommandEntity<AuthorityBindOperationEntity>
    {
        #region	字段

        /// <summary>
        /// 授权操作
        /// </summary>
        public long AuthorithOperation
        {
            get { return valueDic.GetValue<long>("AuthorithOperation"); }
            set { valueDic.SetValue("AuthorithOperation", value); }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public string AuthorityCode
        {
            get { return valueDic.GetValue<string>("AuthorityCode"); }
            set { valueDic.SetValue("AuthorityCode", value); }
        }

        #endregion
    }
}