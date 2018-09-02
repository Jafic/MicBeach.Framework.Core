using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 权限分组
    /// </summary>
    [Serializable]
    public class AuthorityGroupEntity : CommandEntity<AuthorityGroupEntity>
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
        public int SortIndex
        {
            get { return valueDic.GetValue<int>("SortIndex"); }
            set { valueDic.SetValue("SortIndex", value); }
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
        /// 上级分组
        /// </summary>
        public long Parent
        {
            get { return valueDic.GetValue<long>("Parent"); }
            set { valueDic.SetValue("Parent", value); }
        }

        /// <summary>
        /// 分组等级
        /// </summary>
        public int Level
        {
            get { return valueDic.GetValue<int>("Level"); }
            set { valueDic.SetValue("Level", value); }
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