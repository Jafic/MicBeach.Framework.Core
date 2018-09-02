using MicBeach.Application.Identity.Auth;
using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Query.Filter
{
    /// <summary>
    /// 权限筛选信息
    /// </summary>
    public class AuthorityFilterDto: PagingFilter
    {
        #region	属性

        /// <summary>
        /// 权限编码
        /// </summary>
        public List<string> Codes
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
        /// 权限类型
        /// </summary>
        public AuthorityType? AuthType
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityStatus? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort
        {
            get;
            set;
        }

        /// <summary>
        /// 权限分组
        /// </summary>
        public long? AuthGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
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
        /// 添加时间
        /// </summary>
        public DateTime? CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 名称/编码关键字
        /// </summary>
        public string NameCodeMateKey
        {
            get;set;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载分组
        /// </summary>
        public bool LoadGroup
        {
            get; set;
        }

        #endregion
    }
}
