using Newtonsoft.Json;
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

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class MarketingController : _BaseController
    {
        readonly MarketingService objSvc;
        IStockDetailsService objStockDetailsService;
        IDownloadScriptService objDownloadService = null;
        public MarketingController(IMarketingService objSvc, IStockDetailsService objStockDetailsService, IDownloadScriptService objDownloadService)
        {
            this.objSvc = objSvc as MarketingService;
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objDownloadService = objDownloadService as DownloadScriptService;

        }
        // GET: Marketing

        #region BlueNile
        [CustomAuthorize("BlueNile")]
        public ActionResult BlueNile()
        {
            return View();
        }

        public ActionResult CreateBlueNile(BlueNileDiscountModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.createdBy = LoginId;
                obj.Isactive = true;
                int RowCount = this.objSvc.AddBlueNileDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateBlueNile", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult EditBlueNile(int id)
        {
            try
            {
                BlueNileDiscountModel obj = new BlueNileDiscountModel();
                obj = this.objSvc.EditBlueNile(id);
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "EditBlueNile", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult UpdateBlueNile(BlueNileDiscountModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.UpdatedBy = LoginId;
                int RowCount = this.objSvc.UpdateBlueNileDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "UpdateBlueNile", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        public ActionResult DeleteBlueNile(int id)
        {
            try
            {
                int RowCount = this.objSvc.DeleteBlueNileDiscount(id);
                return Json(RowCount, JsonRequestBehavior.AllowGet);

                // return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "DeleteBlueNile", ex);
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
        [CustomAuthorize("JamesAllen")]
        public ActionResult JamesAllen()
        {
            return View();
        }

        public ActionResult CreateJamesAllen(JamesAllenDiscountModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.createdBy = LoginId;
                obj.Isactive = true;
                int RowCount = this.objSvc.AddJamesAllenDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateJamesAllen", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult EditJamesAllen(int id)
        {
            try
            {
                JamesAllenDiscountModel obj = new JamesAllenDiscountModel();
                obj = this.objSvc.EditJamesAllen(id);
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "EditJamesAllen", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult UpdateJamesAllen(JamesAllenDiscountModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.UpdatedBy = LoginId;
                int RowCount = this.objSvc.UpdateJamesAllenDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "UpdateJamesAllenDiscount", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        public ActionResult DeleteJamesAllen(int id)
        {
            try
            {
                int RowCount = this.objSvc.DeleteJamesAllenDiscount(id);
                return Json(RowCount, JsonRequestBehavior.AllowGet);

                // return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "DeleteJamesAllenDiscount", ex);
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
        [CustomAuthorize("JamesAllenHK")]
        public ActionResult JamesAllenHK()
        {
            return View();
        }

        public ActionResult CreateJamesAllenHK(JamesAllenDiscountHKModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.createdBy = LoginId;
                obj.Isactive = true;
                int RowCount = this.objSvc.AddJamesAllenDiscountHK(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "CreateJamesAllen", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult EditJamesAllenHK(int id)
        {
            try
            {
                JamesAllenDiscountHKModel obj = new JamesAllenDiscountHKModel();
                obj = this.objSvc.EditJamesAllenHK(id);
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "EditJamesAllenHK", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult UpdateJamesAllenHK(JamesAllenDiscountHKModel obj)
        {
            try
            {
                int LoginId = GetLogin();
                obj.UpdatedBy = LoginId;
                int RowCount = this.objSvc.UpdateJamesAllenHKDiscount(obj);
                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "UpdateJamesAllenHKDiscount", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        public ActionResult DeleteJamesAllenHK(int id)
        {
            try
            {
                int RowCount = this.objSvc.DeleteJamesAllenHKDiscount(id);
                return Json(RowCount, JsonRequestBehavior.AllowGet);

                // return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "DeleteJamesAllenHKDiscount", ex);
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


        #region MarketingStockSummary
        [CustomAuthorize("SellSummary")]
        public ActionResult MarketingStockSummary()
        {
            int LoginId = GetLogin();
            MarketingStockSummaryModel objVm = new MarketingStockSummaryModel();
            objVm = this.objSvc.StockSummaryFilters(LoginId);
            return View(objVm);


        }

        //[HttpGet]
        //public ActionResult GetListOfCustomer(string search)
        //{
        //    int LoginId = GetLogin();
        //    var objLst = this.objSvc.StockSummaryFilters(LoginId);
        //   var objCustList = objLst.SaleS .Where(x => x.Name.Contains(search)).ToList();
        //    return Json(objCustList, JsonRequestBehavior.AllowGet);


        //}


        [HttpPost]
        public ActionResult MarketingStockSummaryDetails(string CustomerIDs, string FilterYear, string FilterMonth, string salesLocationIDs)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                {
                    List<MarketingStockSummaryDetailsModel> objVM = objSvc.MarketingStockSummaryDetails(CustomerIDs, FilterYear, FilterMonth, salesLocationIDs);

                    return Json(objVM, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "GridJamesAllenHK", ex);
                throw;
            }
        }



        [HttpPost]
        public ActionResult StockList2(string LotNos)
        {
            try
            {
                int LoginID = GetLogin();
                string OrderBy = "", OrderDirection = "";
                if (LoginID > 0)
                {
                    //if (obj.order.Count > 0)
                    //{
                    //    OrderBy = obj.columns[obj.order[0].column].data;
                    //    OrderDirection = obj.order[0].dir;
                    //} 
                    //decimal idx = Math.Ceiling((decimal)(obj.start / obj.length));
                    List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
                    string raisedEvent = "SpecificSearch";
                    string filterText = "LOTNO~" + LotNos;
                    //objInvVM = objStockDetailsService.InventoryAction(LoginID.ToString(),filterText, idx.ToString(), obj.length.ToString(), OrderBy, OrderDirection, raisedEvent, "SpecialSearch");
                    objInvVM = objStockDetailsService.InventoryAction(LoginID.ToString(), filterText, "0", "500000", OrderBy, OrderDirection, raisedEvent, "SpecialSearch");


                    return Json(objInvVM, JsonRequestBehavior.AllowGet);

                    //string json = JsonConvert.SerializeObject(new { draw = obj.draw, recordsTotal = obj.Total.HasValue ? obj.Total.Value : 0, recordsFiltered = obj.Total.HasValue ? obj.Total.Value : 0, data = objInvVM }, Formatting.Indented);

                    //return Content(json, "application/json");
                }

                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "StockList2", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        #endregion



        #region MarketInventory
        [CustomAuthorize("MarketInventoryUpload")]
        public ActionResult MarketInventoryUpload()
        {
            return View();
        }



        [HttpPost]
        public ActionResult MarketUploadInventory()
        {

            string path = "";
            string fileExtn = "";
            string fileId = "";

            string procName = "";
            int RowCount = 0;
            CommonFunction commonFunction = new CommonFunction();
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string InventoryuploadType = "";
                    if (Request.Files.Count > 0)
                    {
                        fileExtn = Path.GetExtension(Request.Files[0].FileName);
                        string ip = Request.UserHostAddress;
                        path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                        if (fileExtn == ".xls" || fileExtn == ".xlsx")
                        {
                            fileId = "MarketInventoryUpload_File_" + DateTime.Now.ToString("MMddyyyyHHmm");
                            Request.Files[0].SaveAs(path + fileId.ToString() + fileExtn);
                            DataTable ds = commonFunction.GetDataFromExcel2(path + fileId.ToString() + fileExtn);

                            procName = "proc_MarketInventoryUpload";
                            DataTable pds = ParseToString(ds);
                            List<InventoryUpload> objLst = objStockDetailsService.InventoryUpload(pds, procName, Convert.ToString(LoginID), Convert.ToString(1000), Request.UserHostAddress, InventoryuploadType);


                            string json = JsonConvert.SerializeObject(new Response
                            {
                                Code = 200,
                                IsSuccess = true,
                                Message = InventoryuploadType,
                                Result = new
                                {
                                    List = objLst,
                                    FileID = fileId
                                }
                            }, Formatting.Indented);
                            return Content(json, "application/json");

                        }
                        else
                        {
                            return RedirectToAction("Marketing", "MarketInventoryUpload");
                        }
                    }
                    else
                    {
                        return Json(new Response { Code = 500, IsSuccess = false, Message = "File not attached", Result = null });
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }



            }
            catch (Exception ex)
            {
                return Json(new Response
                {
                    Code = 500,
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                });
            }
        }


        private DataTable ParseToString(DataTable oldDT)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < oldDT.Columns.Count; i++)
            {

                dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName, typeof(string)));

            }

            for (int i = 0; i < oldDT.Rows.Count; i++)
            {

                DataRow dr = dt.NewRow();
                for (int j = 0; j < oldDT.Columns.Count; j++)
                {
                    if (Convert.ToString(oldDT.Rows[i][j]).Trim() != "")
                    {

                        dr[j] = oldDT.Rows[i][j];

                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        [CustomAuthorize("MarketInventoryDownload")]
        public ActionResult MarketInventoryDownload()
        {
            int LoginID = GetLogin();
            int RoleID = GetRole();
            List<DownloadList> objLst = this.objDownloadService.GetMarketDownloadForMenu(LoginID);
            return View(objLst);
        }

        [HttpPost]
        public JsonResult MarketdownloadForExcel(string id, string FileName)
        {
            try
            {
                int LoginID = GetLogin();
                int Role = GetRole();
                if (LoginID > 0)
                {

                    DataTable dt = this.objDownloadService.MarketInventoryDownloadExcelExport(LoginID.ToString(), id);
                    if (dt.Rows.Count > 0)
                    {
                        TempData["MarketFileName"] = FileName;
                        string imgPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelMailImage"]);
                        // byte[] st = ExportToExcel.InventoryExportToExcel(dt, imgPath, Role == 3 ? true : false, "AT", false);
                        byte[] st = ExportToExcel.DownloadExcel(FileName, dt);

                        TempData["MarketDataExport"] = st;
                        return Json(new Response { IsSuccess = true, Result = "" });
                    }
                    else
                    {
                        return Json(new Response { IsSuccess = false, Message = "No rows found" });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "MarketdownloadForExcel", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MarketExportToExcelInventoryDownload()
        {
            try
            {
                string filename = TempData["MarketFileName"].ToString();
                byte[] st = (byte[])TempData["MarketDataExport"];
                return File(st, System.Net.Mime.MediaTypeNames.Application.Octet, filename + ".xlsx");
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "MarketExportToExcelInventoryDownload", ex);
                return new EmptyResult();
            }
        }


        [HttpPost]
        public JsonResult DeleteMarketInventory(string id)
        {
            try
            {
                int LoginID = GetLogin();
                int Role = GetRole();
                if (LoginID > 0)
                {

                    DataTable dt = this.objDownloadService.MarketInventoryDownloadExcelExport(LoginID.ToString(), "DeleteData");
                    if (dt.Rows.Count > 0)
                    { 
                        return Json(new Response { IsSuccess = true, Result = "" });
                    }
                    else
                    {
                        return Json(new Response { IsSuccess = false, Message = "No rows found" });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "DeleteMarketInventory", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }





        #endregion


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