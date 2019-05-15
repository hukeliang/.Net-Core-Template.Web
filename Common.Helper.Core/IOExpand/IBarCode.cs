using System.Drawing;

namespace Common.Helper.Core.IOExpand
{
    public interface IBarCode
    {
        /// <summary>
        /// 创建一维码 => Bitmap
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        Bitmap CreateBarCodeToBitmap(string message, int width, int height);

        /// <summary>
        /// 创建一维码 => Byte[]
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        byte[] CreateBarCodeToByte(string message, int width, int height);

        /// <summary>
        /// 读取二维码或者条形码从图片
        /// </summary>
        /// <param name="imgPath"></param>
        string ReadBarCodeFromImage(string imagePath);
    }
}
