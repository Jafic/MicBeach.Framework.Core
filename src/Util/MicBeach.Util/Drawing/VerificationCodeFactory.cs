using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Util.Drawing
{
    public static class VerificationCodeFactory
    {
        /// <summary>
        /// generate code
        /// </summary>
        /// <returns></returns>
        public static VerificationCodeBase GetVerificationCode()
        {
            return ContainerManager.Resolve<VerificationCodeBase>();
        }
    }

    /// <summary>
    /// 生成的验证码类型
    /// </summary>
    public enum VCodeType
    {
        纯数字 = 8101,
        纯字母 = 8102,
        纯中文 = 8103,
        数字和字母 = 8104
    }
}
