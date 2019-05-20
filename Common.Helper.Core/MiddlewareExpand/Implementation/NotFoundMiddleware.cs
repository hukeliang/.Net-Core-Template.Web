using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Common.Helper.Core.MiddlewareExpand.Implementation
{

    /// <summary>
    /// 404错误中间件
    /// </summary>
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IViewRenderService _viewRenderService;

        private readonly string _path;
       

        public NotFoundMiddleware(RequestDelegate next,IViewRenderService viewRenderService, string path) //在中间件中只有单例可以构造函数注入
        {
            _next = next;
            _viewRenderService = viewRenderService;
            _path = path;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpResponse httpResponse = context.Response;

            if (StatusCodes.Status404NotFound.Equals(httpResponse.StatusCode) && !httpResponse.HasStarted)
            {
                string html = await _viewRenderService.RenderAsync(_path);

                await httpResponse.WriteAsync(html);
            }

            await _next.Invoke(context);
        }
    }
}
