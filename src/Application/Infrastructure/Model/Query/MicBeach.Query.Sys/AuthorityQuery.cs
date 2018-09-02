using MicBeach.Application.Identity.Auth;
using MicBeach.Develop.CQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Query.Sys
{
    /// <summary>
    /// 权限查询对象
    /// </summary>
    public class AuthorityQuery: IQueryModel<AuthorityQuery>
    {
        #region	属性

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code
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
        public int AuthType
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityStatus Status
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
        public long AuthGroup
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
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        #endregion
    }
}
