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
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    [CheckSession]
    public class RFIDController : _BaseController
    {
        RFIDService objSvc;
        StockDetailsService objSDC;
        public RFIDController(IRFIDService objSvc, IStockDetailsService objSDC)
        {
            this.objSvc = objSvc as RFIDService;
            this.objSDC = objSDC as StockDetailsService;
        }

        // GET: RFID
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
        public JsonResult RecycleRFId(string Rfid)
        {
            try
            {
                //Remove new line, tab and 
                Rfid = Regex.Replace(Rfid, @"\t|\n|\r", "");
                string[] rfIds = Rfid.Split(',');
                int RowCount = this.objSvc.ReleaseRFID(rfIds);
                return Json(new Response { IsSuccess = true, Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "RecycleRFId", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRfId()
        {
            try
            {
                int LoginID = GetLogin();
                if (Request.Files.Count > 0 && LoginID > 0)
                {
                    int FileID = 0;
                    int RowCount = 0;
                    string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
                    CommonFunction com = new CommonFunction();
                    //Guid guFileName = Guid.NewGuid();
                    string FileName = Request.Files[0].FileName.ToString();
                    string FilePath = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"]);
                    string FileExtension = Path.GetExtension(Request.Files[0].FileName);
                    string ip = CommonFunction.GetIpAddress(Request);
                    FileID = this.objSDC.InsertFileUploadLog(FileName + FileExtension, "RFID_" + FileName + FileExtension, LoginID.ToString(), ip, "12", "RFID_UPDATE");
                    Request.Files[0].SaveAs(FilePath + FileID.ToString() + FileExtension);
                    DataTable dt = com.GetDataFromExcel2(FilePath + FileID + FileExtension, true);
                    if (dt == null)
                    {
                        throw new Exception("DataTable is null");
                    }
                    DataTable pDt = ParseToString(dt);
                    if (dt.Rows.Count > 0)
                    {
                        List<RFIDTempTableMiewModel> lstofRfid = this.objSvc.InventoryUploadRFID(pDt, LoginID, ip, FileID);
                        List<RFIDTempTableMiewModel> objValidLst = lstofRfid.Where(x => (x.Status != null && x.Status.ToLower() == "valid")).ToList();
                        List<RFIDTempTableMiewModel> objInValidLst = lstofRfid.Where(x => (x.Status != null && x.Status.ToLower().Contains("invalid"))).ToList();
                        DataTable dtValid = ListtoDataTable.ToDataTable<RFIDTempTableMiewModel>(objValidLst);
                        DataTable dtNotValid = ListtoDataTable.ToDataTable<RFIDTempTableMiewModel>(objInValidLst);
                        ExportToExcel.SaveExcel(path, FileID.ToString() + "_Valid", "Valid", dtValid);
                        ExportToExcel.SaveExcel(path, FileID.ToString() + "_InValid", "InValid", dtNotValid);
                    }
                    return Json(new Response { IsSuccess = true, Result = new { FileID = FileID, RowCount = RowCount } });
                }
                return Json(new Response { IsSuccess = false, Message = "File not uploaded" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "UpdateRfId", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult InventoryUpdateRFIDStatus(string rfid, bool Status)
        {
            try
            {
                int LoginID = GetLogin();
                int RowCount = 0;
                if (LoginID > 0)
                {
                    RowCount = this.objSvc.InventoryUpdateRFIDStatus(rfid, Status);
                    return Json(new Response { IsSuccess = true, Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "InventoryUpdateRFIDStatus", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult StockTally()
        {
            return View();
        }

        public ActionResult ViewStockTally(string stockid, string RFId, string boxName, string Rfidmachine, string Boxid)
        {
            try
            {
                List<RFIDstockmaster> result = new List<RFIDstockmaster>();
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    result = this.objSvc.RFIDstockTally(stockid, RFId, boxName, Rfidmachine, LoginID, Boxid, "AddStockbox");
                    return Json(new Response { Code = 200, Result = result });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "ViewStockTally", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult StockDetails(string stockid, string Boxid, string stockstatus)
        {
            try
            {
                List<RFIDexportBox> result = new List<RFIDexportBox>();
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    if (Boxid == "TotalStockExport")
                        result = this.objSvc.RFIdstockBoxExport(stockid, "", "", "", LoginID, Boxid, "TotalStockExport");
                    else
                        result = this.objSvc.RFIdstockBoxExport(stockid, "", "", "", LoginID, Boxid, "ExportStockbox");

                    return Json(new Response { Code = 200, Result = result });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "StockDetails", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult RFIDMaster()
        {
            List<mstRFIDModel> objLst = new List<mstRFIDModel>();
            try
            {
                objLst = this.objSvc.RFIDmaster();
                return View(objLst);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "RFIDMaster", ex);
                return View(objLst);
            }
        }

        [HttpPost]
        public ActionResult AddNewRfId()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    if (Request.Files.Count > 0)
                    {
                        int RowCount = 0;
                        CommonFunction com = new CommonFunction();
                        Guid guFileName = Guid.NewGuid();
                        string FileName = guFileName.ToString();
                        string FilePath = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"]);
                        string FileExtension = Path.GetExtension(Request.Files[0].FileName);
                        Request.Files[0].SaveAs(FilePath + "RFID_" + FileName + FileExtension);
                        string ip = CommonFunction.GetIpAddress(Request);
                        DataTable dt = com.GetDataFromExcel2(FilePath + "RFID_" + FileName + FileExtension, true);
                        DataTable pDt = ParseToString(dt);
                        if (dt.Rows.Count > 0)
                        {
                            RowCount = this.objSvc.AddRFID(pDt, LoginID);
                        }
                        return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                    }
                    return Json(new Response { IsSuccess = false, Message = "File not attached" });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("RFIDController", "AddNewRfId", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }

        }

        public ActionResult History()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GridHistory(DataTableViewModel objReq)
        {
            try
            {
                int CustomerID = GetLogin();
                int RoleID = GetRole();
                if (objReq != null)
                {
                    DataTableResponse<RFIDhistoryViewModel> objResp = new DataTableResponse<RFIDhistoryViewModel>();
                    IQueryable<RFIDhistoryViewModel> query = this.objSvc.RFIDMasterQueryable();
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "RFIDno":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.RFIDno);
                                else
                                    query = query.OrderByDescending(x => x.RFIDno);
                                break;

                            case "CertificateNO":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.CertificateNO);
                                else
                                    query = query.OrderByDescending(x => x.CertificateNO);
                                break;

                            case "CreatedByName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.CreatedByName);
                                else
                                    query = query.OrderByDescending(x => x.CreatedByName);
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
                ErrorLog.Log("RFIDController", "GridBlueNile", ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult TallyStockByRFID(string RFIDs)
        {
            try
            {
                int CustomerID = GetLogin();
                if (CustomerID != 0)
                {
                    var rData = this.objSvc.TallyStockByRFID(RFIDs, CustomerID);
                    return Json(new Response { IsSuccess = true, Message = "", Result = rData });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MarketingController", "GetStockStatus", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        //[HttpGet]
        //[Route("RFID/TallyMemo/{OrderID}")]
        public ActionResult TallyMemo(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult TallyMemoByRfid(int OrderID, string RFIDs)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    MemoTallyStockByRfidViewModel objLst = this.objSvc.MemoTallyStockByRfid(LoginID, OrderID, RFIDs);
                    return Json(new Response { Code = 200, IsSuccess = true, Message = "", Result = objLst });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("MemoController", "MemoTallyStockByRfid", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult UploadHistory()
        {
            return View();
        }

        public DataTable ParseToString(DataTable oldDT)
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
                    dr[j] = Convert.ToString(oldDT.Rows[i][j]);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}