using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Serializable]
    public class UserRoleEntity : CommandEntity<UserRoleEntity>
    {
        #region	字段

        /// <summary>
        /// 用户
        /// </summary>
        public long UserSysNo
        {
            get { return valueDic.GetValue<long>("UserSysNo"); }
            set { valueDic.SetValue("UserSysNo", value); }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public long RoleSysNo
        {
            get { return valueDic.GetValue<long>("RoleSysNo"); }
            set { valueDic.SetValue("RoleSysNo", value); }
        }

        #endregion
    }
}