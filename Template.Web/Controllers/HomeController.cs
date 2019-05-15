using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Helper.Core.EmailExpand;
using Common.Helper.Core.IOExpand;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Template.Entity;

using Common.Helper.Core.IOExpand.Implementation;
using Template.Model.TemplateModels;

namespace Template.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IHostingEnvironment _IHostingEnvironment;
        private readonly HttpRequest _HttpRequest;
        private readonly HttpResponse _HttpResponse;
        private readonly ISpeedLimit _ISpeedLimit;
        private readonly IVerifyCode _IVerifyCode;
        private readonly ISendEmail _ISendEmail;
        private readonly IExcel _IExcel;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ISpeedLimit speedLimit,
            IVerifyCode verifyCode,
            ISendEmail sendEmail,
            IExcel excel
            )
        {
            _HttpContextAccessor = httpContextAccessor;
            _IHostingEnvironment = hostingEnvironment;
            _HttpRequest = httpContextAccessor.HttpContext.Request;
            _HttpResponse = httpContextAccessor.HttpContext.Response;
            _ISpeedLimit = speedLimit;
            _logger = logger;
            _IVerifyCode = verifyCode;
            _ISendEmail = sendEmail;

            _IExcel = excel;
        }

        //[RequestSizeLimit(1_074_790_400)]
        [Route("/"), Route("/Home")]
        public IActionResult Index([FromServices] TemplateDbContext templateDbContext)
        {
            _logger.LogDebug("这是一个Debug日志");
            _logger.LogWarning("这是一个警告日志");
            _logger.LogInformation("这是一个信息日志");
            _logger.LogError("这是一个错误日志");
            templateDbContext.UserPermissions.ToList();

            return View();
        }

        [Route("/Download")]
        public JsonResult Download()
        {
            bool result = _ISpeedLimit.Download($"{ _IHostingEnvironment.WebRootPath }/files/TestSpeedLimit.zip");

            if (result)
            {
                return new JsonResult(new
                {
                    status = "success"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    status = "终止传输"
                });
            }
        }


        [Route("/qr/{type}")]
        public FileResult CreateBarQrCode(string type)
        {
            List<string> list = new List<string>() { $"{_IHostingEnvironment.WebRootPath}/image/20190507093909.png" };
            switch (type)
            {
                case "4":
                    return File(_IVerifyCode.CreateImageVerifyCodeToByte(VerifyCodeType.Hybrid, 150, 50), "image/jpeg");
                case "5":
                    return File(_IVerifyCode.CreateImageVerifyCodeToByte(VerifyCodeType.Letter, 150, 50), "image/jpeg");
                case "6":
                    return File(_IVerifyCode.CreateImageVerifyCodeToByte(VerifyCodeType.Number, 150, 50), "image/jpeg");
                    //case "7": return File(Common.Helper.Core.IOExpand.Implementation.FilePackaging.CreateZip(list, null, null, 5).GetBuffer(), "application/octet-stream","hujkekjla.zip");
            }
            return null;
        }

    }
}