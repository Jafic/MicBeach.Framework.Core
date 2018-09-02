using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 角色
    /// </summary>
    [Serializable]
    public class RoleEntity : CommandEntity<RoleEntity>
    {
        #region	字段

        /// <summary>
        /// 角色编号
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
        /// 等级
        /// </summary>
        public int Level
        {
            get { return valueDic.GetValue<int>("Level"); }
            set { valueDic.SetValue("Level", value); }
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
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return valueDic.GetValue<DateTime>("CreateDate"); }
            set { valueDic.SetValue("CreateDate", value); }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark
        {
            get { return valueDic.GetValue<string>("Remark"); }
            set { valueDic.SetValue("Remark", value); }
        }

        #endregion
    }
}