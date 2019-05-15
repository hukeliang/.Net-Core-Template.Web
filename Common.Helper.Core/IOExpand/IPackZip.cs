using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Common.Helper.Core.IOExpand
{
    /// <summary>
    /// 提供文件打包服务
    /// </summary>
    public interface IPackZip
    {
        /// <summary>
        /// 创建一个压缩包 返回一个 byte[]
        /// </summary>
        /// <param name="comment">注释信息</param>
        /// <param name="password">压缩密码</param>
        /// <param name="compressionLevel">压缩等级，范围从0到9，可选，默认为6</param>
        /// <param name="filePaths">压缩文件路径</param>
        byte[] CreateZipToByte(string comment = null, string password = null, int compressionLevel = 6,params string [] filePaths);
        
    }
}
