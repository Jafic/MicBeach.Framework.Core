using MicBeach.DataValidation;
using MicBeach.Develop.DataValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataValidation.Config
{
    /// <summary>
    /// 验证规则信息
    /// </summary>
    public class ValidatorRule
    {
        #region 属性

        /// <summary>
        /// 验证类型
        /// </summary>
        [JsonProperty(PropertyName = "vtype")]
        public ValidatorType ValidateType
        {
            get; set;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        [JsonProperty(PropertyName = "operator")]
        public CompareOperator Operator
        {
            get; set;
        }

        /// <summary>
        /// 验证值
        /// </summary>
        [JsonProperty(PropertyName = "val")]
        public dynamic Value
        {
            get; set;
        }

        /// <summary>
        /// 枚举类型名称
        /// </summary>
        [JsonProperty(PropertyName = "enumType")]
        public string EnumType
        {
            get; set;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        [JsonProperty(PropertyName = "maxValue")]
        public dynamic MaxValue
        {
            get; set;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        [JsonProperty(PropertyName = "minValue")]
        public dynamic MinValue
        {
            get; set;
        }

        /// <summary>
        /// 小值边界
        /// </summary>
        [JsonProperty(PropertyName = "lowerBoundary")]
        public RangeBoundary LowerBoundary
        {
            get; set;
        }

        /// <summary>
        /// 大值边界
        /// </summary>
        [JsonProperty(PropertyName = "upperBoundary")]
        public RangeBoundary UpperBoundary
        {
            get; set;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty(PropertyName = "errorMsg")]
        public string ErrorMessage
        {
            get; set;
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        [JsonProperty(PropertyName = "tipMsg")]
        public bool TipMessage
        {
            get; set;
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        [JsonProperty(PropertyName = "compareType")]
        public CompareObject CompareTarget
        {
            get;set;
        }

        #endregion
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public enum CompareObject
    {
        Field,
        Value
    }
}
