using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;

namespace SmlGround.Filters
{
    public class NullException : FilterAttribute , IExceptionFilter
    {
        private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is NullReferenceException)
            {
                logger.Error($"Ошибка {exceptionContext.Exception} произошла по адрессу {exceptionContext.HttpContext.Request.RawUrl}");
                exceptionContext.Result = new HttpNotFoundResult();
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}
