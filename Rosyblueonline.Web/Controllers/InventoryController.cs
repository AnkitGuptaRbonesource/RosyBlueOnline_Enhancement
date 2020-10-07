using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using static Rosyblueonline.Web.FilterConfig;

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class InventoryController : _BaseController
    {
        IStockDetailsService objStockDetailsService;
        IRecentSearchService objRecentSearchService;
        IMemoService objMemoService;
        IDownloadScriptService objDownloadService = null;
        CommonFunction commonFunction;
        MailUtility objMU = null;
        //IUserDetailService objStockDetailsService;
        private readonly UserDetailService objUDSvc = null;
        public InventoryController(IStockDetailsService objStockDetailsService, IRecentSearchService objRecentSearchService, IMemoService objMemoService, IDownloadScriptService objDownloadService, IUserDetailService objUDSvc)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objRecentSearchService = objRecentSearchService as RecentSearchService;
            this.objMemoService = objMemoService as MemoService;
            this.objDownloadService = objDownloadService as DownloadScriptService;
            this.objUDSvc = objUDSvc as UserDetailService;
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
        // GET: Inventory
        #region StockSearch
        public ActionResult SpecificSearch()
        {
            int LoginID = GetLogin();
            _SearchFilterViewModel objVM = new _SearchFilterViewModel();
            objVM = objStockDetailsService.SearchFilter(LoginID);
            return View(objVM);
        }

        [HttpPost]
        public ActionResult SpecificSearchPost(string Query)
        {
            int LoginID = GetLogin();
            _SearchFilterViewModel objVM = new _SearchFilterViewModel();
            objVM = objStockDetailsService.SearchFilter(LoginID);
            objVM.Query = Query;
            
            bool log = this.objUDSvc.UserActivitylogs(LoginID, "Dashboard Search", Query);
            List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
            objInvVM = objStockDetailsService.InventoryAction(LoginID.ToString(), Query, "0", "10", "", "", "SpecificSearch");

            // ViewBag.SearchResultList = objInvVM.Take(10);  
            return View("SpecificSearch", objVM);
        }

        [HttpPost]
        public ActionResult SpecificSearchListView(string Query)
        {
            int LoginID = GetLogin();
            List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
            objInvVM = objStockDetailsService.InventoryAction(LoginID.ToString(), Query, "0", "10000000", "Stock", "desc", "SpecificSearch");
             

            // ViewBag.SearchResultList = objInvVM.Take(10); ;
            //  return View("SpecificSearch", objVM);
            // return PartialView("~/Views/Shared/_PartialSpecificSearchResult.cshtml", "SS");
            return Json(new Response { IsSuccess = true, Message = "200", Result = objInvVM });

        }

        [HttpPost]
        public JsonResult StockList(string filterText)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
                    objInvVM = objStockDetailsService.InventoryAction(LoginID.ToString(), filterText, "0", "50", "SpecificSearch");
                    return Json(new Response { IsSuccess = true, Message = "200", Result = objInvVM });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "StockList", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult StockList2(DataTableViewModel obj, bool AddToRecentSearch = true, bool NewArrival = false)
        {
            try
            {
                int LoginID = GetLogin();
                string OrderBy = "", OrderDirection = "";
                if (LoginID > 0)
                {
                    if (obj.order.Count > 0)
                    {
                        OrderBy = obj.columns[obj.order[0].column].data;
                        OrderDirection = obj.order[0].dir;
                    }
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Specific Search", obj.filterText);
                    if (AddToRecentSearch == true)
                    {//isactive = true,searchWhere = ""
                        this.objRecentSearchService.Add(new RecentSearchModel { createdBy = LoginID, Createdon = DateTime.Now, searchCriteria = obj.filterText, displayCriteria = obj.displayCriteria, searchType = "SpecificSearch", searchCriteriaName = "" });

                    }
                    decimal idx = Math.Ceiling((decimal)(obj.start / obj.length));
                    List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
                    objInvVM = NewArrival == false ? objStockDetailsService.InventoryAction(LoginID.ToString(), obj.filterText, idx.ToString(), obj.length.ToString(), OrderBy, OrderDirection, "SpecificSearch") :
                                                     objStockDetailsService.InventoryAction(LoginID.ToString(), obj.filterText, idx.ToString(), obj.length.ToString(), OrderBy, OrderDirection, "SpecificSearch", "NewArrival");


                    string json = JsonConvert.SerializeObject(new { draw = obj.draw, recordsTotal = obj.Total.HasValue ? obj.Total.Value : 0, recordsFiltered = obj.Total.HasValue ? obj.Total.Value : 0, data = objInvVM }, Formatting.Indented);
                 //ViewBag.SearchResultList = objInvVM.Take(5);
                    return Content(json, "application/json");
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "StockList2", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SummaryData(string lotnumber)
        {
            try
            {
                SummaryCalsViewModel objVM = new SummaryCalsViewModel();
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    objVM = objStockDetailsService.SummaryCalculation(LoginID.ToString(), lotnumber, "", "", "", "", "summaryCalc");
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "200", Result = objVM });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "SummaryData", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult StockCount(string filterText, bool NewArrival = false)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<InventoryCountViewModel> StockCount = NewArrival == false ?
                        objStockDetailsService.InventoryCount(LoginID.ToString(), filterText, "0", "500000", "", "", "SpecificSearchCount") :
                        objStockDetailsService.InventoryCount(LoginID.ToString(), filterText, "0", "500000", "", "", "SpecificSearchCount", "NewArrival");
                    return Json(new Response { IsSuccess = true, Message = "200", Result = StockCount });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "StockCount", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetStockForPrint(string filterText)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    DataTable dt = this.objStockDetailsService.GetDataForExcelExportForPrint(filterText, LoginID);
                    string dtStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    return Json(new Response { IsSuccess = true, Message = "200", Result = dtStr });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetStockForPrint", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetStockForPrintMemo(int OrderID)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    OrderInfoViewModel objVM = this.objMemoService.MemoInfo(OrderID);
                    if (objVM != null)
                    {
                        string LotNos = "LOTNO~" + string.Join(",", objVM.OrderItemDetail.Select(x => x.Stock).ToArray());
                        DataTable dt = this.objStockDetailsService.GetDataForExcelExportForPrint(LotNos, LoginID);
                        string dtStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                        return Json(new Response { IsSuccess = true, Message = "200", Result = dtStr });
                    }
                    return Json(new Response { IsSuccess = false, Message = StringResource.DoesNotExist });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetStockForPrintMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetStockForMailMemo(int OrderID)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    OrderInfoViewModel objVM = this.objMemoService.MemoInfo(OrderID);
                    if (objVM != null)
                    {
                        string LotNos = "LOTNO~" + string.Join(",", objVM.OrderItemDetail.Select(x => x.Stock).ToArray());
                        DataTable dt = this.objStockDetailsService.GetDataForExcelExportForPrint(LotNos, LoginID);
                        string dtStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                        return Json(new Response { IsSuccess = true, Message = "200", Result = dtStr });
                    }
                    return Json(new Response { IsSuccess = false, Message = StringResource.DoesNotExist });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetStockForPrintMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        #endregion

        #region Cart

        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddtoCart(string filterText)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    CartCountViewModel rst = objStockDetailsService.Cart(LoginID.ToString(), TokenId, filterText, "AddtoCart");
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Add to Cart", filterText);

                    return Json(new Response { IsSuccess = true, Message = "Inventory Added into Cart", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddtoCart", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        [HttpGet]
        public ActionResult GetCart()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
                    objInvVM = objStockDetailsService.GetCartandWatch(LoginID.ToString(), TokenId.ToString(), "", "GetCart");
                    return Json(new Response { IsSuccess = false, Message = "", Result = objInvVM }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "Cart", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        //[HttpGet]
        //public ActionResult GetCartForExcel()
        //{
        //    try
        //    {
        //        int LoginID = GetLogin();
        //        if (LoginID > 0)
        //        {
        //            string TokenId = GetToken();
        //            List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
        //            objInvVM = objStockDetailsService.GetCartandWatch(LoginID.ToString(), TokenId.ToString(), "", "GetCart");
        //            return Json(new Response { IsSuccess = false, Message = "", Result = objInvVM }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.Log("InventoryController", "Cart", ex);
        //        return Json(new Response { IsSuccess = false, Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public JsonResult RemoveCart(string LotNos)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    CartCountViewModel rst = objStockDetailsService.Cart(LoginID.ToString(), TokenId, LotNos, "DeleteCart");
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Remove from Cart", LotNos);

                    return Json(new Response { IsSuccess = true, Message = "Inventory Removed From Cart", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "RemoveCart", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
        #endregion

        #region WatchList
        [HttpGet]
        public ActionResult Watch()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddtoWatchList(string filterText)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    WatchListCountViewModel rst = objStockDetailsService.Watch(LoginID.ToString(), TokenId, filterText, "AddtoWatchList");

                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Add to Watch List", filterText);
                    return Json(new Response { IsSuccess = true, Message = "Inventory Added into Watchlist", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddtoWatchList", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        [HttpGet]
        public ActionResult GetWatchList()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> objInvVM = new List<inventoryDetailsViewModel>();
                    objInvVM = objStockDetailsService.GetCartandWatch(LoginID.ToString(), "", "", "GetWatchList");
                    return Json(new Response { IsSuccess = true, Message = "", Result = objInvVM }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetWatchList", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult RemoveWatchList(string filterText)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    WatchListCountViewModel rst = objStockDetailsService.Watch(LoginID.ToString(), TokenId, filterText, "DeleteWatchList");
                    return Json(new Response { IsSuccess = true, Message = "Inventory Removed From WatchList", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "RemoveWatchList", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }


        #endregion

        #region recentSearch
        public JsonResult AddRecent(RecentSearchModel obj)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    obj.createdBy = LoginID;
                    obj.Createdon = DateTime.Now;
                    int RowCount = obj.recentSearchID == 0 ? objRecentSearchService.Add(obj) : objRecentSearchService.Edit(obj);
                    return Json(new Response { IsSuccess = true, Message = "200", Result = RowCount });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddRecent", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        public JsonResult GetRecentForOptions(string SearchType)
        {
            try
            {
                var lstData = objRecentSearchService.GetForOptions(SearchType);
                return Json(new Response { IsSuccess = true, Message = "200", Result = lstData });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddRecent", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        #endregion

        #region Inventory Upload
        //[HttpGet]
        //public ActionResult InventoryUpload()
        //{
        //    try
        //    {
        //        int LoginID = GetLogin();
        //        if (LoginID > 0)
        //        {
        //            List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
        //            objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
        //            return View(objVM);
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.Log("InventoryController", "AddtoCart", ex);
        //        return Json(new Response { IsSuccess = false, Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult InventoryUpload(int uploadFormatId)
        {
            string path = "";
            string fileExtn = "";
            int fileId = 0;
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
                    objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
                    string InventoryuploadType = objVM.Where(x => x.uploadFormatId == uploadFormatId).Select(x => x.uploadValue).FirstOrDefault();
                    if (Request.Files.Count > 0)
                    {
                        fileExtn = Path.GetExtension(Request.Files[0].FileName);
                        //string fileExtn1 = Path.GetFullPath(Request.Files[0].FileName);
                        string ip = Request.UserHostAddress;
                        path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                        if (fileExtn == ".xls" || fileExtn == ".xlsx")
                        {
                            fileId = this.objStockDetailsService.InsertFileUploadLog(Request.Files[0].FileName.ToString(), Request.Files[0].FileName.ToString(), LoginID.ToString(), Request.UserHostAddress, uploadFormatId.ToString(), InventoryuploadType);
                            Request.Files[0].SaveAs(path + fileId.ToString() + fileExtn);
                            List<InventoryUpload> objLst = UploadInventory(LoginID, InventoryuploadType, fileExtn, path, fileId);
                            if (objLst.Count > 0)
                            {
                                path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["INVUpload"]);
                                List<InventoryUpload> objValidLst = objLst.Where(x => (x.LotStatus != null && x.LotStatus.ToLower() == "valid")).ToList();
                                List<InventoryUpload> objInValidLst = objLst.Where(x => (x.LotStatus != null && x.LotStatus.ToLower().Contains("invalid"))).ToList();
                                DataTable dtValid = ListtoDataTable.ToDataTable<InventoryUpload>(objValidLst);
                                DataTable dtNotValid = ListtoDataTable.ToDataTable<InventoryUpload>(objInValidLst);
                                ExportToExcel.SaveExcel(path, fileId.ToString() + "_Valid", "Valid", dtValid);
                                ExportToExcel.SaveExcel(path, fileId.ToString() + "_InValid", "InValid", dtNotValid);
                                if (InventoryuploadType == "MEMO_CANCEL" || InventoryuploadType == "INVENTORY_UPLOAD" || InventoryuploadType == "CHANGE_DISCOUNT" || InventoryuploadType == "MEMO_RETURN_SALE")
                                {
                                    fileUploadLogModel objFile = this.objMemoService.GetFileByID(fileId);
                                    if (objFile != null)
                                    {
                                        objFile.validInv = dtValid.Rows.Count;
                                        objFile.invalidInv = dtNotValid.Rows.Count;
                                        this.objMemoService.UpdateFile(objFile);
                                    }
                                }
                                SendMailOnUploadEvent(InventoryuploadType,
                                                      (path + fileId.ToString() + "_Valid" + ".xlsx"),
                                                      (path + fileId.ToString() + "_InValid" + ".xlsx"),
                                                      (path + fileId.ToString() + "_backup" + ".xlsx"),
                                                      dtValid.Rows.Count,
                                                      dtNotValid.Rows.Count,
                                                      fileId);
                            }

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

                            //return Json(new Response
                            //{
                            //    Code = 200,
                            //    IsSuccess = true,
                            //    Message = InventoryuploadType,
                            //    Result = new
                            //    {
                            //        List = objLst,
                            //        FileID = fileId
                            //    }
                            //});
                        }
                        else
                        {
                            return RedirectToAction("Upload", "Inventory");
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
                //String paths = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["INVUpload"]);
                //ErrorLog.Log("InventoryController", path, ex);
                return Json(new Response
                {
                    Code = 500,
                    IsSuccess = false,
                    Message = ex.Message,// + "path:" + path + " ,fileid:" + fileId.ToString() + " ,extension:" + fileExtn,
                    Result = null
                });
                //throw ex;
            }
        }

        private List<InventoryUpload> UploadInventory(int LoginID, string InventoryuploadType, string fileExtn, string path, int fileId)
        {
            string procName = "";
            int RowCount = 0;
            this.commonFunction = new CommonFunction();
            if (fileId > 0)
            {
                DataTable ds = commonFunction.GetDataFromExcel2(path + fileId.ToString() + fileExtn);

                //DataTable ds1 = commonFunction.GetDataFromExcel2(path + fileId.ToString() + fileExtn);
                if (InventoryuploadType == "INVENTORY_UPLOAD")
                {
                    procName = "proc_InventoryUpload";
                    DataTable pds = ParseToString(ds);
                    return objStockDetailsService.InventoryUpload(pds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "INVENTORY_MODIFY" || InventoryuploadType == "JA_BN")
                {
                    procName = "proc_QCModifyInventory";
                    ds = RemoveBlankRows(ds);
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "CHANGE_DISCOUNT")
                {
                    //NotAppicableConfMemo
                    procName = "proc_DiscModifyInventory";
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);
                    string LotIDs = string.Join(",", objLst.Where(x => x.Stock != null && x.Stock != "").Select(x => x.Stock).ToArray<string>());
                    List<inventoryDetailsViewModel> objInvLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotIDs);
                    ExportToExcel.SaveExcel(path, fileId.ToString() + "_backup" + fileExtn, "Back", ListtoDataTable.ToDataTable<inventoryDetailsViewModel>(objInvLst));
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "CHANGE_RAPP")
                {
                    procName = "proc_RapModifyInventory";
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);
                    string LotIDs = string.Join(",", objLst.Where(x => x.Stock != null && x.Stock != "").Select(x => x.Stock).ToArray<string>());
                    List<inventoryDetailsViewModel> objInvLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotIDs);
                    ExportToExcel.SaveExcel(path, fileId.ToString() + "_backup" + fileExtn, "Back", ListtoDataTable.ToDataTable<inventoryDetailsViewModel>(objInvLst));
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "MEMO_UPLOAD")
                {
                    return GetLotNosFromDataTable(ds);
                }
                //else if (InventoryuploadType == "MEMO_RETURN_SALE")
                //{
                //    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);
                //    List<string> LotNos = objLst.Select(x => x.Stock).ToList();
                //    int[] OrderID = objMemoService.GetOrderIDFromLotNos(string.Join(",", LotNos));
                //    if (OrderID.Length == 0)
                //    {
                //        throw new UserDefinedException("No memo created against these lot nos.");
                //    }
                //    //else if (OrderID.Length > 1)
                //    //{
                //    //    throw new UserDefinedException("Cannot cancel item from multiple memos");
                //    //}
                //    else
                //    {
                //        string OrderIDs = string.Join(",", OrderID.Select(x => Convert.ToString(x)).ToArray());
                //        int rowCount = objMemoService.MemoReturnSale(OrderIDs, LoginID);
                //    }
                //    return objLst;
                //}
                else if (InventoryuploadType == "MEMO_CANCEL")
                {
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);//
                    List<string> LotNos = objLst.Select(x => x.Stock).ToList();
                    int[] OrderID = objMemoService.GetOrderIDFromLotNos(string.Join(",", LotNos));
                    if (OrderID.Length == 0)
                    {
                        throw new UserDefinedException("No memo created against these lot nos.");
                    }
                    else if (OrderID.Length > 1)
                    {
                        throw new UserDefinedException("Cannot cancel item from multiple memos");
                    }
                    else
                    {
                        //OrderID

                        MemoDetail objMd = objMemoService.CancelPartialMemo(OrderID[0], string.Join(",", LotNos), LoginID);
                        //for (int i = 0; i < objLst.Count; i++)
                        //{
                        //    objLst[i].LotStatus = rowCount > 0 ? "Valid" : "InValid";
                        //}
                        objLst = objMd.Inv;
                    }
                    return objLst;
                }
                else if (InventoryuploadType == "SPLIT_MEMO")
                {
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);//
                    List<string> LotNos = objLst.Select(x => x.Stock).ToList();
                    int[] OrderID = objMemoService.GetOrderIDFromLotNos(string.Join(",", LotNos));

                    if (OrderID.Length == 0)
                    {
                        throw new UserDefinedException("No memo created against these lot nos.");
                    }
                    else if (OrderID.Length > 1)
                    {
                        throw new UserDefinedException("Cannot split item from multiple memos");
                    }
                    for (int i = 0; i < objLst.Count; i++)
                    {
                        objLst[i].OrderID = OrderID[0];
                    }
                    return objLst;
                }
                else if (InventoryuploadType == "ENABLE_INV")
                {
                    procName = "proc_EnableDisableInventory";
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, "ENABLE");
                }
                else if (InventoryuploadType == "DISABLE_INV")
                {
                    procName = "proc_EnableDisableInventory";
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), Request.UserHostAddress, "DISABLE");
                }
                else if (InventoryuploadType == "BD_ADD")
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (ds.Rows[i]["inventoryID"].ToString().Trim() == "")
                        {
                            ds.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    RowCount = objStockDetailsService.BestDeals(ds, "", 0, "Via File Upload", fileId, GetToken(), LoginID, "Add_bestdeal_upload");
                    if (RowCount > 0)
                    {
                        return new List<InventoryUpload>();
                    }
                    else
                    {
                        throw new UserDefinedException("Best deal not added");
                    }
                }
                else if (InventoryuploadType == "BD_REMOVE")
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (ds.Rows[i]["inventoryID"].ToString() == "")
                        {
                            ds.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    RowCount = objStockDetailsService.BestDeals(ds, "", 0, "", fileId, GetToken(), LoginID, "Remove_bestdeal_upload");
                    if (RowCount > 0)
                    {
                        return new List<InventoryUpload>();
                    }
                    else
                    {
                        throw new UserDefinedException("Best deal not removed");
                    }
                }
                else if (InventoryuploadType == "ADD_LAB")
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (ds.Rows[i][0].ToString().Trim() == "")
                        {
                            ds.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int j = 1; j < ds.Columns.Count; j++)
                    {
                        ds.Columns.RemoveAt(j);
                        j--;
                    }
                    RowCount = objStockDetailsService.AddRemoveLabStatus(ds, fileId, "add");
                    if (RowCount > 0)
                    {
                        return new List<InventoryUpload>();
                    }
                    else
                    {
                        throw new UserDefinedException("Lab not set");
                    }
                }
                else if (InventoryuploadType == "REMOVE_LAB")
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (ds.Rows[i][0].ToString().Trim() == "")
                        {
                            ds.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int j = 1; j < ds.Columns.Count; j++)
                    {
                        ds.Columns.RemoveAt(j);
                        j--;
                    }
                    RowCount = objStockDetailsService.AddRemoveLabStatus(ds, fileId, "remove");
                    if (RowCount > 0)
                    {
                        return new List<InventoryUpload>();
                    }
                    else
                    {
                        throw new UserDefinedException("Lab not set");
                    }
                }
                else if (InventoryuploadType == "V360VIACERTNO")
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (ds.Rows[i][1].ToString().Trim() == "")
                        {
                            ds.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    return objStockDetailsService.UpdateV360ViaCertNo(ds, fileId);
                }
                else if (InventoryuploadType == "MEMO_RETURN_SALE")
                {
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);//
                    List<string> LotNos = objLst.Select(x => x.Stock).Where(x => !string.IsNullOrEmpty(x)).ToList();
                    //for (int i = 0; i < objLst.Count; i++)
                    //{
                    //    if (objLst[i].Stock.ToString().Trim() == "")
                    //    {
                    //        ds.Rows.RemoveAt(i);
                    //        i--;
                    //    }
                    //}
                    RowCount = objMemoService.MemoPartialReturnSale(string.Join(",", LotNos), LoginID);
                    return objLst;
                }

                //Before inventory Upload process fetch old stock details for email

                //After inventory Upload process fetch old stock details for email

            }
            return null;
        }

        private void SendMailOnUploadEvent(string EventName, string ValidFileName, string InValidFileName, string BackUpFile, int ValidCount, int InValidCount, int FileID)
        {
            bool SentMail = true;
            string Subject = string.Format("List of inventory \"{0}\" from - Rosyblueonline.com", EventName);
            string Message = "<p>Please find the attached file to view stones updated with {0}.</p><p>No. of stones: {1}</p> ";
            switch (EventName)
            {
                case "INVENTORY_UPLOAD":
                case "ENABLE_INV":
                case "INVENTORY_MODIFY":
                case "BD_ADD":
                case "BD_REMOVE":
                case "DISABLE_INV":
                case "V360VIACERTNO":
                case "ADD_LAB":
                case "REMOVE_LAB":
                case "MEMO_RETURN_SALE":
                case "JA_BN":
                case "MEMO_UPLOAD":
                case "MEMO_CANCEL":
                    Message = string.Format(Message, EventName, ValidCount);
                    if (SentMail)
                    {
                        objStockDetailsService.SendUploadEventMail(Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_EventSendMail"]), GetEmailID(), GetFullName(), Subject, Message, ValidFileName, InValidFileName, ConfigurationManager.AppSettings["CCemail"].ToString());
                    }
                    break;
                case "CHANGE_DISCOUNT":
                case "CHANGE_RAPP":
                    if (SentMail)
                    {
                        objStockDetailsService.SendUploadEventMail2(Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_EventInventoryChangeSendMail"]), GetEmailID(), GetFullName(), Subject, EventName, BackUpFile, FileID, ConfigurationManager.AppSettings["CCemail"].ToString());
                    }
                    break;
                default:
                    Message = "";
                    SentMail = false;
                    break;
            }



        }

        private DataTable ParseToString(DataTable oldDT)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < oldDT.Columns.Count; i++)
            {
                if (i == 1 || i == 2)
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName, typeof(string)));
                }
                else if (i == 43)
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName, typeof(DateTime)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName));
                }
            }

            for (int i = 0; i < oldDT.Rows.Count; i++)
            {

                DataRow dr = dt.NewRow();
                for (int j = 0; j < oldDT.Columns.Count; j++)
                {
                    if (Convert.ToString(oldDT.Rows[i][j]).Trim() != "")
                    {
                        if (j == 1 || j == 2)
                        {
                            dr[j] = Convert.ToString(oldDT.Rows[i][j]);
                        }
                        else if (j == 43)
                        {
                            dr[j] = Convert.ToString(oldDT.Rows[i][j]).Trim() == "" ? "" : Convert.ToDateTime(oldDT.Rows[i][j]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            dr[j] = oldDT.Rows[i][j];
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private DataTable RemoveBlankRows(DataTable oldDT)
        {
            for (int i = 0; i < oldDT.Rows.Count; i++)
            {
                if (Convert.ToString(oldDT.Rows[i][0]) == string.Empty)
                {
                    oldDT.Rows.RemoveAt(i);
                    i--;
                }
            }


            return oldDT;
        }

        public List<InventoryUpload> GetLotNosFromDataTable(DataTable dt)
        {
            List<InventoryUpload> lst = new List<InventoryUpload>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Stock"].ToString().ToLower() != "yes" && dt.Rows[i]["Stock"].ToString().ToLower() != "no")
                {
                    lst.Add(new InventoryUpload { Stock = dt.Rows[i]["Stock"].ToString() });
                }
            }
            return lst;
        }

        #endregion

        [HttpPost]
        public ActionResult GetInventory(string LotIDs)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    CompareInventoryViewModel objCmp = objStockDetailsService.CompareInventory(LoginID, LotIDs);
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Compare stock", LotIDs);

                    return Json(new Response { IsSuccess = true, Message = "", Result = objCmp });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetInventory", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        //[HttpPost]
        //public ActionResult GetInventoryForGrid(string LotIDs)
        //{
        //    try
        //    {
        //        int LoginID = GetLogin();
        //        if (LoginID > 0)
        //        {
        //            CompareInventoryViewModel objCmp = objStockDetailsService.CompareInventory(LoginID, LotIDs);
        //            return Json(new Response { IsSuccess = true, Message = "", Result = objCmp });
        //        }
        //        return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.Log("InventoryController", "GetInventory", ex);
        //        return Json(new Response { IsSuccess = false, Message = ex.Message });
        //    }
        //}

        public ActionResult GetInventoriesByLotID(string LotIDs)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> objLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotIDs);
                    return Json(new Response { IsSuccess = true, Message = "", Result = objLst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetInventoriesByLotID", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        //[DeleteFile] //Action Filter, it will auto delete the file after download,
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = ConfigurationManager.AppSettings["InventoryEmailAttachment"];
            fullPath = fullPath + file;

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            //Added New Comment
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        public ActionResult Upload()
        {
            List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
            objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
            return View(objVM);
        }

        public ActionResult RfidUpload()
        {
            List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
            objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
            return View(objVM);
        }

        public ActionResult UploadHistory()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadHistoryForGrid(DataTableViewModel objReq, string Type = "Normal")
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<FileUploadLogViewModel> objResp = new DataTableResponse<FileUploadLogViewModel>();
                    IQueryable<FileUploadLogViewModel> query = this.objStockDetailsService.QueryableFileUploadLog();
                    if (Type == "Normal")
                    {
                        query = query.Where(x => x.uploadType == 1 || x.uploadType == 6 || x.uploadType == 4 || x.uploadType == 5 || x.uploadType == 15);
                    }
                    else
                    {
                        query = query.Where(x => x.uploadType == 12);
                    }
                    //query = query.Where(x => x.invalidInv != 0 && x.validInv != 0);
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "uploadFullName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.uploadFullName);
                                else
                                    query = query.OrderByDescending(x => x.uploadFullName);
                                break;
                            case "validInv":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.validInv);
                                else
                                    query = query.OrderByDescending(x => x.validInv);
                                break;
                            case "invalidInv":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.invalidInv);
                                else
                                    query = query.OrderByDescending(x => x.invalidInv);
                                break;
                            case "fileId":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.fileId);
                                else
                                    query = query.OrderByDescending(x => x.fileId);
                                break;

                            default:
                            case "createdOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.createdOn);
                                else
                                    query = query.OrderByDescending(x => x.createdOn);
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
                ErrorLog.Log("InventoryController", "UploadHistoryForGrid", ex);
                throw;
            }
        }

        public ActionResult StoneStatus()
        {
            return View();
        }

        public ActionResult UploadStoneStatus()
        {
            if (Request.Files.Count > 0)
            {
                string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                string extension = Path.GetExtension(Request.Files[0].FileName);
                Guid guid = Guid.NewGuid();
                string FileName = guid.ToString() + extension;
                Request.Files[0].SaveAs(path + "/" + FileName);
                return Json(new Response { IsSuccess = true, Message = "", Result = FileName });
            }
            return Json(new Response { IsSuccess = false, Message = "No file uploaded" });
        }

        public ActionResult GetStoneStatus(DataTableViewModel obj, string LotNos, string FileName, string Type)
        {
            try
            {
                CommonFunction com = new CommonFunction();
                DataTable dt = new DataTable();
                dt.Columns.Add("Lotnumber");
                int LoginID = GetLogin();
                string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                string OrderBy = "", OrderDirection = "";
                if (LoginID > 0)
                {
                    if (obj.order.Count > 0)
                    {
                        OrderBy = obj.columns[obj.order[0].column].data;
                        OrderDirection = obj.order[0].dir;
                    }
                    if (!string.IsNullOrEmpty(LotNos) && string.IsNullOrEmpty(FileName))
                    {
                        for (int i = 0; i < LotNos.Split(',').Length; i++)
                        {
                            dt.Rows.Add(LotNos.Split(',')[i]);
                        }
                    }
                    else if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(LotNos))
                    {
                        dt = com.GetDataFromExcel2(path + "/" + FileName, false);
                    }
                    decimal idx = Math.Ceiling((decimal)(obj.start / obj.length));
                    StockStatusViewModel objSS = objStockDetailsService.StockStatus(dt, LoginID, idx, obj.length, Type, OrderBy == "" ? "LotNumber" : OrderBy, OrderDirection == "" ? "asc" : OrderDirection);
                    if (objSS != null)
                    {
                        return Json(new { draw = obj.draw, recordsTotal = objSS.TotalCount, recordsFiltered = objSS.TotalCount, data = objSS.inventories, OrderBy = OrderBy });
                    }
                    else
                    {
                        return Json(new Response { IsSuccess = false, Message = "No data found" });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetStoneStatus", ex);
                return Json(new { draw = obj.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<string>() });
            }
        }

        public ActionResult ExportToExcelStoneStatus(string LotNos, string FileName, string Type)
        {
            try
            {
                CommonFunction com = new CommonFunction();
                DataTable dt = new DataTable();
                dt.Columns.Add("Lotnumber");
                int LoginID = GetLogin();
                string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                string OrderBy = "", OrderDirection = "";
                if (LoginID > 0)
                {
                    if (!string.IsNullOrEmpty(LotNos) && string.IsNullOrEmpty(FileName))
                    {
                        for (int i = 0; i < LotNos.Split(',').Length; i++)
                        {
                            dt.Rows.Add(LotNos.Split(',')[i]);
                        }
                    }
                    else if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(LotNos))
                    {
                        dt = com.GetDataFromExcel2(path + "/" + FileName, false);
                    }
                    DataSet ds = objStockDetailsService.StockStatus(dt, LoginID, Type, OrderBy == "" ? "LotNumber" : OrderBy, OrderDirection == "" ? "asc" : OrderDirection);
                    if (ds.Tables.Count == 2)
                    {
                        ds.Tables[1].Columns.Remove("RowNum");
                        //ds.Tables[1].Columns.Remove("inventoryID");
                        //ds.Tables[1].Columns["refData"].SetOrdinal(1);
                        ds.Tables[1].Columns["refData"].ColumnName = "Status";
                        TempData["SpecificSearchExport"] = ExportToExcel.StoneStatusExportToExcel(ds.Tables[1]);
                    }


                    return Json(new Response { IsSuccess = true });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "ExportToExcelStoneStatus", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult GIA()
        {
            return View();
        }

        public ActionResult GetGIADataFromExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    CommonFunction com = new CommonFunction();
                    string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    Guid guid = Guid.NewGuid();
                    string FileName = guid.ToString() + extension;
                    Request.Files[0].SaveAs(path + "/" + FileName);
                    DataTable dt = com.GetDataFromExcel2(path + "/" + FileName, true);
                    List<GIAUpload> objGiaLst = new List<GIAUpload>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objGiaLst.Add(new GIAUpload
                        {
                            Certificate = dt.Rows[i]["Certificate"].ToString(),
                            Lotnumber = dt.Rows[i]["Lotnumber"].ToString(),
                            Weight = dt.Rows[i]["Weight"].ToString(),
                        });
                    }
                    return Json(new Response { IsSuccess = true, Message = "", Result = objGiaLst });
                }
                return Json(new Response { IsSuccess = false, Message = "No file uploaded" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetGIADataFromExcel", ex);
                return Json(new Response { IsSuccess = false, Message = "Error" });
            }

        }

        [HttpPost]
        public ActionResult ExportToExcelInventory(string filterText, bool NewArrival = false, bool IsSpecialSearch = true, bool IsOnlyMemo = false)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int Role1 = GetRole();
                    string PermissibleDownload = "NO";
                    if (Role1 == 3)
                    {
                        PermissibleDownload = "YES";
                    }

                    CommonFunction objCom = new CommonFunction();
                    DataTable dt = objStockDetailsService.SpecificSearchDownloadExcelExport(filterText, NewArrival, LoginID, IsSpecialSearch, IsOnlyMemo,PermissibleDownload);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("RowNum");
                        //Guid fname = Guid.NewGuid();
                        string imgPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelMailImage"]);
                        int Role = GetRole();
                        byte[] st = ExportToExcel.InventoryExportToExcel(dt, imgPath, Role == 3 ? true : false,"", Role == 3 ? true : false);
                        TempData["SpecificSearchExport"] = st;
                        string bs64 = Convert.ToBase64String(st);
                        string json = JsonConvert.SerializeObject(new Response { IsSuccess = true, Result = bs64, Message = "InventoryExport.xlsx" }, Formatting.Indented);
                        return Content(json, "application/json");
                    }
                    else
                    {
                        return Json(new Response { IsSuccess = false, Message = "No rows found" });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "ExportToExcelInventory", ex);
                return new EmptyResult();
            }
        }

        //[Route("/Inventory/ExportToExcelInventory/Download")]
        public ActionResult ExportToExcelInventoryDownload(string filename = "SpecificSearchExport")
        {
            try
            {
                byte[] st = (byte[])TempData["SpecificSearchExport"];
                return File(st, System.Net.Mime.MediaTypeNames.Application.Octet, filename + ".xlsx");
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "ExportToExcelInventoryDownload", ex);
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult InventorySendMail(SendInventoryRequestModel objInv)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    CommonFunction objCom = new CommonFunction();
                    string imgPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelMailImage"]);
                    string TemplatePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_SendMail"]);

                    objStockDetailsService.SendInventoryViaMail(objInv, TemplatePath, imgPath, LoginID);
                    return Json(new Response { IsSuccess = true, Result = "", Message = "" });

                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "InventorySendMail", ex);
                return new EmptyResult();
            }
        }

        public ActionResult PriceMarkUpDiscount()
        {
            return View();
        }

        public ActionResult AddBestDealViaForm(string InventoryIDs, decimal Discount, string Remark)
        {
            try
            {
                int LoginID = GetLogin();
                string Token = GetToken();
                int RowCount = 0;
                if (LoginID > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("inventoryID"); dt.Columns.Add("discountPercentBD");
                    RowCount = this.objStockDetailsService.BestDeals(dt, InventoryIDs, Discount, Remark, 0, Token, LoginID, "Add_bestdeal_search");
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddBestDealViaForm", ex);
                return Json(new Response { IsSuccess = false, Message = "Error" });
            }
        }

        public ActionResult RemoveBestDealViaForm(string InventoryIDs)
        {
            try
            {
                int LoginID = GetLogin();
                string Token = GetToken();
                int RowCount = 0;
                if (LoginID > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("InventoryID"); dt.Columns.Add("DiscountPercentBD");
                    RowCount = this.objStockDetailsService.BestDeals(dt, InventoryIDs, 0, "", 0, Token, LoginID, "Remove_bestdeal_search");
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "AddBestDealViaForm", ex);
                return Json(new Response { IsSuccess = false, Message = "Error" });
            }
        }

        public ActionResult SendEmail(string FromEmailID, string Subject, string Message, string LotNo)
        {
            try
            {
                if (objMU == null)
                {
                    objMU = new MailUtility();
                }
                int LoginID = GetLogin();
                string AdminEmailID = ConfigurationManager.AppSettings["InspectEmail_ID"].ToString();
                List<string> lstOfEmailIDs = new List<string>();
                List<inventoryDetailsViewModel> objLst = new List<inventoryDetailsViewModel>();
                if (LoginID > 0)
                {
                    objLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotNo);
                }
                TokenLogModel objToken = (TokenLogModel)Session["Token"];
                if (objToken != null)
                {
                    //objToken.EmailID;
                    lstOfEmailIDs.Add(FromEmailID.ToString());
                    lstOfEmailIDs.Add(AdminEmailID.ToString());
                    DataTable dt = ListtoDataTable.ToDataTable<inventoryDetailsViewModel>(objLst);
                    string tblString = CommonFunction.ConvertDataTableToHTML(dt);
                    objMU.SendMail(lstOfEmailIDs, Subject, true, Message + "</br>" + tblString);
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Inventory", "SendEmail->InspectionReport", ex);
                return Json(false);
            }

        }

        public ActionResult Downloads()
        {
            int LoginID = GetLogin();
            int RoleID = GetRole();
            List<DownloadList> objLst = this.objDownloadService.GetDownloadForMenu(LoginID, RoleID);
            return View(objLst);
        }

        //public ActionResult DownloadDynamicDownload(string IDs, string FileName, string SheetName)
        //{
        //    int LoginID = GetLogin();
        //    int RoleID = GetRole();
        //    DataSet ds = new DataSet();
        //    ds = this.objDownloadService.ExecuteDownload(IDs);
        //    if (ds.Tables.Count > 0)
        //    {
        //        byte[] file = ExportToExcel.DownloadExcel(SheetName, ds);
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

        //        using (MemoryStream MyMemoryStream = new MemoryStream(file))
        //        {
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //        //using (XLWorkbook wb = new XLWorkbook())
        //        //{
        //        //    wb.Worksheets.Add(ds);
        //        //    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //    wb.Style.Font.Bold = true;

        //        //    Response.Clear();
        //        //    Response.Buffer = true;
        //        //    Response.Charset = "";
        //        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //    Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

        //        //    using (MemoryStream MyMemoryStream = new MemoryStream())
        //        //    {
        //        //        wb.SaveAs(MyMemoryStream);
        //        //        MyMemoryStream.WriteTo(Response.OutputStream);
        //        //        Response.Flush();
        //        //        Response.End();
        //        //    }
        //        //}
        //        //System.IO.StringWriter tw = new System.IO.StringWriter();
        //        //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //        //GridView grdResultDetails = new GridView();
        //        //grdResultDetails.DataSource = ds;
        //        //grdResultDetails.DataBind();
        //        //grdResultDetails.HeaderRow.Style.Add("background-color", "#fff");
        //        //for (int i = 0; i <= grdResultDetails.HeaderRow.Cells.Count - 1; i++)
        //        //{
        //        //    grdResultDetails.HeaderRow.Cells[i].Style.Add("background-color", "#9a9a9a");
        //        //}
        //        //grdResultDetails.RenderControl(hw);
        //        //Response.ContentType = "application/vnd.ms-excel";
        //        //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
        //        //Response.Write(tw.ToString());
        //        //Response.End();
        //        return RedirectToAction("Downloads");
        //        //return Content(tw.ToString(), "application/vnd.ms-excel");
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(204, "No Data");
        //    }

        //}
        public ActionResult DownloadDynamicDownload(string IDs, string FileName, string SheetName)
        {
            int LoginID = GetLogin();
            int RoleID = GetRole();
            DataSet ds = new DataSet();
            ds = this.objDownloadService.ExecuteDownload(IDs);
            if (ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = SheetName;
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                GridView grdResultDetails = new GridView();
                grdResultDetails.DataSource = ds;
                grdResultDetails.DataBind();
                grdResultDetails.HeaderRow.Style.Add("background-color", "#fff");
                for (int i = 0; i <= grdResultDetails.HeaderRow.Cells.Count - 1; i++)
                {
                    grdResultDetails.HeaderRow.Cells[i].Style.Add("background-color", "#9a9a9a");
                }
                grdResultDetails.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
                Response.Write(tw.ToString());
                Response.End();
                return RedirectToAction("Downloads");
                //return Content(tw.ToString(), "application/vnd.ms-excel");
            }
            else
            {
                return new HttpStatusCodeResult(204, "No Data");
            }

        }

        private void ExportToExcelViaDS(DataSet datasources, string filename)
        {
            try
            {
                if (datasources.Tables[0].Rows.Count > 0)
                {

                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    GridView grdResultDetails = new GridView();
                    grdResultDetails.DataSource = datasources;
                    grdResultDetails.DataBind();
                    grdResultDetails.HeaderRow.Style.Add("background-color", "#fff");
                    for (int i = 0; i <= grdResultDetails.HeaderRow.Cells.Count - 1; i++)
                    {
                        grdResultDetails.HeaderRow.Cells[i].Style.Add("background-color", "#9a9a9a");
                    }

                    grdResultDetails.RenderControl(hw);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex) { }
            finally { }
        }

        public ActionResult StoneHistory()
        {
            return View();
        }

        public ActionResult GetStoneHistory(DataTableViewModel objReq, string StoneIDs)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                DataTableResponse<StockHistoryViewModel> objResp = new DataTableResponse<StockHistoryViewModel>();
                if (objReq != null && !string.IsNullOrEmpty(StoneIDs))
                {
                    List<string> StoneID = StoneIDs.Split(',').ToList();
                    IQueryable<StockHistoryViewModel> query = this.objStockDetailsService.QueryableStockHistory();
                    query = query.Where(x => StoneID.Contains(x.Lotnumber));
                    //query = OType == "Order" ? query.Where(x => x.orderType == 14) : query.Where(x => x.orderType == 15 || x.orderType == 155);
                    //Role ID 3 for admin
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "Lotnumber":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.Lotnumber);
                                else
                                    query = query.OrderByDescending(x => x.Lotnumber);
                                break;

                            default:
                            case "orderCreatedOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderByDescending(x => x.CreatedOn);
                                else
                                    query = query.OrderBy(x => x.CreatedOn);
                                break;
                        }
                    }
                    objResp.draw = objReq.draw;
                    objResp.recordsFiltered = query.Count();
                    objResp.data = query.Skip(objReq.start).Take(objReq.length).ToList();
                    return Json(objResp);
                }
                objResp.recordsTotal = 0;
                objResp.draw = 0;
                objResp.recordsFiltered = 0;
                objResp.data = new List<StockHistoryViewModel>();
                return Json(objResp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetSalesLocation()
        {
            try
            {
                List<MstSalesLocationModel> objLst = this.objStockDetailsService.SalesLocationActive();
                return Json(objLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetSalesLocation", ex);
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult GetAvgRapOff(string LotNos)
        {
            try
            {
                decimal AvgRapOff = this.objStockDetailsService.GetAvgRapOff(LotNos);
                return Json(new Response { IsSuccess = true, Message = "", Result = AvgRapOff });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetAvgRapOff", ex);
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }

        }

        [HttpGet]
        public ActionResult MemoInfoDetails(int inventoryID)
        {

            orderDetailModel objO = new orderDetailModel();


          //  objO = this.objMemoService.orderItemDetailsfrominvintory(inventoryID);
            return RedirectToAction("MemoDetails", new RouteValueDictionary(new { Controller = "Memo", action = "MemoDetails", OrderID = objO.customerId }));

            //orderItemDetailModel objO = new orderItemDetailModel();
            //objO = this.objMemoService.orderItemDetailsfrominvintory(inventoryID);
            // return RedirectToAction("MemoDetails", new RouteValueDictionary(new { Controller = "Memo", action = "MemoDetails", OrderID = objO.orderDetailsId }));

        }


        [HttpPost]
        public ActionResult GetOrderDtl(int ID)
        {

            CustOrderDetailModel objO = new CustOrderDetailModel();
            objO = this.objMemoService.orderItemDetailsfrominvintory(ID);
            return Json(new Response { IsSuccess = true, Message = "", Result = objO });


        }



        [HttpGet]
        public ActionResult GetSizePermision()
        {
            try
            {
                MstCustomerPermisionModel objLst = new MstCustomerPermisionModel();
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                      objLst = this.objStockDetailsService.GetSizePermision(LoginID);
                }
                return Json(objLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "GetSizePermision", ex);
                throw ex;
            }

        }

        //[HttpGet]
        //public ActionResult OrderInfoDetails(int inventoryID)
        //{
        //    orderItemDetailModel objO = new orderItemDetailModel();
        //    objO= this.objMemoService.orderItemDetailsfrominvintory(inventoryID); 
        //    return RedirectToAction("Info", new RouteValueDictionary(new { Controller = "Order", action = "Info", OrderID = objO.orderDetailsId }));
        //}


        //[HttpPost]
        //public JsonResult GetMemoDetailsInventoryid(int Inventoryid)
        //{
        //    try
        //    {
        //        orderItemDetailModel objO = new orderItemDetailModel();
        //        objO = this.objMemoService.orderItemDetailsfrominvintory(Inventoryid);
        //        return Json(new Response { IsSuccess = true, Message = "Memo Order Details", Result = objO });

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.Log("InventoryController", "Details", ex);
        //        return Json(new Response { IsSuccess = false, Message = ex.Message });
        //    }

        //}


    }
}