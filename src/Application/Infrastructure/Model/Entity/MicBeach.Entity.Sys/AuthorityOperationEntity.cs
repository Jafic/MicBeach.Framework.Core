using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 授权操作
    /// </summary>
    [Serializable]
    public class AuthorityOperationEntity : CommandEntity<AuthorityOperationEntity>
    {
        #region	字段

        /// <summary>
        /// 主键编号
        /// </summary>
        public long SysNo
        {
            get { return valueDic.GetValue<long>("SysNo"); }
            set { valueDic.SetValue("SysNo", value); }
        }

        /// <summary>
        /// 控制器
        /// </summary>
        public string ControllerCode
        {
            get { return valueDic.GetValue<string>("ControllerCode"); }
            set { valueDic.SetValue("ControllerCode", value); }
        }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string ActionCode
        {
            get { return valueDic.GetValue<string>("ActionCode"); }
            set { valueDic.SetValue("ActionCode", value); }
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Method
        {
            get { return valueDic.GetValue<int>("Method"); }
            set { valueDic.SetValue("Method", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return valueDic.GetValue<string>("Name"); }
            set { valueDic.SetValue("Name", value); }
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return valueDic.GetValue<int>("Sort"); }
            set { valueDic.SetValue("Sort", value); }
        }

        /// <summary>
        /// 操作分组
        /// </summary>
        public long Group
        {
            get { return valueDic.GetValue<long>("Group"); }
            set { valueDic.SetValue("Group", value); }
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public int AuthorizeType
        {
            get { return valueDic.GetValue<int>("AuthorizeType"); }
            set { valueDic.SetValue("AuthorizeType", value); }
        }

        /// <summary>
        /// 方法描述
        /// </summary>
        public string Remark
        {
            get { return valueDic.GetValue<string>("Remark"); }
            set { valueDic.SetValue("Remark", value); }
        }

        #endregion
    }
}