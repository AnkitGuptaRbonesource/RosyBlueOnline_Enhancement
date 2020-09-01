using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class MarketingController : _BaseController
    {
        readonly MarketingService objSvc;
        public MarketingController(IMarketingService objSvc)
        {
            this.objSvc = objSvc as MarketingService;
        }
        // GET: Marketing

        #region BlueNile
        public ActionResult BlueNile()
        {
            return View();
        }

        public ActionResult CreateBlueNile(BlueNileDiscountModel obj)
        {
            try
            {
                int RowCount = this.objSvc.AddBlueNileDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateBlueNile", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GridBlueNile(DataTableViewModel objReq)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<BlueNileDiscountModel> objResp = new DataTableResponse<BlueNileDiscountModel>();
                    IQueryable<BlueNileDiscountModel> query = this.objSvc.GetBlueNileDiscountQueryable();
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "caratRange1Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange1Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange1Disc);
                                break;
                            case "caratRange2Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange2Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange2Disc);
                                break;

                            case "caratRange3Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange3Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange3Disc);
                                break;

                            case "caratRange4Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange4Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange4Disc);
                                break;

                            case "caratRange5Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange5Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange5Disc);
                                break;

                            case "caratRange6Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange6Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange6Disc);
                                break;

                            case "caratRange7Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange7Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange7Disc);
                                break;

                            case "caratRange8Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange8Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange8Disc);
                                break;

                            case "caratRange9Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange9Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange9Disc);
                                break;

                            case "caratRange10Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange10Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange10Disc);
                                break;

                            case "caratRange11Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange11Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange11Disc);
                                break;

                            case "caratRange12Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange12Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange12Disc);
                                break;

                            case "caratRange13Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange13Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange13Disc);
                                break;

                            case "caratRange14Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange14Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange14Disc);
                                break;

                            case "caratRange15Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange15Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange15Disc);
                                break;

                            case "caratRange16Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange16Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange16Disc);
                                break;

                            case "caratRange17Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange17Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange17Disc);
                                break;

                            case "caratRange18Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange18Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange18Disc);
                                break;

                            case "caratRange19Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange19Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange19Disc);
                                break;

                            case "caratRange20Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange20Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange20Disc);
                                break;
                            case "caratRange21Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange21Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange21Disc);
                                break;
                            case "caratRange22Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange22Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange22Disc);
                                break;

                            case "caratRange23Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange23Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange23Disc);
                                break;

                            case "caratRange24Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange24Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange24Disc);
                                break;

                            case "caratRange25Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange25Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange25Disc);
                                break;

                            case "haExDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haExDisc);
                                else
                                    query = query.OrderByDescending(x => x.haExDisc);
                                break;

                            case "haVgDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haVgDisc);
                                else
                                    query = query.OrderByDescending(x => x.haVgDisc);
                                break;

                            case "createdOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.createdOn);
                                else
                                    query = query.OrderByDescending(x => x.createdOn);
                                break;

                            default:
                            case "SrNo":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.SrNo);
                                else
                                    query = query.OrderByDescending(x => x.SrNo);
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
                ErrorLog.Log("MarketingController", "GridBlueNile", ex);
                throw;
            }
        }

        #endregion

        #region JamesAllen
        public ActionResult JamesAllen()
        {
            return View();
        }

        public ActionResult CreateJamesAllen(JamesAllenDiscountModel obj)
        {
            try
            {
                int RowCount = this.objSvc.AddJamesAllenDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateJamesAllen", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GridJamesAllen(DataTableViewModel objReq)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<JamesAllenDiscountModel> objResp = new DataTableResponse<JamesAllenDiscountModel>();
                    IQueryable<JamesAllenDiscountModel> query = this.objSvc.GetJamesAllenDiscountQueryable();
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "caratRange1Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange1Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange1Disc);
                                break;
                            case "caratRange2Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange2Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange2Disc);
                                break;

                            case "caratRange3Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange3Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange3Disc);
                                break;

                            case "caratRange4Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange4Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange4Disc);
                                break;

                            case "caratRange5Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange5Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange5Disc);
                                break;

                            case "caratRange6Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange6Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange6Disc);
                                break;

                            case "caratRange7Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange7Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange7Disc);
                                break;

                            case "caratRange8Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange8Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange8Disc);
                                break;

                            case "caratRange9Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange9Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange9Disc);
                                break;

                            case "caratRange10Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange10Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange10Disc);
                                break;

                            case "caratRange11Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange11Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange11Disc);
                                break;

                            case "caratRange12Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange12Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange12Disc);
                                break;

                            case "caratRange13Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange13Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange13Disc);
                                break;

                            case "caratRange14Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange14Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange14Disc);
                                break;

                            case "caratRange15Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange15Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange15Disc);
                                break;

                            case "caratRange16Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange16Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange16Disc);
                                break;

                            case "caratRange17Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange17Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange17Disc);
                                break;

                            case "caratRange18Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange18Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange18Disc);
                                break;

                            case "caratRange19Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange19Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange19Disc);
                                break;

                            case "caratRange20Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange20Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange20Disc);
                                break;
                            case "caratRange21Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange21Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange21Disc);
                                break;
                            case "caratRange22Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange22Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange22Disc);
                                break;

                            case "caratRange23Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange23Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange23Disc);
                                break;

                            case "caratRange24Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange24Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange24Disc);
                                break;

                            case "caratRange25Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange25Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange25Disc);
                                break;

                            case "CNRDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.CNRDisc);
                                else
                                    query = query.OrderByDescending(x => x.CNRDisc);
                                break;

                            case "haExDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haExDisc);
                                else
                                    query = query.OrderByDescending(x => x.haExDisc);
                                break;

                            case "haVgDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haVgDisc);
                                else
                                    query = query.OrderByDescending(x => x.haVgDisc);
                                break;

                            case "cnrDiscHA":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.cnrDiscHA);
                                else
                                    query = query.OrderByDescending(x => x.cnrDiscHA);
                                break;

                            case "createdOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.createdOn);
                                else
                                    query = query.OrderByDescending(x => x.createdOn);
                                break;

                            default:
                            case "SrNo":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.SrNo);
                                else
                                    query = query.OrderByDescending(x => x.SrNo);
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
                ErrorLog.Log("MarketingController", "GridJamesAllen", ex);
                throw;
            }
        }

        #endregion

        #region JamesAllen HK
        public ActionResult JamesAllenHK()
        {
            return View();
        }

        public ActionResult CreateJamesAllenHK(JamesAllenDiscountHKModel obj)
        {
            try
            {
                int RowCount = this.objSvc.AddJamesAllenDiscountHK(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateJamesAllen", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GridJamesAllenHK(DataTableViewModel objReq)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<JamesAllenDiscountHKModel> objResp = new DataTableResponse<JamesAllenDiscountHKModel>();
                    IQueryable<JamesAllenDiscountHKModel> query = this.objSvc.GetJamesAllenDiscountHKQueryable();
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "caratRange1Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange1Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange1Disc);
                                break;
                            case "caratRange2Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange2Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange2Disc);
                                break;

                            case "caratRange3Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange3Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange3Disc);
                                break;

                            case "caratRange4Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange4Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange4Disc);
                                break;

                            case "caratRange5Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange5Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange5Disc);
                                break;

                            case "caratRange6Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange6Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange6Disc);
                                break;

                            case "caratRange7Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange7Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange7Disc);
                                break;

                            case "caratRange8Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange8Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange8Disc);
                                break;

                            case "caratRange9Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange9Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange9Disc);
                                break;

                            case "caratRange10Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange10Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange10Disc);
                                break;

                            case "caratRange11Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange11Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange11Disc);
                                break;

                            case "caratRange12Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange12Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange12Disc);
                                break;

                            case "caratRange13Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange13Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange13Disc);
                                break;

                            case "caratRange14Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange14Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange14Disc);
                                break;

                            case "caratRange15Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange15Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange15Disc);
                                break;

                            case "caratRange16Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange16Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange16Disc);
                                break;

                            case "caratRange17Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange17Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange17Disc);
                                break;

                            case "caratRange18Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange18Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange18Disc);
                                break;

                            case "caratRange19Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange19Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange19Disc);
                                break;

                            case "caratRange20Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange20Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange20Disc);
                                break;
                            case "caratRange21Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange21Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange21Disc);
                                break;
                            case "caratRange22Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange22Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange22Disc);
                                break;

                            case "caratRange23Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange23Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange23Disc);
                                break;

                            case "caratRange24Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange24Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange24Disc);
                                break;

                            case "caratRange25Disc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.caratRange25Disc);
                                else
                                    query = query.OrderByDescending(x => x.caratRange25Disc);
                                break;

                            case "CNRDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.CNRDisc);
                                else
                                    query = query.OrderByDescending(x => x.CNRDisc);
                                break;

                            case "haExDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haExDisc);
                                else
                                    query = query.OrderByDescending(x => x.haExDisc);
                                break;

                            case "haVgDisc":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.haVgDisc);
                                else
                                    query = query.OrderByDescending(x => x.haVgDisc);
                                break;

                            case "cnrDiscHA":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.cnrDiscHA);
                                else
                                    query = query.OrderByDescending(x => x.cnrDiscHA);
                                break;

                            case "createdOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.createdOn);
                                else
                                    query = query.OrderByDescending(x => x.createdOn);
                                break;

                            default:
                            case "SrNo":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.SrNo);
                                else
                                    query = query.OrderByDescending(x => x.SrNo);
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
                ErrorLog.Log("MarketingController", "GridJamesAllenHK", ex);
                throw;
            }
        }

        #endregion

    }
}