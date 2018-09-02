using System;

namespace MicBeach.Entity.Sys
{
    /// <summary>
    /// 权限功能分组
    /// </summary>
    [Serializable]
    public class AuthorityFunGroupEntity
    {
        #region	字段

        /// <summary>
        /// 主键编号
        /// </summary>
        public long SysNo
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get;
            set;
        }

        /// <summary>
        /// 权限分组
        /// </summary>
        public long Group
        {
            get;
            set;
        }

        #endregion

        #region	扩展

        /// <summary>
        /// 分页总数
        /// </summary>
        public int PagingTotalCount
        {
            get; set;
        }

        #endregion
    }
}