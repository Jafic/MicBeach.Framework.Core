using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MicBeach.Util.Drawing
{
    public abstract class VerificationCodeBase
    {
        #region 字段

        protected string code = string.Empty;//验证码值

        protected int length = 5;//字符个数

        protected int minLength = 1;//最低字符个数

        protected int maxLenght = 10;//最大字符个数

        protected VCodeType codeType = VCodeType.数字和字母;//验证码类型

        protected static readonly char[] charArray = { '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        protected int fontSize = 12;//字体大小

        protected int spaceBetween = 1;//字符间距

        protected Color backgroundColor = Color.White;//图片背景色

        protected Color fontColor = Color.Black;//字体颜色

        protected static readonly string[] fontFamilys = { "Arial", "Georgia" };//字体样式

        protected static readonly Color[] colors = new Color[] { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };//字体颜色

        protected static readonly Random random = new Random();//随机对象

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置验证码的字符个数
        /// </summary>
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                SetCodeLength(value);
            }
        }

        /// <summary>
        /// 获取或设置验证码类型
        /// </summary>
        public VCodeType CodeType
        {
            get
            {
                return this.codeType;
            }
            set
            {
                this.codeType = value;
            }
        }

        /// <summary>
        /// 获取验证码的值
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }
        }

        /// <summary>
        /// 获取或设置验证码字体大小
        /// </summary>
        public int FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                this.fontSize = value;
            }
        }

        /// <summary>
        /// 获取或设置字符间距
        /// </summary>
        public int SpaceBetween
        {
            get
            {
                return this.spaceBetween;
            }
            set
            {
                this.spaceBetween = value;
            }
        }

        /// <summary>
        /// 获取或设置图片的背景色
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        /// <summary>
        /// 获取或设置字体颜色
        /// </summary>
        public Color FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                this.fontColor = value;
            }

        }

        #endregion

        #region 方法

        #region 设置验证码字符个数

        /// <summary>
        /// 设置验证码字符个数
        /// </summary>
        /// <param name="length">新的验证码长度</param>
        private void SetCodeLength(int length)
        {
            if (length <= 0)
            {
                return;
            }
            length = length < minLength ? minLength : length;
            length = length > maxLenght ? maxLenght : length;
            this.length = length;
        }

        #endregion

        #region 生成验证码

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns>返回生成图片的二进制数据</returns>
        public abstract byte[] CreateCode();

        #endregion

        #endregion
    }
}
