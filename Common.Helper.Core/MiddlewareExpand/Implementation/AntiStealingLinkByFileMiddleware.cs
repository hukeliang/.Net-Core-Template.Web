using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper.Core.MiddlewareExpand.Implementation
{
    public class AntiStealingLinkByFileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IViewRenderService _viewRenderService;

        private readonly string[] _suffixArray = { ".js", ".css" };

        private readonly string[] _whiteList;

        private readonly string _path;

        public AntiStealingLinkByFileMiddleware(RequestDelegate next, IViewRenderService viewRenderService, string path, string whiteList)
        {
            _next = next;
            _path = path;
            _viewRenderService = viewRenderService;
            //白名单
            _whiteList = whiteList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;

            HttpResponse response = context.Response;

            IHeaderDictionary headersDictionary = request.Headers;

            string applicationUrl = $"{request.Scheme}://{request.Host.Value}";

            string urlReferrer = request.Headers[HeaderNames.Referer].ToString();

            if (!string.IsNullOrWhiteSpace(urlReferrer) && (_whiteList.Contains(urlReferrer) || !urlReferrer.StartsWith(applicationUrl)))
            {
                string suffix = Path.GetExtension(request.Path).ToLower();

                if (_suffixArray.Contains(suffix))
                {
                    response.StatusCode = StatusCodes.Status403Forbidden;

                    string html = await _viewRenderService.RenderAsync(_path);

                    await response.WriteAsync(html);
                }
            }

            await _next(context);

        }
    }
}
