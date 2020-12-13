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


        public ActionResult GetFeedbackQuestion(int QTypeId, string Flag)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<mstFAQBankModel> objLFAQ = new List<mstFAQBankModel>();
                    objLFAQ = this.objUDSvc.GetFAQuestions(QTypeId, Flag);

                    return Json(new Response { IsSuccess = true, Result = objLFAQ });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CustomerFeedbackFAQController", "GetFAQuestion", ex);
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });

            }
        }



        public ActionResult SubmitFAQAnswers(string FAQId, string FAQTypeID, string OptionId, string TextAnswer)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    CustomerFAQAnswersModel objLFAQA = new CustomerFAQAnswersModel();
                    int FAqId = this.objUDSvc.SubmitFAQAnswers(Convert.ToInt32(FAQId), Convert.ToInt32(FAQTypeID), OptionId, TextAnswer, LoginID);
                    if (FAqId > 0)
                    {
                        return Json(new Response { IsSuccess = true, Result = FAqId });
                    }
                    else
                    {
                        return Json(new Response { IsSuccess = false, Result = 0 });

                    }

                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CustomerFeedbackFAQController", "GetFAQuestion", ex);

                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });


            }
        }


        public ActionResult GetBindPreviousQuestions(int QTypeId)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    CustomerFAQAnswersModel objFAQAL = new CustomerFAQAnswersModel();
                    objFAQAL = this.objUDSvc.GetBindPreviousQuestions(QTypeId, LoginID);

                    return Json(new Response { IsSuccess = true, Result = objFAQAL });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CustomerFeedbackFAQController", "GetBindPreviousQuestions", ex);
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });

            }
        }


        public ActionResult GetTotalFAQCount(int QTypeId)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objUDSvc.GetTotalFAQCount();

                    return Json(new Response { IsSuccess = true, Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CustomerFeedbackFAQController", "GetBindPreviousQuestions", ex);
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });

            }
        }

    }
}