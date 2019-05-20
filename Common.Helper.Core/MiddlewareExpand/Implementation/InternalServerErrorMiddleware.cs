using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Common.Helper.Core.MiddlewareExpand.Implementation
{
    public class InternalServerErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly string _path;

        public InternalServerErrorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IViewRenderService viewRenderService, string path)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<InternalServerErrorMiddleware>();
            _viewRenderService = viewRenderService;
            _path = path;
        }

        public async Task Invoke(HttpContext context, IViewRenderService viewRenderService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HttpResponse response = context.Response;

                response.Clear();

                response.StatusCode = StatusCodes.Status500InternalServerError;   //发生未捕获的异常，手动设置状态码

                if (response.HasStarted)
                {
                    _logger.LogWarning("响应已经启动，错误页面中间件将不会被执行");
                    throw;
                }

                string htmlString = await viewRenderService.RenderAsync(_path);

                await response.WriteAsync(htmlString);


                _logger.LogError(ex, "执行请求时发生未处理的异常");
            }
        }
    }
}
