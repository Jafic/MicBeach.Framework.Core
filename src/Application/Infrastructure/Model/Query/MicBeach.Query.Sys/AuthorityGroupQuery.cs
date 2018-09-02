using MicBeach.Develop.CQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Query.Sys
{
    public class AuthorityGroupQuery: IQueryModel<AuthorityGroupQuery>
    {
        #region	属性

        /// <summary>
        /// 编号
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
        public int SortIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 上级分组
        /// </summary>
        public long Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 分组等级
        /// </summary>
        public int Level
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

        #endregion
    }
}
