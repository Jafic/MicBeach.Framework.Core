using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Util.Request
{
    public abstract class BaseArgument
    {
        /// <summary>
        /// 验证参数是否合法可用
        /// </summary>
        /// <returns></returns>
        public virtual bool Verify()
        {
            return true;
        }
    }
}
