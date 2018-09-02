using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Develop.CQuery.CriteriaConvert
{
    /// <summary>
    /// 字符串长度转换
    /// </summary>
    public class StringLengthCriteriaConvert : ICriteriaConvert
    {
        /// <summary>
        /// convert type
        /// </summary>
        public CriteriaConvertType Type { get; set; } = CriteriaConvertType.StringLength;
    }
}
