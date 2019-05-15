

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using SharpZipLib = ICSharpCode.SharpZipLib.Zip;
using SharpZipLibCore = ICSharpCode.SharpZipLib.Core;

namespace Common.Helper.Core.IOExpand.Implementation
{
    /// <summary>
    /// 打包压缩
    /// </summary>
    public class PackZipService : IPackZip
    {

        //进行拆包,每包大小10K bytes 避免一次将整个下载文件都加载到服务器内存中，导致服务器崩溃       
        private const int bufferSize = 10240;

        /// <summary>
        /// 创建一个压缩包 返回一个 byte[]
        /// </summary>
        /// <param name="comment">注释信息</param>
        /// <param name="password">压缩密码</param>
        /// <param name="compressionLevel">压缩等级，范围从0到9，可选，默认为6</param>
        /// <param name="filePaths">压缩文件路径</param>
        byte[] IPackZip.CreateZipToByte(string comment, string password, int compressionLevel, params string[] filePaths)
        {
            using (MemoryStream memoryStream = this.CreateZip(comment, password, compressionLevel, filePaths))
            {
                return memoryStream.ToArray();
            }

        }



        /// <summary>
        /// 压缩多个文件/文件夹
        /// </summary>
        /// <param name="comment">注释信息</param>
        /// <param name="password">压缩密码</param>
        /// <param name="compressionLevel">压缩等级，范围从0到9，可选，默认为6</param>
        /// <param name="filePaths">压缩文件路径</param>
        /// <returns></returns>
        private MemoryStream CreateZip(string comment, string password, int compressionLevel, params string[] filePaths)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (SharpZipLib.ZipOutputStream zipStream = new SharpZipLib.ZipOutputStream(memoryStream))
            {
                if (!string.IsNullOrWhiteSpace(password))
                {
                    zipStream.Password = password;//设置密码
                }

                if (!string.IsNullOrWhiteSpace(comment))
                {
                    zipStream.SetComment(comment);//添加注释
                }

                //设置压缩级别
                zipStream.SetLevel(compressionLevel);

                foreach (string item in filePaths)//从字典取文件添加到压缩文件
                {
                    //如果不是文件直接跳过不打包
                    if (!File.Exists(item))
                    {
                        continue;
                    }

                    FileInfo fileInfo = new FileInfo(item);

                    using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        SharpZipLib.ZipEntry zipEntry = new SharpZipLib.ZipEntry(Path.GetFileName(item));

                        zipEntry.DateTime = fileInfo.LastWriteTime;

                        zipEntry.Size = fileStream.Length;

                        zipStream.PutNextEntry(zipEntry);

                        int readLength = 0;

                        byte[] buffer = new byte[bufferSize];

                        do
                        {
                            readLength = fileStream.Read(buffer, 0, bufferSize);
                            zipStream.Write(buffer, 0, readLength);
                        }
                        while (readLength == bufferSize);
                    }
                }
            }

            return memoryStream;
        }


    }
}
