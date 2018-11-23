using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SmlGround.Filters
{
    public class LogInfoAttribute : ActionFilterAttribute
    {
        private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logger.Info($"{filterContext.HttpContext.User.Identity.Name} c IP-{filterContext.HttpContext.Request.UserHostAddress} зашёл на {filterContext.HttpContext.Request.RawUrl}");
        }
    }
}