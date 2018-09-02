using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 权限
    /// </summary>
    [Serializable]
    public class AuthorityEntity : CommandEntity<AuthorityEntity>
    {
        #region	字段

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code
        {
            get { return valueDic.GetValue<string>("Code"); }
            set { valueDic.SetValue("Code", value); }
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
        /// 权限类型
        /// </summary>
        public int AuthType
        {
            get { return valueDic.GetValue<int>("AuthType"); }
            set { valueDic.SetValue("AuthType", value); }
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
        /// 权限分组
        /// </summary>
        public long AuthGroup
        {
            get { return valueDic.GetValue<long>("AuthGroup"); }
            set { valueDic.SetValue("AuthGroup", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get { return valueDic.GetValue<string>("Remark"); }
            set { valueDic.SetValue("Remark", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return valueDic.GetValue<DateTime>("CreateDate"); }
            set { valueDic.SetValue("CreateDate", value); }
        }

        #endregion
    }
}