using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Sys.Filter
{
    /// <summary>
    /// 用户筛选信息
    /// </summary>
    public class AdminUserFilterViewModel: UserFilterViewModel
    {
        #region 数据筛选

        /// <summary>
        /// 角色筛选
        /// </summary>
        public RoleFilterViewModel RoleFilter
        {
            get;set;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载用户角色数据
        /// </summary>
        public bool LoadRole
        {
            get;set;
        }

        #endregion
    }
}
