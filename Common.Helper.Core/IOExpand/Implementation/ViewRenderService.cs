using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Helper.Core.IOExpand.Implementation
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 异步获取视图文件转换为字符串
        /// </summary>
        /// <param name="viewPath">视图文件路径</param>
        /// <returns></returns>
        public async Task<string> RenderAsync(string viewPath)
        {
            return await RenderAsync(viewPath, string.Empty);
        }
        /// <summary>
        /// 异步获取视图文件转换为字符串
        /// </summary>
        /// <typeparam name="TModel">model类型</typeparam>
        /// <param name="viewPath">视图文件路径</param>
        /// <param name="model">传递到视图中的model数据</param>
        /// <returns></returns>
        public async Task<string> RenderAsync<TModel>(string viewPath, TModel model)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            var viewResult = _viewEngine.GetView(null, viewPath, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"找不到视图模板 {viewPath}");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                     actionContext,
                     viewResult.View,
                     viewDictionary,
                     new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                     writer,
                     new HtmlHelperOptions()
                 );

                await viewResult.View.RenderAsync(viewContext).ConfigureAwait(false);

                return writer.ToString();
            }
        }
        /// <summary>
        ///  获取视图文件转换为字符串
        /// </summary>
        /// <typeparam name="TModel">model类型</typeparam>
        /// <param name="viewPath">视图文件路径</param>
        /// <param name="model">传递到视图中的model数据</param>
        /// <returns></returns>
        public string Render<TModel>(string viewPath, TModel model)
        {
            var task = this.RenderAsync(viewPath, model);
            task.Wait();
            return task.Result;
        }

        public string Render(string viewPath)
        {
            var task = this.RenderAsync(viewPath, string.Empty);
            task.Wait();
            return task.Result;
        }
    }
}
