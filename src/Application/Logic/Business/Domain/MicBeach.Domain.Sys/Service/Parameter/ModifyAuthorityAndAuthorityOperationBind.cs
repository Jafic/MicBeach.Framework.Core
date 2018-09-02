using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Sys.Service.Parameter
{
    /// <summary>
    /// 修改权限和授权操作绑定
    /// </summary>
    public class ModifyAuthorityAndAuthorityOperationBind
    {
        /// <summary>
        /// 绑定信息
        /// </summary>
        public IEnumerable<Tuple<Authority, AuthorityOperation>> Binds
        {
            get;set;
        }

        /// <summary>
        /// 解绑信息
        /// </summary>
        public IEnumerable<Tuple<Authority, AuthorityOperation>> UnBinds
        {
            get;set;
        }
    }
}
