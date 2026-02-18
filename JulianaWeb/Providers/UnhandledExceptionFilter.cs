using JulianaWeb.Interfaces.Aspects;
using JulianaWeb.Services.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace JulianaWeb
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILoggerService<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter()
        {
          _logger = LoggerServiceFactory.Get<UnhandledExceptionFilter>();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            Debug.WriteLine($"Error on: {context.Exception.Message}");
            _logger.LogError("Error", context.Exception);
            Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(context.Exception));
        }
    }
}
