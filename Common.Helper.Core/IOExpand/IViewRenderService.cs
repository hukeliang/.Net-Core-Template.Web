using System.Threading.Tasks;



namespace Common.Helper.Core.IOExpand
{
    /// <summary>
    /// 使用Razor视图引擎来生成邮件内容
    /// </summary>
    public interface IViewRenderService
    {
        /// <summary>
        ///  获取视图文件转换为字符串
        /// </summary>
        /// <param name="viewPath">视图文件路径</param>
        /// <returns></returns>
        string Render(string viewPath);

        /// <summary>
        ///  获取视图文件转换为字符串
        /// </summary>
        /// <typeparam name="TModel">model类型</typeparam>
        /// <param name="viewPath">视图文件路径</param>
        /// <param name="model">传递到视图中的model数据</param>
        /// <returns></returns>
        string Render<TModel>(string viewPath, TModel model);

        /// <summary>
        /// 异步获取视图文件转换为字符串
        /// </summary>
        /// <param name="viewPath">视图文件路径</param>
        /// <returns></returns>
        Task<string> RenderAsync(string viewPath);

        /// <summary>
        /// 异步获取视图文件转换为字符串
        /// </summary>
        /// <typeparam name="TModel">model类型</typeparam>
        /// <param name="viewPath">视图文件路径</param>
        /// <param name="model">传递到视图中的model数据</param>
        /// <returns></returns>
        Task<string> RenderAsync<TModel>(string viewPath, TModel model);
    }
}
