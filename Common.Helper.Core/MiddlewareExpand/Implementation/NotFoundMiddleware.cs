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

        private readonly string _errorPath;


        public NotFoundMiddleware(RequestDelegate next, string errorPath)
        {
            _next = next;
            _errorPath = errorPath;
        }

        public async Task Invoke(HttpContext context, IViewRenderService viewRenderService)//在中间件中只有单例可以构造函数注入所以这里采用方法注入
        {
            HttpResponse httpResponse = context.Response;

            if (StatusCodes.Status404NotFound.Equals(httpResponse.StatusCode) && !httpResponse.HasStarted)
            {
                string htmlString = await viewRenderService.RenderAsync(_errorPath);

                await httpResponse.WriteAsync(htmlString);
            }

            await _next.Invoke(context);
        }
    }
}
