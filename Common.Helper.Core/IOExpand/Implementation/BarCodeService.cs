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
    /// 一维码
    /// </summary>
    public sealed class BarCodeService : IBarCode
    {
        /// <summary>
        /// 创建一维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        Bitmap IBarCode.CreateBarCodeToBitmap(string message, int width, int height)
        {
            return this.CreateBarCode(message, width, height);
        }


        /// <summary>
        /// 创建一维码 => Byte[]
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        byte[] IBarCode.CreateBarCodeToByte(string message, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream())
            using (Bitmap bitmap = this.CreateBarCode(message, width, height))
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 读取条形码从图片
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        string IBarCode.ReadBarCodeFromImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentNullException(nameof(imagePath));
            }

            using (Image image = Image.FromFile(imagePath))
            using (Bitmap bitmap = new Bitmap(image))
            {
                //该类名称为BarcodeReader,可以读二维码和条形码       
                ZKWebNet.BarcodeReader barcodeReader = new ZKWebNet.BarcodeReader();

                barcodeReader.Options = new DecodingOptions
                {
                    CharacterSet = "UTF-8"
                };

                Result result = barcodeReader.Decode(bitmap);
                return result.Text;
            }
        }


        /// <summary>
        /// 创建一维码
        /// </summary>
        /// <param name="message"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Bitmap CreateBarCode(string message, int width, int height)
        {
            CodaBarWriter codaBarWriter = new CodaBarWriter();
            BitMatrix bitMatrix = codaBarWriter.encode(message, BarcodeFormat.ITF, width, height);
            ZKWebNet.BarcodeWriter barcodeWriter = new ZKWebNet.BarcodeWriter();
            barcodeWriter.Options = new EncodingOptions()
            {
                Margin = 3,
                PureBarcode = true
            };
            Bitmap bitmap = barcodeWriter.Write(bitMatrix);

            return bitmap;
        }
    }
}
