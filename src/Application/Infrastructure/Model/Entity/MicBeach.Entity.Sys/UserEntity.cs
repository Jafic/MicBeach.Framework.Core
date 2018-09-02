using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 管理用户
    /// </summary>
    [Serializable]
    public class UserEntity : CommandEntity<UserEntity>
    {
        #region	字段

        /// <summary>
        /// 用户编号
        /// </summary>
        public long SysNo
        {
            get { return valueDic.GetValue<long>("SysNo"); }
            set { valueDic.SetValue("SysNo", value); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return valueDic.GetValue<string>("UserName"); }
            set { valueDic.SetValue("UserName", value); }
        }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName
        {
            get { return valueDic.GetValue<string>("RealName"); }
            set { valueDic.SetValue("RealName", value); }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return valueDic.GetValue<string>("Pwd"); }
            set { valueDic.SetValue("Pwd", value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int UserType
        {
            get { return valueDic.GetValue<int>("UserType"); }
            set { valueDic.SetValue("UserType", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return valueDic.GetValue<int>("Status"); }
            set { valueDic.SetValue("Status", value); }
        }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            get { return valueDic.GetValue<string>("Mobile"); }
            set { valueDic.SetValue("Mobile", value); }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get { return valueDic.GetValue<string>("Email"); }
            set { valueDic.SetValue("Email", value); }
        }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            get { return valueDic.GetValue<string>("QQ"); }
            set { valueDic.SetValue("QQ", value); }
        }

        /// <summary>
        /// 超级管理员
        /// </summary>
        public bool SuperUser
        {
            get { return valueDic.GetValue<bool>("SuperUser"); }
            set { valueDic.SetValue("SuperUser", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return valueDic.GetValue<DateTime>("CreateDate"); }
            set { valueDic.SetValue("CreateDate", value); }
        }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LastLoginDate
        {
            get { return valueDic.GetValue<DateTime>("LastLoginDate"); }
            set { valueDic.SetValue("LastLoginDate", value); }
        }

        #endregion
    }
}