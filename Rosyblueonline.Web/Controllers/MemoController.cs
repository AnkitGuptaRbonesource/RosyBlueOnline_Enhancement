using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class MemoController : _BaseController
    {
        IStockDetailsService objStockDetailsService;
        IMemoService objMemoService;
        private readonly UserDetailService objUDSvc = null;
        public MemoController(IStockDetailsService objStockDetailsService, IMemoService objMemoService, IUserDetailService objUDSvc)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objMemoService = objMemoService as MemoService;
            this.objUDSvc = objUDSvc as UserDetailService;
        }
        // GET: Memo
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.Cookies["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
            }
        }
        [CustomAuthorize("AllMemo")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book(string LotNos, int CustomerID, int isConfirmed, int isSellDirect, string Remark, int FileNo = 0)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    MemoDetail obj = this.objMemoService.CreateMemo(LotNos, LoginID, CustomerID, isConfirmed, isSellDirect, Remark);
                    if (FileNo != 0)
                    {
                        fileUploadLogModel objFile = this.objMemoService.GetFileByID(FileNo);
                        if (objFile != null)
                        {
                            String path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["INVUpload"]);
                            DataTable dtValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "Valid").ToList());
                            DataTable dtNotValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "InValid").ToList());
                            ExportToExcel.SaveExcel(path, FileNo.ToString() + "_Valid", "Valid", dtValid);
                            ExportToExcel.SaveExcel(path, FileNo.ToString() + "_InValid", "InValid", dtNotValid);
                            objFile.validInv = obj.Inv.Where(x => x.LotStatus == "Valid").Count();
                            objFile.invalidInv = obj.Inv.Where(x => x.LotStatus == "InValid").Count();
                            this.objMemoService.UpdateFile(objFile);
                            if (obj.Counts.OrderId > 0)
                            {
                                this.objMemoService.UpdateFile(obj.Counts.OrderId, FileNo);
                            }
                        }
                    }
                    if (obj != null && obj.Counts.OrderId != 0)
                    {
                        this.objMemoService.SendMailMemo(obj.Counts.OrderId, GetEmailID(), GetFullName(), CustomerID, "List of inventory put on memo to", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_PutOnMemo"]));
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Put on memo", LotNos);


                    }
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = obj.Counts });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "Book", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult PartialCancel(int OrderID, string LotNos, int FileNo = 0)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    OrderInfoViewModel objInfo = this.objMemoService.MemoInfo(OrderID);
                    MemoDetail obj = this.objMemoService.CancelPartialMemo(OrderID, LotNos, LoginID);
                    if (FileNo != 0)
                    {
                        fileUploadLogModel objFile = this.objMemoService.GetFileByID(FileNo);
                        if (objFile != null)
                        {
                            String path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["INVUpload"]);
                            DataTable dtValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "Valid").ToList());
                            DataTable dtNotValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "InValid").ToList());
                            ExportToExcel.SaveExcel(path, FileNo.ToString() + "_Valid", "Valid", dtValid);
                            ExportToExcel.SaveExcel(path, FileNo.ToString() + "_InValid", "InValid", dtNotValid);
                            objFile.validInv = obj.Inv.Where(x => x.LotStatus == "Valid").Count();
                            objFile.invalidInv = obj.Inv.Where(x => x.LotStatus == "InValid").Count();
                            this.objMemoService.UpdateFile(objFile);
                            if (obj.Counts.OrderId > 0)
                            {
                                this.objMemoService.UpdateFile(obj.Counts.OrderId, FileNo);
                            }
                        }
                    }
                    if (objInfo != null && obj.Counts != null && obj.Counts.OrderId > 0)
                    {
                        this.objMemoService.SendMailMemo(obj.Counts.OrderId, GetEmailID(), GetFullName(), objInfo.UserDetail.loginID, "List of inventory Partial Canceled Memo from memo-", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_CancelMemo"]), objInfo, LotNos);
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Partial canceled memo", LotNos);
                    }
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = obj.Counts.validCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "PartialCancel", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SplitMemo(int OrderID, string LotNos, int CustomerID, int isConfirmed, int isSellDirect, string Remark, int FileNo = 0)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    OrderInfoViewModel objInfo = this.objMemoService.MemoInfo(OrderID);
                    int RowCount = this.objMemoService.SplitMemo(OrderID, LotNos, LoginID, CustomerID, isConfirmed, isSellDirect, Remark);
                    if (FileNo != 0)
                    {
                        fileUploadLogModel objFile = this.objMemoService.GetFileByID(FileNo);
                        objFile.validInv = LotNos.Split(',').Length;
                        objFile.invalidInv = 0;
                        this.objMemoService.UpdateFile(objFile);
                    }
                    if (objInfo != null && RowCount > 0)
                    {
                        this.objMemoService.SendMailMemo(RowCount, GetEmailID(), GetFullName(), objInfo.UserDetail.loginID, "List of inventory SplitMemo from memo-", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_SplitMemo"]));
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Split memo", LotNos);

                    }
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "PartialCancel", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult PartialSellMemo(int OrderID, string LotNos, int MemoMode, decimal salesAvgDiscount)
        {
            try
            {
                int RowCount = 0;
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    OrderInfoViewModel objOVM = this.objMemoService.MemoInfo(OrderID);
                    if (objOVM != null && objOVM.OrderDetail != null)
                    {
                        if (LotNos.Split(',').Count() == 0)
                        {
                            throw new Exception("No items selected");
                        }
                        else if (objOVM.OrderItemDetail.Count == LotNos.Split(',').Count())
                        {
                            RowCount = this.objMemoService.SellFullMemo(OrderID, LoginID, MemoMode, salesAvgDiscount);
                        }
                        else
                        {
                            int newOrderID = this.objMemoService.SplitMemo(OrderID, LotNos, LoginID, objOVM.OrderDetail.customerId, 0, 0, "Partial Sell");
                            if (newOrderID > 0)
                            {
                                RowCount = this.objMemoService.SellFullMemo(newOrderID, LoginID, MemoMode, salesAvgDiscount);
                                OrderID = newOrderID;
                            }

                        }

                        if (objOVM != null && RowCount > 0) //Added By Ankit 08Jun2020--Suggested By LK
                        {
                            this.objMemoService.SendMailMemo(OrderID, GetEmailID(), GetFullName(), LoginID, "List of inventory sell partial memo to", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_SellMemo"]), null, null, MemoMode, salesAvgDiscount);

                           // this.objMemoService.SendMailMemo(OrderID, GetEmailID(), GetFullName(), LoginID, "List of inventory sell partial memo to", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_SellMemo"]));
                            bool log = this.objUDSvc.UserActivitylogs(LoginID, "partial sell memo", LotNos);
                        }
                    }

                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "PartialSellMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SellFullMemo(int OrderID, int MemoMode, decimal salesAvgDiscount)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objMemoService.SellFullMemo(OrderID, LoginID, MemoMode, salesAvgDiscount);
                    if (RowCount > 0) //Added By Ankit 08Jun2020--Suggested By LK
                    {
                        this.objMemoService.SendMailMemo(OrderID, GetEmailID(), GetFullName(), LoginID, "List of inventory sell full memo to", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_SellMemo"]), null, null, MemoMode, salesAvgDiscount);
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Sell full memo", OrderID.ToString());

                    }
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "PartialCancel", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult CancelFullMemo(int OrderID)
        {
            try
            {
                int RowCount = 0;
                OrderInfoViewModel objInfo = this.objMemoService.MemoInfo(OrderID);
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    RowCount = this.objMemoService.CancelFullMemo(OrderID, LoginID);
                    if (objInfo != null && RowCount > 0)
                    {
                        this.objMemoService.SendMailMemo(OrderID, GetEmailID(), GetFullName(), objInfo.UserDetail.loginID, "List of inventory CancelMemo from memo-", "", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate_CancelMemo"]), objInfo);
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Cancel full memo", OrderID.ToString());
                    }
                }

                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "PartialCancel", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult MergeMemo(int CustomerID, int isConfirmed, int isSellDirect, string Remark, string MergeOrderList)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objMemoService.MergeMemo(LoginID, CustomerID, isConfirmed, isSellDirect, Remark, MergeOrderList);
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "CancelFullMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult UpdateMemo(int OrderID, int CustomerID, string Remark)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objMemoService.UpdateOrder(OrderID, CustomerID, Remark, LoginID);
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Update memo", OrderID.ToString());


                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "UpdateMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ReturnMemo(string OrderIDs)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objMemoService.MemoReturnSale(OrderIDs, LoginID);
                    if (RowCount > 0)
                    {
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Return memo", OrderIDs);

                        return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "ReturnMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ReturnPartailMemo(string LotNos)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objMemoService.MemoPartialReturnSale(LotNos, LoginID);
                    if (RowCount > 0)
                    {
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Return Partial memo", LotNos);

                        return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
                    }
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "ReturnMemo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }


        [Route("Memo/Filter/{OrderID}")]
        public ViewResult MemoDetails()
        {
            return View("Index");
        }


        //[HttpGet]
        //public ActionResult GetOrderDtl(int ID)
        //{

        //    orderDetailModel objO = new orderDetailModel(); 
        //    objO = this.objMemoService.orderItemDetailsfrominvintory(inventoryID);
        //    return RedirectToAction("MemoDetails", new RouteValueDictionary(new { Controller = "Memo", action = "MemoDetails", OrderID = objO.customerId }));

        //}

        //public class MyViewModel {

        //         public bool NeedsToRunJs { get; set; }
        //}
        //[Route("Memo/{OrderID}")]
        //public ActionResult MemoDetails(int OrderID)
        //{
        //    var myViewModel = new MyViewModel();
        //    myViewModel.NeedsToRunJs = true; 
        //    return View("Index");
        //}

        //[HttpPost]
        //public ActionResult MemoPartialReturnSale(string LotNos)
        //{
        //    try
        //    {
        //        int LoginID = GetLogin();
        //        if (LoginID > 0)
        //        {
        //            int RowCount = this.objMemoService.MemoPartialReturnSale(LotNos, LoginID);
        //            if (RowCount > 0)
        //            {
        //                return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = RowCount });
        //            }
        //        }
        //        return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.Log("MemoController", "MemoPartialReturnSale", ex);
        //        return Json(new Response { IsSuccess = false, Message = ex.Message });
        //    }
        //}
    }
}