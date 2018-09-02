using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Sys.Service.Parameter
{
    /// <summary>
    /// 修改角色权限绑定
    /// </summary>
    public class ModifyRoleAuthorize
    {
        #region 属性

        /// <summary>
        /// 绑定信息
        /// </summary>
        public IEnumerable<Tuple<Role, Authority>> Binds
        {
            get;set;
        }

        /// <summary>
        /// 解绑信息
        /// </summary>
        public IEnumerable<Tuple<Role, Authority>> UnBinds
        {
            get; set;
        }

        #endregion
    }
}
