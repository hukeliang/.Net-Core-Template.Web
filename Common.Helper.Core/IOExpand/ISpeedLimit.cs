using System.IO;

namespace Common.Helper.Core.IOExpand
{
    /// <summary>
    /// 提供文件的速度限制服务
    /// </summary>
    public interface ISpeedLimit
    {
        /// <summary>
        /// 文件限速下载
        /// </summary>
        /// <param name="filePath">带文件名下载路径</param>
        /// <param name="speed">每秒允许下载的多少MB,默认1MB</param>
        /// <returns></returns>
        bool Download(string filePath, int speed = 1);

        /// <summary>
        /// 文件限速上载保存
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        bool Upload(Stream fileStream, string fileName, int speed = 1);

    }
}
