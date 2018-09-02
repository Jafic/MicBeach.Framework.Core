using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 用户授权
    /// </summary>
    [Serializable]
    public class UserAuthorizeEntity : CommandEntity<UserAuthorizeEntity>
    {
        #region	字段

        /// <summary>
        /// 用户
        /// </summary>
        public long User
        {
            get { return valueDic.GetValue<long>("User"); }
            set { valueDic.SetValue("User", value); }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public string Authority
        {
            get { return valueDic.GetValue<string>("Authority"); }
            set { valueDic.SetValue("Authority", value); }
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disable
        {
            get { return valueDic.GetValue<bool>("Disable"); }
            set { valueDic.SetValue("Disable", value); }
        }

        #endregion
    }
}