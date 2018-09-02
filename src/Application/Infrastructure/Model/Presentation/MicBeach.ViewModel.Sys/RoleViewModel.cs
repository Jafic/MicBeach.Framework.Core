using MicBeach.Application.Identity.User;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MicBeach.ViewModel.Sys
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleViewModel
    {
        #region	属性

        /// <summary>
        /// 角色编号
        /// </summary>
        public long SysNo
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Remote("CheckRoleName", "Sys", AdditionalFields = "SysNo", ErrorMessage = "角色名已存在",HttpMethod ="post")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 上级
        /// </summary>
        public RoleViewModel Parent
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
        public RoleStatus Status
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