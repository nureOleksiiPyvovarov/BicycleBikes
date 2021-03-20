using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace BikeStore.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> _logger)
        {
            this._logger = _logger;
        }
        [AllowAnonymous]
        [Route("/Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path {exceptionHandlerPathFeature.Path} " +
                            $"threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult =
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource could not be found";
                    _logger.LogWarning($"404 error occured. Path = " +
                                      $"{statusCodeResult.OriginalPath} and QueryString = " +
                                      $"{statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("StatusCodes/UrlNotFound");
        }
    }
}