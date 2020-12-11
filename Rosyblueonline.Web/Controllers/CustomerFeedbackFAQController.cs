using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static Rosyblueonline.Framework.Constant;

namespace Rosyblueonline.Web.Controllers
{
    public class CustomerFeedbackFAQController : _BaseController
    {

        private readonly UserDetailService objUDSvc = null;
        public CustomerFeedbackFAQController(IUserDetailService objUDSvc)
        {
            this.objUDSvc = objUDSvc as UserDetailService;
        }

        public ActionResult Index()
        {
            return View();
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.Cookies["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
            }
        }


        public ActionResult GetFAQuestion(int QTypeId)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<mstFAQBankModel> objLFAQ = new List<mstFAQBankModel>();
                    objLFAQ = this.objUDSvc.GetFAQuestions(QTypeId);

                    return Json(new Response { IsSuccess = true, Result = objLFAQ });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });

                ErrorLog.Log("CustomerFeedbackFAQController", "GetFAQuestion", ex);
                throw ex;
            }
        }



    }
}