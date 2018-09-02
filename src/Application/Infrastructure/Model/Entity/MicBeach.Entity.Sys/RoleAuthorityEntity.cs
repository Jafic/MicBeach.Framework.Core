using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Serializable]
    public class RoleAuthorizeEntity : CommandEntity<RoleAuthorizeEntity>
    {
        #region	字段

        /// <summary>
        /// 角色
        /// </summary>
        public long Role
        {
            get { return valueDic.GetValue<long>("Role"); }
            set { valueDic.SetValue("Role", value); }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public string Authority
        {
            get { return valueDic.GetValue<string>("Authority"); }
            set { valueDic.SetValue("Authority", value); }
        }

        #endregion
    }
}