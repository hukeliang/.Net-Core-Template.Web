using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Common.Helper.Core.IOExpand.Implementation
{
    /// <summary>
    /// 验证码
    /// </summary>
    public sealed class VerifyCodeService : IVerifyCode
    {
        #region 创建验证码
        /// <summary>
        /// 创建全数字验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string CreateNumberVerifyCode(int length)
        {

            int[] randMembers = new int[length];

            int[] validateNums = new int[length];

            string validateNumberStr = "";

            //生成起始序列值  

            int seekSeek = unchecked((int)DateTime.Now.Ticks);

            Random seekRand = new Random(seekSeek);

            int beginSeek = seekRand.Next(0, Int32.MaxValue - length * 10000);

            int[] seeks = new int[length];

            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;

                seeks[i] = beginSeek;
            }

            //生成随机数字  

            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);

                int pownum = 1 * (int)Math.Pow(10, length);

                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }

            //抽取随机数字  
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();

                int numLength = numStr.Length;

                Random rand = new Random();

                int numPosition = rand.Next(0, numLength - 1);

                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }

            //生成验证码  
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 2.字母验证码
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns>验证码字符</returns>
        private string CreateLetterVerifyCode(int length)
        {
            char[] verification = new char[length];

            char[] dictionary = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                verification[i] = dictionary[random.Next(dictionary.Length - 1)];
            }
            return new string(verification);
        }

        /// <summary>
        /// 3.混合验证码
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns>验证码字符</returns>
        private string CreateHybridVerifyCode(int length)
        {
            char[] verification = new char[length];

            char[] dictionary = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                verification[i] = dictionary[random.Next(dictionary.Length - 1)];
            }

            return new string(verification);
        }

        /// <summary>
        /// 产生验证
        /// </summary>
        /// <param name="type">验证码类型：数字，字符，符合</param>
        /// <returns></returns>
        private Bitmap CreateVerifyCode(VerifyCodeType verifyCodeType, int width, int height, int length)
        {
            string verifyCode = string.Empty;

            switch (verifyCodeType)
            {
                case VerifyCodeType.Number: verifyCode = this.CreateNumberVerifyCode(length); break;

                case VerifyCodeType.Letter: verifyCode = this.CreateLetterVerifyCode(length); break;

                case VerifyCodeType.Hybrid: verifyCode = this.CreateHybridVerifyCode(length); break;
            }

            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));

                SizeF totalSizeF = graphics.MeasureString(verifyCode, font);

                PointF startPointF = new PointF(0, (height - totalSizeF.Height) / 2);

                Random random = new Random(); //随机数产生器

                graphics.Clear(Color.White); //清空图片背景色  

                for (int i = 0; i < verifyCode.Length; i++)
                {
                    Brush brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));

                    graphics.DrawString(verifyCode[i].ToString(), font, brush, startPointF);

                    SizeF curCharSizeF = graphics.MeasureString(verifyCode[i].ToString(), font);

                    startPointF.X += curCharSizeF.Width;

                    brush.Dispose();
                }

                font.Dispose();

                //画图片的干扰线  
                for (int i = 0; i < 10; i++)
                {
                    int x1 = random.Next(bitmap.Width);

                    int x2 = random.Next(bitmap.Width);

                    int y1 = random.Next(bitmap.Height);

                    int y2 = random.Next(bitmap.Height);

                    graphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

                }

                //画图片的前景干扰点  
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(bitmap.Width);

                    int y = random.Next(bitmap.Height);

                    bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1); //画图片的边框线  

                return bitmap;
            }
        }

        #endregion

        /// <summary>
        /// 验证码图片 => Bitmap
        /// </summary>
        /// <param name="verifyCode">验证码类型</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>Bitmap</returns>
        Bitmap IVerifyCode.CreateImageVerifyCodeToBitmap(VerifyCodeType verifyCodeType, int width, int height, int length)
        {
            return this.CreateVerifyCode(verifyCodeType, width, height, length);
        }

        /// <summary>
        /// 验证码图片 => byte[]
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>byte[]</returns>
        byte[] IVerifyCode.CreateImageVerifyCodeToByte(VerifyCodeType verifyCodeType, int width, int height, int length)
        {
            //保存图片数据  
            using (MemoryStream stream = new MemoryStream())
            using (Bitmap bitmap = this.CreateVerifyCode(verifyCodeType, width, height, length))
            {
                bitmap.Save(stream, ImageFormat.Jpeg);

                //输出图片流  
                return stream.ToArray();
            }
             
        }
      
    }

    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerifyCodeType
    {
        /// <summary>
        /// 全数字
        /// </summary>
        Number,

        /// <summary>
        /// 全字母
        /// </summary>
        Letter,

        /// <summary>
        /// 混合
        /// </summary>
        Hybrid
    };


}
