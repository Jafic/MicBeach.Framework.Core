using MicBeach.Application.Identity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Cmd
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public class AuthorityCmdDto
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
        public AuthorityType AuthType
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
        public AuthorityGroupCmdDto AuthGroup
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
