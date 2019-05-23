using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper.Core.IOExpand
{
    public interface IPictureService
    {
        /// <summary>
        /// 生成略缩图
        /// </summary>
        /// <param name="sourcePath">图片地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        void MakeThumbnail(string sourcePath, int? width, int? height);

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        byte[] Watermark(string path, string watermarkPath);
    }
}
