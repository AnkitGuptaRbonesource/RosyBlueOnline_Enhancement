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
    public class OrderController : _BaseController
    {
        IOrderService objOrderService;
        IStockDetailsService objStockDetailsService;

        IWS_SchedulerService objSchedulerService;
        private readonly UserDetailService objUDSvc = null;
        public OrderController(IOrderService objOrderService, IStockDetailsService objStockDetailsService, IWS_SchedulerService objSchedulerService, IUserDetailService objUDSvc)
        {
            this.objOrderService = objOrderService as OrderService;
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objSchedulerService = objSchedulerService as WS_SchedulerService;
            this.objUDSvc = objUDSvc as UserDetailService;
        }

        //// GET: Order
        //public ActionResult Index()
        //{
        //    return View();
        //}
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.Cookies["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
            }
        }
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PreBookOrder(string LotNos, int ShippingMode)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    string TokenId = GetToken();
                    OrderViewModel rst = objOrderService.PreBookOrder(LotNos, LoginID, ShippingMode);

                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Order Pre Book", LotNos);

                    return Json(new Response { IsSuccess = true, Message = "", Result = rst }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "PreBookOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BookOrder(string LotNos, int CustomerId, int ShippingMode, int BillingID, int ShippingID)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    if (CustomerId == 0)
                    {
                        CustomerId = LoginID;
                    }
                    PlaceOrderViewModel rst = objOrderService.BookOrder(LotNos, LoginID, CustomerId, ShippingMode, BillingID, ShippingID);
                    objOrderService.SendMailPreBookOrder(rst.OrderId, LoginID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderAdmin"]), "Customer order details @ www.rosyblueonline.com");
                    objOrderService.SendMailPreBookOrder(rst.OrderId, LoginID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderCustomer"]), "Your order details @ www.rosyblueonline.com", true);

                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Order Book", LotNos);

                    return Json(new Response { IsSuccess = true, Message = "", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "BookOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOrderItemsByOrderID(string OrderID)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> rst = objOrderService.GetOrderItemsByOrderID(OrderID, LoginID);
                    return Json(new Response { IsSuccess = true, Message = "", Result = rst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "GetOrderItemsByOrderID", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult OrderListing(DataTableViewModel objReq, string OType = "Order", string OStatus = "Pending", int FilterCustomerID = 0)
        {
            try
            {
 
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<OrderListView> objResp = new DataTableResponse<OrderListView>();
                    IQueryable<OrderListView> query = this.objOrderService.OrderListView();
                    query = OType == "Order" ? query.Where(x => x.orderType == 14) : query.Where(x => x.orderType == 15 || x.orderType == 155);
                    //query = OType == "Order" ? query.Where(x => x.orderType == 14) : query.Where(x => x.orderType == 15);
                    query = OStatus == "Pending" ? query.Where(x => x.orderStatus == 10) : query.Where(x => x.orderStatus == 11);
 

                    //Role ID 3 for admin
                    if (RoleID == 3)
                    {
                        query = query.Where(x => x.loginID == CustomerID);
                    }
                    if (FilterCustomerID != 0)
                    {
                        query = query.Where(x => x.loginID == FilterCustomerID);
                    }

                    //if (OrderId != 0)  //Added BY ANkit 03JUly2020
                    //{
                    //    query = query.Where(x => x.loginID == OrderId);
                    //}

                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "companyName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.companyName);
                                else
                                    query = query.OrderByDescending(x => x.companyName);
                                break;

                            default:
                            case "orderCreatedOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.orderCreatedOn);
                                else
                                    query = query.OrderByDescending(x => x.orderCreatedOn);
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
                ErrorLog.Log("OrderController", "OrderListing", ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult OrderListingDownloadExcel(string OType = "Order", string OStatus = "Pending", int FilterCustomerID = 0)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                DataTable dt = objOrderService.OrderDetailForDownload(OType, OStatus, FilterCustomerID);
                //DataSet ds12 = new DataSet();
                //ds12.Tables.Add(dt);
                if (dt.Rows.Count > 0)
                {
                    //using (XLWorkbook wb = new XLWorkbook())
                    //{
                    //    wb.Worksheets.Add(ds);
                    //    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //    wb.Style.Font.Bold = true;
                    //    Response.Clear();
                    //    Response.Buffer = true;
                    //    Response.Charset = "";
                    //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //    Response.AddHeader("content-disposition", "attachment;filename=" + OType + "_" + OStatus + ".xlsx");
                    //    using (MemoryStream MyMemoryStream = new MemoryStream())
                    //    {
                    //        wb.SaveAs(MyMemoryStream);
                    //        MyMemoryStream.WriteTo(Response.OutputStream);
                    //        Response.Flush();
                    //        Response.End();
                    //    }
                    //}
                    dt.TableName = OType + "_" + OStatus;
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    GridView grdResultDetails = new GridView();
                    grdResultDetails.DataSource = dt;
                    grdResultDetails.DataBind();
                    grdResultDetails.HeaderRow.Style.Add("background-color", "#fff");
                    for (int i = 0; i <= grdResultDetails.HeaderRow.Cells.Count - 1; i++)
                    {
                        grdResultDetails.HeaderRow.Cells[i].Style.Add("background-color", "#9a9a9a");
                    }
                    grdResultDetails.RenderControl(hw);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + OType + "_" + OStatus + ".xls");
                    Response.Write(tw.ToString());
                    Response.End();

                    return new HttpStatusCodeResult(204, "No Data");
                    //return Content(tw.ToString(), "application/vnd.ms-excel");
                }
                return new HttpStatusCodeResult(204, "No Data");
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "OrderListingDownloadExcel", ex);
                throw;
            }
        }

        [Route("Order/Info/{OrderID}")]
        public ActionResult Info(int OrderID)
        {
            return View(OrderID);
        }

        [HttpPost]
        public JsonResult GetInfo(int OrderID)
        {
            try
            {
                OrderInfoViewModel obj = this.objOrderService.OrderInfo(OrderID);
                return Json(new Response { IsSuccess = true, Message = "", Result = obj });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "GetInfo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetMultipleInfo(string OrderIDs)
        {
            try
            {
                List<int> lstOrderIDs = new List<int>();
                string[] OrderID = OrderIDs.Split(',');
                for (int i = 0; i < OrderID.Count(); i++)
                {
                    lstOrderIDs.Add(Convert.ToInt32(OrderID[i]));
                }
                var lstOLV = this.objOrderService.OrderListView().Where(x => lstOrderIDs.Contains(x.orderDetailsId)).ToList();
                return Json(new Response { IsSuccess = true, Message = "", Result = lstOLV });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "GetMultipleInfo", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult RemoveItemsFormOrder(int OrderID, string LotNos)
        {
            try
            {
                int AdminID = GetLogin();
                int RoleID = GetRole();
                int RowCount = 0;
                if (AdminID != 0 && (RoleID == 2 || RoleID == 8 || RoleID == 9))
                {
                    int CustomerID = this.objOrderService.OrderListView().Where(x => x.orderDetailsId == OrderID).Select(x => x.loginID).FirstOrDefault();
                    RowCount = this.objOrderService.OrderPartialCancel(OrderID, LotNos, AdminID, CustomerID);
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });

                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "RemoveItemsFormOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CancelOrder(int OrderID)
        {
            try
            {
                int AdminID = GetLogin();
                int RoleID = GetRole();
                int RowCount = 0;
                if (AdminID != 0 && (RoleID == 2 || RoleID == 8 || RoleID == 9))
                {
                    OrderInfoViewModel objinfo = this.objOrderService.OrderInfo(OrderID);
                    int CustomerID = this.objOrderService.OrderListView().Where(x => x.orderDetailsId == OrderID).Select(x => x.loginID).FirstOrDefault();
                    RowCount = this.objOrderService.OrderCancel(OrderID, AdminID, CustomerID);
                    if (RowCount > 0 && objinfo != null)
                    {
                        this.objOrderService.SendForOrder(objinfo, CustomerID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_CancelOrder"]), "", true);
                    }
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "CancelOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CompleteOrder(int OrderID, string ShippingCompany, string TrackingNumber)
        {
            try
            {
                int AdminID = GetLogin();
                int RoleID = GetRole();
                int RowCount = 0;
                if (AdminID != 0 && (RoleID == 2 || RoleID == 8 || RoleID == 9))
                {
                    OrderInfoViewModel objinfo = this.objOrderService.OrderInfo(OrderID);
                    int CustomerID = this.objOrderService.OrderListView().Where(x => x.orderDetailsId == OrderID).Select(x => x.loginID).FirstOrDefault();
                    //objOrderService.SendMailPreBookOrder(OrderID, CustomerID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_ConfirmOrder"]), true);
                    RowCount = this.objOrderService.OrderComplete(OrderID, AdminID, CustomerID, ShippingCompany, TrackingNumber);
                    if (RowCount > 0 && objinfo != null)
                    {
                        this.objOrderService.SendForOrder(objinfo, CustomerID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_ConfirmOrder"]), "", true);
                    }
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "CompleteOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }


        }

        [HttpPost]
        public JsonResult MergeOrder(string MergeOrderList)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID != 0)
                {
                    int RowCount = this.objOrderService.OrderMerge(LoginID, MergeOrderList);
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("OrderController", "MergeOrder", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }


        }

        public JsonResult AllowEditCustomer(int OrderLoginID)
        {
            int LoginID = GetLogin();
            if (OrderLoginID == LoginID)
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetOrderItemForExcel(string OrderID)
        {
            try
            {
                int LoginID = GetLogin();
                int Role = GetRole();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> rst = objOrderService.GetOrderItemsByOrderID(OrderID, LoginID);
                    string[] LotNo = rst.Select(x => x.Stock).ToArray();
                    string filterText = "LOTNO~" + string.Join(",", LotNo);
                    DataTable dt = this.objStockDetailsService.GetDataForExcelExport(filterText, false, LoginID);
                    if (dt.Rows.Count > 0)
                    {
                        //Guid fname = Guid.NewGuid();
                        string imgPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelMailImage"]);
                        byte[] st = ExportToExcel.InventoryExportToExcel(dt, imgPath, Role == 3 ? true : false, "AT", false);
                        TempData["SpecificSearchExport"] = st;
                        //return File(st, System.Net.Mime.MediaTypeNames.Application.Octet, "SpecificSearchExport.xlsx");
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
                ErrorLog.Log("OrderController", "GetOrderItemsByOrderID", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetMemoItemForExcel(string OrderID, string OrderStatus)
        {
            try
            {
                int LoginID = GetLogin();
                int Role = GetRole();
                if (LoginID > 0)
                {
                    List<inventoryDetailsViewModel> rst = objOrderService.GetOrderItemsByOrderID(OrderID, LoginID);
                    string[] LotNo = rst.Select(x => x.Stock).ToArray();
                    string filterText = "LOTNO~" + string.Join(",", LotNo);
                    DataTable dt = new DataTable();
                    if (OrderStatus == "Pending")
                    {
                        dt = this.objStockDetailsService.GetDataForExcelExport(filterText, false, LoginID);
                    }
                    else
                    {
                        dt = this.objStockDetailsService.GetMemoDataForExcelExport(filterText, LoginID);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //Guid fname = Guid.NewGuid();
                        string imgPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelMailImage"]);
                        byte[] st = ExportToExcel.InventoryExportToExcel(dt, imgPath, Role == 3 ? true : false, "AT", false);
                        TempData["SpecificSearchExport"] = st;
                        //return File(st, System.Net.Mime.MediaTypeNames.Application.Octet, "SpecificSearchExport.xlsx");
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
                ErrorLog.Log("OrderController", "GetOrderItemsByOrderID", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetOrderDetails(string OType)
        {
            int LoginID = GetLogin();
            int RoleID = GetRole();

            List<OrderListView> objResp = new List<OrderListView>();
            IQueryable<OrderListView> query = this.objOrderService.OrderListView();
 
            if (OType == "Pending")
            {
                query = query.Where(x => x.orderType == 14 && x.orderStatus == 10 );
            }
            else
            {
                query = query.Where(x => x.orderType == 14 && x.orderStatus == 11 );
            }
              
            if (RoleID == 3)
            {
                query = query.Where(x => x.loginID == LoginID);
            }


            objResp = query.Take(10).ToList(); 

            // ViewBag.SearchResultList = objInvVM.Take(10);  
            return Json(new Response { IsSuccess = true, Message = "200", Result = objResp });

        }


    }
}