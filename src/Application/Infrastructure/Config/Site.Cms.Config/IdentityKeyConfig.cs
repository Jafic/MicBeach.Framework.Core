using MicBeach.Application.Identity;
using MicBeach.Util.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site.Cms.Config
{
    /// <summary>
    /// 唯一标识符配置
    /// </summary>
    public static class IdentityKeyConfig
    {
        public static void Init()
        {
            List<string> groupCodes = new List<string>();

            #region Identity

            Array values = Enum.GetValues(IdentityGroup.授权操作.GetType());
            foreach (IdentityGroup group in values)
            {
                groupCodes.Add(AppIdentityUtil.GetIdGroupCode(group));
            }

            #endregion

            SerialNumber.RegisterGenerator(groupCodes, 1, 1);
        }
    }
}
