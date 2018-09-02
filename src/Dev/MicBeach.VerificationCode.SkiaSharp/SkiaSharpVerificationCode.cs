using MicBeach.Util.Drawing;
using SkiaSharp;
using System;
using System.Drawing;
using System.IO;

namespace MicBeach.VerificationCode.SkiaSharp
{
    public class SkiaSharpVerificationCode : VerificationCodeBase
    {
        #region 方法

        #region 设置验证码字符个数

        #endregion

        #region 生成验证码

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns>返回生成图片的二进制数据</returns>
        public override byte[] CreateCode()
        {
            byte[] codeImageBinary = new byte[0];

            #region 获取验证码的值

            string codeString = CreateCodeString();//创建一个新的验证码值
            if (string.IsNullOrWhiteSpace(codeString))
            {
                return codeImageBinary;
            }
            this.code = codeString;

            #endregion

            #region 生成图片

            int width = (this.fontSize + spaceBetween) * this.length + 5;
            int height = this.fontSize + 11;
            using (SKBitmap bitmap = new SKBitmap(width, height))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear();
                    canvas.DrawColor(new SKColor(backgroundColor.R, backgroundColor.G, backgroundColor.B, backgroundColor.A));
                    using (var paint = new SKPaint())
                    {
                        for (int n = 0; n < this.length; n++)
                        {
                            paint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                            paint.TextSize = fontSize;
                            canvas.DrawText(codeString[n].ToString(), n * fontSize + spaceBetween, bitmap.Height - (bitmap.Height - fontSize) / 2, paint);
                        }
                        //干扰线
                        for (int i = 0; i < 5; i++)
                        {
                            paint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                            canvas.DrawLine(random.Next(0, 40), random.Next(1, 29), random.Next(41, 80), random.Next(1, 29), paint);
                        }
                    }
                    using (var image = SKImage.FromBitmap(bitmap))
                    {
                        using (var skdata = image.Encode(SKEncodedImageFormat.Png, 100))
                        {
                            codeImageBinary = skdata.ToArray();
                        }
                    }
                }
            }

            #endregion

            return codeImageBinary;
        }

        #endregion

        #region 创建一个新的验证码字符串

        /// <summary>
        /// 创建一个新的验证码字符串
        /// </summary>
        /// <returns>生成的验证码字符串值</returns>
        private string CreateCodeString()
        {
            string newCode = string.Empty;
            if (charArray == null || charArray.Length == 0)
            {
                return newCode;
            }
            int beginIndexNumber = GetBeginIndexNumber();//开始索引编号
            int endIndexNumber = GetEndIndexNumber();//结束索引编号
            Random random = new Random();
            for (int i = 0; i < this.length; i++)
            {
                int nowIndex = random.Next(beginIndexNumber, endIndexNumber + 1);
                newCode += charArray[nowIndex].ToString();
            }
            return newCode;
        }

        #endregion

        #region 返回开始索引编号

        /// <summary>
        /// 返回开始索引编号
        /// </summary>
        /// <returns>开始索引编号</returns>
        private int GetBeginIndexNumber()
        {
            int newNumber = 0;
            switch (this.codeType)
            {
                case VCodeType.纯数字:
                case VCodeType.纯中文:
                case VCodeType.数字和字母:
                    newNumber = 0;
                    break;
                case VCodeType.纯字母:
                    newNumber = 10;
                    break;
                default:
                    newNumber = 10;
                    break;

            }
            return newNumber;
        }

        #endregion

        #region 返回结束索引编号

        /// <summary>
        /// 返回结束索引编号
        /// </summary>
        /// <returns>结束所有编号</returns>
        private int GetEndIndexNumber()
        {
            int newNumber = 0;
            int maxNumber = charArray.Length - 1;
            switch (this.codeType)
            {
                case VCodeType.纯数字:
                case VCodeType.纯中文:
                    newNumber = 9;
                    break;
                case VCodeType.纯字母:
                case VCodeType.数字和字母:
                    newNumber = maxNumber;
                    break;
                default:
                    newNumber = maxNumber;
                    break;
            }
            return newNumber;
        }

        #endregion

        #endregion
    }
}
