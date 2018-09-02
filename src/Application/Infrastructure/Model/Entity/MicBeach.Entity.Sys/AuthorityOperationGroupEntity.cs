using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 授权操作组
    /// </summary>
    [Serializable]
    public class AuthorityOperationGroupEntity : CommandEntity<AuthorityOperationGroupEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        public long SysNo
        {
            get { return valueDic.GetValue<long>("SysNo"); }
            set { valueDic.SetValue("SysNo", value); }
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return valueDic.GetValue<int>("Sort"); }
            set { valueDic.SetValue("Sort", value); }
        }

        /// <summary>
        /// 上级
        /// </summary>
        public long Parent
        {
            get { return valueDic.GetValue<long>("Parent"); }
            set { valueDic.SetValue("Parent", value); }
        }

        /// <summary>
        /// 根组
        /// </summary>
        public long Root
        {
            get { return valueDic.GetValue<long>("Root"); }
            set { valueDic.SetValue("Root", value); }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get { return valueDic.GetValue<int>("Level"); }
            set { valueDic.SetValue("Level", value); }
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
        /// 说明
        /// </summary>
        public string Remark
        {
            get { return valueDic.GetValue<string>("Remark"); }
            set { valueDic.SetValue("Remark", value); }
        }

        #endregion
    }
}