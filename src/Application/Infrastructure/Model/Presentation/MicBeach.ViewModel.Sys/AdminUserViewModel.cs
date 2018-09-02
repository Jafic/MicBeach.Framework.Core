using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Sys
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class AdminUserViewModel : UserViewModel
    {
        #region	属性

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<RoleViewModel> Roles
        {
            get; set;
        }

        #endregion
    }
}
