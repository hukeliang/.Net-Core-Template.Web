using Common.Helper.Core.IOExpand.Implementation;
using System.Drawing;
using System.IO;

namespace Common.Helper.Core.IOExpand
{
    /// <summary>
    /// 提供验证码服务 如果服务器为Linux就需要安装GDI库
    /// </summary>
    public interface IVerifyCode
    {
        /// <summary>
        /// 验证码图片 => Bitmap
        /// </summary>
        /// <param name="verifyCode">验证码类型</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>Bitmap</returns>
        Bitmap CreateImageVerifyCodeToBitmap(VerifyCodeType verifyCodeType, int width, int height, int length = 4);

        /// <summary>
        /// 验证码图片 => byte[]
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>byte[]</returns>
        byte[] CreateImageVerifyCodeToByte(VerifyCodeType verifyCodeType, int width, int height, int length = 4);


    }
}
