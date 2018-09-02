using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Sys.Query.Filter
{
    /// <summary>
    /// 管理用户筛选
    /// </summary>
    public class AdminUserFilterDto : UserFilterDto
    {
        #region 数据筛选

        /// <summary>
        /// 角色筛选
        /// </summary>
        public RoleFilterDto RoleFilter
        {
            get; set;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载用户角色数据
        /// </summary>
        public bool LoadRole
        {
            get; set;
        }

        #endregion
    }
}
