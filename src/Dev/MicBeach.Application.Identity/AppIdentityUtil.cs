using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Application.Identity
{
    public static class AppIdentityUtil
    {
        #region 获取ID生成分组编码

        /// <summary>
        /// 获取ID生成分组编码
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <returns></returns>
        public static string GetIdGroupCode(IdentityGroup group)
        {
            return ((int)group).ToString();
        }

        #endregion
    }
}
