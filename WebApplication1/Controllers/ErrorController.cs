using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class ErrorController:Controller
    {
        private ILogger<ErrorController> logger;
        /// <summary>
        /// 依赖注入ilogger接口
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，你访问的页面不存在";
                    logger.LogWarning($"次异常发生在{statusCodeResult.OriginalPath}，字符串为{statusCodeResult.OriginalQueryString}");//日志记录异常
                    //ViewBag.OriginalPath = statusCodeResult.OriginalPath;
                    //ViewBag.OriginalQueryString = statusCodeResult.OriginalQueryString;
                    break;
            }
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            //获取异常细节
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"次异常发生在{exceptionHandlerPathFeature.Path}，错误为{exceptionHandlerPathFeature.Error}");
            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            return View("Error");
        }
    }
}
