using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper.Core.IOExpand
{
    public interface IExcel
    {
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="models">实体数据</param>
        /// <param name="startRow">填充数据起始行 默认第二行开始</param>
        /// <param name="password">是否设置密码</param>
        byte[] ExportToByte<TModel>(string filePath, List<TModel> models, int startRow = 2, string password = null);
    }
}
