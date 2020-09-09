using ClosedXML.Excel;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class WS_SchedulerController : _BaseController
    {
 
        IWS_SchedulerService objSchedulerService;
        public WS_SchedulerController(  IWS_SchedulerService objSchedulerService)
        { 
            this.objSchedulerService = objSchedulerService as WS_SchedulerService;
        }


        // GET: WS_Scheduler
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

        [HttpPost]
        public JsonResult GetList(DataTableViewModel objReq)
        {
            try
            { 
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    DataTableResponse<WS_SchedulerModel> objResp = new DataTableResponse<WS_SchedulerModel>();
                    //IQueryable<BlueNileDiscountModel> query = this.objSvc.GetBlueNileDiscountQueryable();
                    IQueryable<WS_SchedulerModel> query = this.objSchedulerService.SchedulerList();
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "WSID":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.WSID);
                                else
                                    query = query.OrderByDescending(x => x.WSID);
                                break;
                             
                            case "Name":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.Name);
                                else
                                    query = query.OrderByDescending(x => x.Name);
                                break;
                                 
                        }
                    }
                    objResp.draw = objReq.draw;
                     objResp.recordsFiltered = query.Count();
                     objResp.data = query.Skip(objReq.start).Take(objReq.length).ToList();
                    return Json(objResp);
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.Log("SchedulerController", "GetList", ex);
                throw;
            }
        }
        //public JsonResult EditScheduler(int WSID)
        //{
        //    int LoginID = GetLogin();
        //    if (WSID == LoginID)
        //    {
        //        return Json(true);
        //    }
        //    return Json(false);
        //}
        public ActionResult UpdateScheduler(int WSID, string Name, string Frequency, int FrequencyInt, bool Status)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                     
                     int RowCount = this.objSchedulerService.UpdateScheduler(WSID,Name, Frequency, FrequencyInt, Status);
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("SchedulerController", "UpdateScheduler", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }


    }
}