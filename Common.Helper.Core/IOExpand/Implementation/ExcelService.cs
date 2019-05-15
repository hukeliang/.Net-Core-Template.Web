using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;

namespace Common.Helper.Core.IOExpand.Implementation
{
    /// <summary>
    /// 导入Excel
    /// </summary>
    public sealed class ExcelService : IExcel
    {
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="models">实体数据</param>
        /// <param name="startRow">填充数据起始行</param>
        /// <param name="password">是否设置密码</param>
        /// <returns></returns>
        byte[] IExcel.ExportToByte<TModel>(string filePath, List<TModel> models, int startRow, string password)
        {

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(Path.GetFileName(filePath));
            }

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;

                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets[1];

                //设置Excel密码
                if (!string.IsNullOrWhiteSpace(password))
                {
                    excelWorksheet.Protection.IsProtected = true;//设置是否进行锁定
                    excelWorksheet.Protection.SetPassword(password);//设置密码
                    excelWorksheet.Protection.AllowAutoFilter = false;//下面是一些锁定时权限的设置
                    excelWorksheet.Protection.AllowDeleteColumns = false;
                    excelWorksheet.Protection.AllowDeleteRows = false;
                    excelWorksheet.Protection.AllowEditScenarios = false;
                    excelWorksheet.Protection.AllowEditObject = false;
                    excelWorksheet.Protection.AllowFormatCells = false;
                    excelWorksheet.Protection.AllowFormatColumns = false;
                    excelWorksheet.Protection.AllowFormatRows = false;
                    excelWorksheet.Protection.AllowInsertColumns = false;
                    excelWorksheet.Protection.AllowInsertHyperlinks = false;
                    excelWorksheet.Protection.AllowInsertRows = false;
                    excelWorksheet.Protection.AllowPivotTables = false;
                    excelWorksheet.Protection.AllowSelectLockedCells = false;
                    excelWorksheet.Protection.AllowSelectUnlockedCells = false;
                    excelWorksheet.Protection.AllowSort = false;
                }

                //获取要反射的属性
                Type type = typeof(TModel);

                PropertyInfo[] propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (TModel item in models)
                {
                    int column = 1;

                    foreach (PropertyInfo props in propertyInfo)
                    {
                        //对不映射的实体数据跳过处理
                        NotMappedAttribute notMapped = props.GetCustomAttribute(typeof(NotMappedAttribute), true) as NotMappedAttribute;

                        if (notMapped != null)
                        {
                            continue;
                        }

                        excelWorksheet.Cells[startRow, column].Value = props.GetValue(item, null);
                        //列加加
                        column++;
                    }
                    //行加加
                    startRow++;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);

                    return memoryStream.ToArray();
                }
            }
        }



    }
}

