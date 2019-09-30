﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class ErrorController:Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，你访问的页面不存在";
                    ViewBag.OriginalPath = statusCodeResult.OriginalPath;
                    ViewBag.OriginalQueryString = statusCodeResult.OriginalQueryString;
                    break;
            }
            return View("NotFound");
        }
    }
}
