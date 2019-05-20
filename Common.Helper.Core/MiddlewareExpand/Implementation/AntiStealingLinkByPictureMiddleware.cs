using Common.Helper.Core.IOExpand;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Helper.Core.MiddlewareExpand.Implementation
{
    public class AntiStealingLinkByPictureMiddleware
    {

        private readonly RequestDelegate _next;
        

        private readonly string[] _suffixArrayByPicture = { ".jpg", ".png", ".gif", ".jpeg" };


        private readonly string[] _whiteList;

        private readonly string _path;
        public AntiStealingLinkByPictureMiddleware(RequestDelegate next, string path, string whiteList)
        {
            _next = next;
            _path = path;

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

                if (_suffixArrayByPicture.Contains(suffix))
                {
                    await response.SendFileAsync(_path);
                }
            }

            await _next(context);

        }
    }
}
