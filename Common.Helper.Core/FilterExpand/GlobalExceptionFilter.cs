using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Common.Helper.Core.FilterExpand
{
    /// <summary>
    ///  在MVC层面上捕获和处理异常 MVC层面下的异常无法捕获和处理
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {

        private readonly ILoggerFactory _loggerFactory;//采用内置日志记录
        private readonly IViewRenderService _viewRenderService;//采用内置日志记录

        public GlobalExceptionFilter(ILoggerFactory loggerFactory, IViewRenderService viewRenderService)
        {
            _loggerFactory = loggerFactory;
            _viewRenderService = viewRenderService;
            
        }

        public void OnException(ExceptionContext context)
        {
            ActionDescriptor controller = context.ActionDescriptor;
            ILogger logger  = _loggerFactory.CreateLogger(controller.ToString());

            context.ExceptionHandled = true; //若果context.ExceptionHandled为true，系统对异常的处理就结束了。

        }
        
    }

}
