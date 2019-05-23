using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Common.Helper.Core.IOExpand.Implementation
{
    public class PictureService : IPictureService
    {
        /// <summary>
        /// 生成略缩图
        /// </summary>
        /// <param name="sourcePath">图片地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        void IPictureService.MakeThumbnail(string sourcePath, int? width, int? height)
        {
            using (Image originalImage = Image.FromFile(sourcePath))
            {
                int towidth = 0;
                int toheight = 0;

                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                if (width != null && height != null) //指定高宽缩放（可能变形）         
                {
                    towidth = width.Value;
                    toheight = height.Value;

                    double result1 = originalImage.Width / originalImage.Height;
                    double result2 = towidth / towidth;

                    if (result1 > towidth / result2)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height.Value / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                }

                if (width != null && height == null) //指定宽，高按比例       
                {
                    toheight = originalImage.Height * width.Value / originalImage.Width;
                }

                if (width == null && height != null)//指定高，宽按比例
                {
                    towidth = originalImage.Width * height.Value / originalImage.Height;
                }



                using (Image bitmap = new Bitmap(towidth, toheight))//新建一个bmp图片
                using (Graphics graphics = Graphics.FromImage(bitmap))//新建一个画板
                {
                    //设置高质量插值法
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    //设置高质量,低速度呈现平滑程度
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //清空画布并以透明背景色填充
                    graphics.Clear(System.Drawing.Color.Transparent);

                    //在指定位置并且按指定大小绘制原图片的指定部分
                    graphics.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                    //以jpg格式保存缩略图

                    bitmap.Save($"{sourcePath.Substring(0, sourcePath.LastIndexOf('.')) }_min.jpg", ImageFormat.Jpeg);
                }
            }
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        byte[] IPictureService.Watermark(string path, string watermarkPath)
        {
            using (Image image = Image.FromFile(path))
            using (Image watermarkImage = Image.FromFile(watermarkPath))
            using (Graphics graphics = Graphics.FromImage(watermarkImage))
            {
                graphics.DrawImage(watermarkImage, new Rectangle(image.Width - watermarkImage.Width,
                    image.Height - watermarkImage.Height,
                    watermarkImage.Width, watermarkImage.Height),
                    0, 0, watermarkImage.Width, watermarkImage.Height, GraphicsUnit.Pixel);

                using (MemoryStream memoryStream = new MemoryStream())
                using (Bitmap bitmap = new Bitmap(image))
                {
                    bitmap.Save(memoryStream, ImageFormat.Jpeg);
                    return memoryStream.ToArray();
                }
            }
        }

    }
}
