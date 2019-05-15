using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Common.Helper.Core.IOExpand.Implementation
{
    /// <summary>
    /// 限速
    /// </summary>
    public sealed class SpeedLimitService : ISpeedLimit
    {
        //进行拆包,每包大小10K bytes 避免一次将整个下载文件都加载到服务器内存中，导致服务器崩溃       
        private const int bufferSize = 10240;

        private readonly HttpContext _HttpContext;
        private readonly HttpRequest _HttpRequest;
        private readonly HttpResponse _HttpResponse;


        public SpeedLimitService(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContext = httpContextAccessor.HttpContext;
            _HttpRequest = _HttpContext.Request;
            _HttpResponse = _HttpContext.Response;
        }

        /// <summary>
        /// 文件的限速下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        bool ISpeedLimit.Download(string filePath, int speed)
        {
            // 限速时每个包的时间 每秒下载多少MB
            double time = 1000 / ((speed * 1024 * 1024) / bufferSize);

            byte[] buffer = new byte[bufferSize];

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                _HttpResponse.ContentType = HttpContentType.APPLICATION_OCTET_STREAM;

                _HttpResponse.Headers.Add("Content-Disposition", $"attachment;filename={ HttpUtility.UrlEncode(Path.GetFileName(filePath), Encoding.UTF8) }");//

                Stopwatch stopwatch = new Stopwatch();

                //循环读取下载文件的内容，并发送到客户端浏览器
                int contentLength;

                do
                {
                    //检测客户端浏览器和服务器之间的连接状态，如果返回true，说明客户端浏览器中断了连接
                    //这立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                    if (_HttpContext.RequestAborted.IsCancellationRequested)
                    {
                        return false;
                    }

                    stopwatch.Restart();

                    //从下载文件中读取bufferSize(1024字节)大小的内容到服务器内存中
                    contentLength = binaryReader.Read(buffer, 0, bufferSize);

                    //发送读取的内容数据到客户端浏览器
                    _HttpResponse.Body.Write(buffer);

                    //注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间
                    _HttpResponse.Body.Flush();

                    stopwatch.Stop();

                    //如果实际带宽小于限制时间就不需要等待
                    if (stopwatch.ElapsedMilliseconds < time)
                    {
                        Thread.Sleep((int)(time - stopwatch.ElapsedMilliseconds));
                    }

                } while (contentLength != 0);

                return true;
            }

        }

        /// <summary>
        ///  文件的限速上载
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        bool ISpeedLimit.Upload(Stream stream, string filePath, int speed)
        {
            // 限速时每个包的时间 每秒上载多少MB
            double time = 1000 / ((speed * 1024 * 1024) / bufferSize);

            byte[] buffer = new byte[bufferSize];

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                Stopwatch stopwatch = new Stopwatch();

                int bytesRead, totalBytesRead = 0;

                do
                {
                    //检测客户端浏览器和服务器之间的连接状态，如果返回true，说明客户端浏览器中断了连接
                    //这立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                    if (_HttpContext.RequestAborted.IsCancellationRequested)
                    {
                        return false;
                    }

                    stopwatch.Restart();

                    //从文件中读取bufferSize大小的内容到服务器内存中
                    bytesRead = stream.Read(buffer, 0, bufferSize);

                    totalBytesRead += bytesRead;

                    stopwatch.Stop();

                    //如果实际带宽小于限制时间就不需要等待
                    if (stopwatch.ElapsedMilliseconds < time)
                    {
                        Thread.Sleep((int)(time - stopwatch.ElapsedMilliseconds));
                    }

                } while (bytesRead > 0);

                fileStream.Write(buffer, 0, bufferSize);

                return true;
            }

        }

    }

}


