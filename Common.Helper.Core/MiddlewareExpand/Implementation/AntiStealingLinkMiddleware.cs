using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;

namespace Common.Helper.Core.MiddlewareExpand.Implementation
{
    public class AntiStealingLinkMiddleware
    {
        private readonly string _wwwrootFolder;
        private readonly RequestDelegate _next;


        public AntiStealingLinkMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _wwwrootFolder = env.WebRootPath;
            _next = next;
        }

        public async Task Invoke(HttpContext context, IViewRenderService viewRenderService)
        {
            HttpRequest request = context.Request;

            IHeaderDictionary headersDictionary = request.Headers;

            string applicationUrl = $"{request.Scheme}://{request.Host.Value}";
            string urlReferrer = headersDictionary[HeaderNames.Referer].ToString();

            if (!string.IsNullOrEmpty(urlReferrer) && (!urlReferrer.StartsWith(applicationUrl) || WhiteList(urlReferrer)))
            {
                HttpResponse response = context.Response;
                string suffix = Path.GetExtension(request.Path).ToLower();

                if (suffix.Equals(".css") || suffix.Equals(".js"))
                {

                    var htmlString = await viewRenderService.RenderAsync("/Common/ViewTemplate/403.cshtml");

                    response.StatusCode = 403;

                    await response.WriteAsync(htmlString);
                }
                if (suffix.Equals(".jpg") || suffix.Equals(".png") || suffix.Equals(".gif") || suffix.Equals(".jpeg"))
                {
                    await response.SendFileAsync(Path.Combine(_wwwrootFolder, "images/img-16434886-f889-46fd-ab1e-3bbe6de7fedf.png"));
                }
            }

            await _next(context);

        }
        private bool WhiteList(string httpReferer) => httpReferer.Contains("baidu.com") || httpReferer.Contains("google.com");
    }
}
