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

namespace Rosyblueonline.Web.Controllers
{
    public class ChargesController : Controller
    {
        readonly ChargeService objSvc = null;
        public ChargesController(IChargeService objSvc)
        {
            this.objSvc = objSvc as ChargeService;
        }
        // GET: Charges
        [CustomAuthorize("SummaryDiscount")]
        public ActionResult Index()
        {
            List<mstChargesModel> objLst = this.objSvc.GetAllCharges();
            return View(objLst);
        }

        public ActionResult Update(List<KeyValueViewModel> obj)
        {
            try
            {
                int RowCount = this.objSvc.Update(obj);
                return Json(new Response { IsSuccess = true, Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("ChargesController", "Update", ex);
                return Json(new Response { IsSuccess = false, Message = "Some error occured" });
            }
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

    }
}