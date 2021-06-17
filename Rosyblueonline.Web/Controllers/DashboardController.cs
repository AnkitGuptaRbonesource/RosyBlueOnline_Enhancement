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
using System.Web.Services;

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class DashboardController : _BaseController
    {
        private readonly IHomeServiceProvider objHomeServiceProvider;
        private readonly IStockDetailsService objStockDetailsService;
        private readonly IRecentSearchService objRecentSearchService;
        private readonly IUserDetailService objUserDetailService;
        private readonly IOrderService objOrderService;
        public DashboardController(IStockDetailsService objStockDetailsService, IRecentSearchService objRecentSearchService, IHomeServiceProvider objHomeServiceProvider, IUserDetailService objUserDetailService, IOrderService objOrderService)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objRecentSearchService = objRecentSearchService as RecentSearchService;
            this.objHomeServiceProvider = objHomeServiceProvider as HomeServiceProvider;
            this.objUserDetailService = objUserDetailService as UserDetailService;
            this.objOrderService = objOrderService as OrderService;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            List<OrderListView> objLst = this.objOrderService.OrderListView().Where(x => x.orderType == 14 && x.orderStatus == 10).ToList();
            return View(objLst);
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
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Sales()
        {
            return View("Admin");
        }
        public ActionResult AdminSupport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetDashboard()
        {
            try
            {
                int LoginID = GetLogin();
                DashboardViewModel objVM = objStockDetailsService.DashboardView(LoginID);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetDashboardSavedSearches()
        {
            try
            {
                int LoginID = GetLogin();
                List<RecentSearchViewModel> data = new List<RecentSearchViewModel>();

                //DashboardViewModel objVM = objStockDetailsService.DashboardView(LoginID);
                //        return Json(objVM, JsonRequestBehavior.AllowGet);

                return View("~/Views/Dashboard/_SavedSearchView.cshtml", data);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetDashboardRecentSearches()
        {
            try
            {
                int LoginID = GetLogin();
                List<RecentSearchViewModel> data = new List<RecentSearchViewModel>();

                //DashboardViewModel objVM = objStockDetailsService.DashboardView(LoginID);
                //        return Json(objVM, JsonRequestBehavior.AllowGet);

                return View("~/Views/Dashboard/_RecentSearchView.cshtml", data);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetDashboardDemandSearches()
        {
            try
            {
                int LoginID = GetLogin();
                List<RecentSearchViewModel> data = new List<RecentSearchViewModel>();

                //DashboardViewModel objVM = objStockDetailsService.DashboardView(LoginID);
                //        return Json(objVM, JsonRequestBehavior.AllowGet);

                return View("~/Views/Dashboard/_DemandSearchView.cshtml", data);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetRecentSearchView(int UserID)
        {
            try
            {
                List<RecentSearchViewModel> objVM = objStockDetailsService.RecentSearchView(UserID);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }
        [CustomAuthorize("StockSummary")]
        public ActionResult StockSummary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStockSummary(string Stone, string LocationID)
        {
            try
            {
                int LoginID = GetLogin();
                List<StockSummaryViewModel> objVM = objStockDetailsService.StockSummary(Stone, LocationID);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetStockSummary", ex);
                throw ex;
            }
        }

        //Added by Ankit 11July2020

        [HttpPost]
        public ActionResult GetStoneDetailsStockSummary(string StartRange, string EndRange, string Shape, string Location, string Mode, string Event)
        {
            try
            {
                int LoginID = GetLogin();
                //List<StoneDetailsStockSummaryModel> objVM = objStockDetailsService.StoneDetailsStockSummary("0.23", "4.30", "ALL", "1,11", "VALUES", "FOR_TOTAL_INVENTORY");
                List<StoneDetailsStockSummaryModel> objVM = objStockDetailsService.StoneDetailsStockSummary(StartRange, EndRange, Shape, Location, Mode, Event);
                // List<StoneDetailsStockSummaryModel> objVM = objStockDetailsService.StoneDetailsStockSummary(StartRange, EndRange, "ALL", "1,11", "VALUES", "FOR_TOTAL_INVENTORY");

                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetStoneDetailsStockSummary", ex);
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult GetStockStatus()
        {
            try
            {
                int LoginID = GetLogin();
                DashboardViewModel objVM = objStockDetailsService.DashboardView(LoginID);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetDashboard", ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult RemoveRecent(int recentSearchID)
        {
            try
            {
                var RCount = objRecentSearchService.Delete(recentSearchID);
                return Json(new Response { IsSuccess = true, Message = "200", Result = RCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", "RemoveRecent", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Count()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID != 0)
                {
                    UserCountViewModel objCount = this.objHomeServiceProvider.GetUserCounts(LoginID);
                    return Json(new Response { IsSuccess = true, Message = "", Result = objCount }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("DashboardController", "Count", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddToCartPermitted()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID != 0)
                {
                    MstCustomerPermisionModel objCP = this.objHomeServiceProvider.AddToCartPermitted(LoginID);
                    return Json(new Response { IsSuccess = true, Message = "", Result = objCP }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("DashboardController", "AddToCartPermitted", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CustomerListingPendingApproval(DataTableViewModel objReq)
        {
            try
            {
                if (objReq != null)
                {
                    DataTableResponse<CustomerListView> objResp = new DataTableResponse<CustomerListView>();
                    IQueryable<CustomerListView> query = this.objUserDetailService.CustomerList().Where(x => x.isApproved == 0);
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
                            case "firstName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.firstName);
                                else
                                    query = query.OrderByDescending(x => x.firstName);
                                break;
                            case "emailId":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.emailId);
                                else
                                    query = query.OrderByDescending(x => x.emailId);
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
                ErrorLog.Log("DashboardController", "CustomerListingPendingApproval", ex);
                throw;
            }
        }



        [HttpPost]
        public JsonResult UserActivityDetails(DataTableViewModel objReq)
        {
            try
            {

                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {

                    DataTableResponse<UserGeoLocationModel> objResp = new DataTableResponse<UserGeoLocationModel>();
                    // IQueryable<UserGeoLocationModel> query = this.objUserDetailService.UserActivityDetails();

                    List<UserGeoLocationModel> objgeo = objUserDetailService.GetUserGeoLoctionLog(CustomerID);

                    IQueryable<UserGeoLocationModel> query = objgeo.AsQueryable();

                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "Username":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.Username);
                                else
                                    query = query.OrderByDescending(x => x.Username);
                                break;

                            default:
                            case "CreatedOn":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.CreatedOn);
                                else
                                    query = query.OrderByDescending(x => x.CreatedOn);
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
        [HttpGet]
        public ActionResult GetCustomerLogDetails(int UserID)
        {
            try
            {

                List<UserActivityLogModel> objVM = objUserDetailService.GetCustomerLog(UserID);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Dashboard", "GetCustomerLogDetails", ex);
                throw ex;
            }
        }

        public ActionResult SearchDataFromExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    CommonFunction com = new CommonFunction();
                    string path = Server.MapPath("~/Content/SearchFiles/");
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    Guid guid = Guid.NewGuid();
                    string FileName = guid.ToString() + extension;
                    Request.Files[0].SaveAs(path + "/" + FileName); 

                    DataTable dt = com.GetDataFromExcel2(path + "/" + FileName, true); 

                    if (System.IO.File.Exists(path + "/" + FileName))
                    {
                        System.IO.File.Delete(path + "/" + FileName);
                    }
                    string Searchid = "";
                    if (dt.Columns[0].ToString() == "Lotnumber")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Searchid = Searchid + dt.Rows[i]["Lotnumber"].ToString() + " " + dt.Rows[i]["Certificate"].ToString() + " ";

                        }
                        return Json(new Response { IsSuccess = true, Message = "", Result = Searchid });
                    }

                    return Json(new Response { IsSuccess = false, Message = "Wrong file format!" });

                }
                return Json(new Response { IsSuccess = false, Message = "No file uploaded" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("DashboardController", "SearchDataFromExcel", ex);
                return Json(new Response { IsSuccess = false, Message = "Error" });
            }

        }


    }
}