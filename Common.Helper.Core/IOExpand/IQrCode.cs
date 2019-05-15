using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Common.Helper.Core.IOExpand
{
    interface IQrCode
    {
        /// <summary>
        /// 创建不带图片的二维码 => Bitmap
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="pixel">二维码大小</param>
        Bitmap CreateQrCodeToBitmap(string message, int pixel = 300);

        /// <summary>
        /// 创建不带图片的二维码 => Byte[]
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="pixel">二维码大小</param>
        byte[] CreateQrCodeToByte(string message, int pixel = 300);

        /// <summary>
        /// 创建带图片的二维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="iconUrl">图片链接地址</param>
        /// <param name="pixel">二维码大小</param>
        Bitmap CreateQrCodeToBitmap(string message, string iconUrl, int pixel = 300);

        /// <summary>
        /// 创建带图片的二维码 => Byte[]
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="iconUrl">图片链接地址</param>
        /// <param name="pixel">二维码大小</param>
        byte[] CreateQrCodeToByte(string message, string iconUrl, int pixel = 300);
    }
}
