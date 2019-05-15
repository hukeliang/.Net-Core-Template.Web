using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

using ZKWebNet = ZXing.ZKWeb.Net;

namespace Common.Helper.Core.IOExpand.Implementation
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QrCodeService : IQrCode
    {
        /// <summary>
        /// 创建不带图片的二维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="pixel">二维码大小</param>
        /// <returns></returns>
        Bitmap IQrCode.CreateQrCodeToBitmap(string message, int pixel)
        {
            return this.CreateQrCode(message, pixel);
        }

        /// <summary>
        /// 创建带图片的二维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="iconUrl">图片链接地址</param>
        /// <param name="pixel">二维码大小</param>
        /// <returns></returns>
        Bitmap IQrCode.CreateQrCodeToBitmap(string message, string iconUrl, int pixel)
        {
            return this.CreateQrCode(message, iconUrl, pixel);
        }

        /// <summary>
        /// 创建不带图片的二维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="pixel">二维码大小</param>
        byte[] IQrCode.CreateQrCodeToByte(string message, int pixel)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap bitmap = this.CreateQrCode(message, pixel);

                bitmap.Save(stream, ImageFormat.Jpeg);

                bitmap.Dispose();

                return stream.ToArray();
            }
        }

        /// <summary>
        /// 创建带图片的二维码 =>  Byte[]
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="iconUrl">图片链接地址</param>
        /// <param name="pixel">二维码大小</param>
        byte[] IQrCode.CreateQrCodeToByte(string message, string iconUrl, int pixel)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap bitmap = this.CreateQrCode(message, iconUrl, pixel);

                bitmap.Save(stream, ImageFormat.Jpeg);

                bitmap.Dispose();

                return stream.ToArray();
            }
        }

        /// <summary>
        /// 创建带图片的二维码
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="middleImg">图片</param>
        /// <param name="pixel">大小</param>
        /// <returns></returns>
        private Bitmap CreateQrCode(string message, string imagePath, int pixel)
        {
            using (Image middleImg = new Bitmap(imagePath))
            {
                //构造二维码写码器
                MultiFormatWriter mutiWriter = new MultiFormatWriter();
                Dictionary<EncodeHintType, object> encodeHint = new Dictionary<EncodeHintType, object>();
                encodeHint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                encodeHint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

                //生成二维码
                BitMatrix bitMatrix = mutiWriter.encode(message, BarcodeFormat.QR_CODE, pixel, pixel, encodeHint);
                ZKWebNet.BarcodeWriter barcodeWriter = new ZKWebNet.BarcodeWriter();

                Bitmap bitmap = barcodeWriter.Write(bitMatrix);

                //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
                int[] rectangle = bitMatrix.getEnclosingRectangle();

                //计算插入图片的大小和位置
                int middleImgW = Math.Min((int)(rectangle[2] / 3.5), middleImg.Width);
                int middleImgH = Math.Min((int)(rectangle[3] / 3.5), middleImg.Height);
                int middleImgL = (bitmap.Width - middleImgW) / 2;
                int middleImgT = (bitmap.Height - middleImgH) / 2;

                //将img转换成bmp格式，否则后面无法创建 Graphics对象
                Bitmap bmpimg = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

                using (Graphics graphics = Graphics.FromImage(bmpimg))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.DrawImage(bitmap, 0, 0);
                }

                //在二维码中插入图片
                using (Graphics myGraphic = Graphics.FromImage(bmpimg))
                {
                    //白底
                    myGraphic.FillRectangle(Brushes.White, middleImgL, middleImgT, middleImgW, middleImgH);

                    myGraphic.DrawImage(middleImg, middleImgL, middleImgT, middleImgW, middleImgH);
                }

                bitmap.Dispose();

                return bmpimg;
            }
        }

        /// <summary>
        /// 创建不带图片的二维码
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="pixel">大小</param>
        /// <returns></returns>
        private Bitmap CreateQrCode(string message, int pixel)
        {
            ZKWebNet.BarcodeWriter barcodeWriter = new ZKWebNet.BarcodeWriter();
            EncodingOptions encodingOptions = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = pixel,
                Height = pixel,
                ErrorCorrection = ErrorCorrectionLevel.H,
            };
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = encodingOptions;

            Bitmap bitmap = barcodeWriter.Write(message);

            return bitmap;
        }
    }
}
