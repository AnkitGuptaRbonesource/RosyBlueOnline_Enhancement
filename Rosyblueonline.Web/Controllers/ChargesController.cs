using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}