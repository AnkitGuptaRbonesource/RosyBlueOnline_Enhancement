using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Attribute
{
    public class CheckSession : ActionFilterAttribute, IActionFilter
    {
        public CheckSession()
        {
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TokenLogModel objToken = (TokenLogModel)filterContext.HttpContext.Session["Token"];
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (objToken == null)
                {
                    SessionTimeOut(filterContext);
                }
            }
            else
            {
                if (objToken != null)
                {
                    string TokenID = filterContext.HttpContext.Request.Headers["TokenID"];
                    if (objToken.tokenID != TokenID)
                    {
                        SessionTimeOut(filterContext);
                    }
                }
            }
        }

        private static void SessionTimeOut(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult(440, "Session Timeout");
            }
            else
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
        }


    }

}