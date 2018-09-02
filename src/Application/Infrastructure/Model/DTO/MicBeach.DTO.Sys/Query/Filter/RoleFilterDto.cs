using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Query.Filter
{
    /// <summary>
    /// 角色信息筛选
    /// </summary>
    public class RoleFilterDto: PagingFilter
    {
        #region	属性

        /// <summary>
        /// 角色编号
        /// </summary>
        public List<long> SysNos
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
        /// 等级
        /// </summary>
        public int? Level
        {
            get;
            set;
        }

        /// <summary>
        /// 上级
        /// </summary>
        public long? Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int? SortIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 所属应用
        /// </summary>
        public string Application
        {
            get;
            set;
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        #endregion
    }
}
