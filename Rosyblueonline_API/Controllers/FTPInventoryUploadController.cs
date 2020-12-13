using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace Rosyblueonline_API.Controllers
{
    [RoutePrefix("api/FTPInventoryUpload")]
    public class FTPInventoryUploadController : ApiController
    {

        IStockDetailsService objStockDetailsService;
        IMemoService objMemoService;
        CommonFunction commonFunction;
        IUserDetailService objUDSvc = null;
        IOrderService objOrderService;
        public FTPInventoryUploadController(IStockDetailsService objStockDetailsService, IMemoService objMemoService, IUserDetailService objUDSvc, IOrderService objOrderService)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objMemoService = objMemoService as MemoService;
            this.objUDSvc = objUDSvc as UserDetailService;
            this.objOrderService = objOrderService as OrderService;
        }





        // string UserHostAddress = "91.183.153.22";

        string UserHostAddress = ConfigurationManager.AppSettings["AntwerpIpAddress"].ToString();

        [HttpGet]
        [Route("InventoryUpload")]
        public void InventoryUpload()
        {
            string Filepath = ConfigurationManager.AppSettings["InventoryUploadInput"].ToString();

            string[] filePaths = Directory.GetFiles(Filepath, "*.xlsx");
            if (filePaths.Length > 0)
            {
                if (filePaths[0] != null || filePaths[0] != "")
                {
                    InventoryFileUpload(filePaths[0]);
                }
            }
        }


        public void InventoryFileUpload(string Filepath)
        {
            int uploadFormatId = 1;
            string path = ConfigurationManager.AppSettings["InventoryUploadOutput"].ToString();

            string fileExtn = "";
            string FileName = Path.GetFileName(Filepath);
            int fileId = 0;
            int LoginID = Convert.ToInt32(ConfigurationManager.AppSettings["AntwerpLoginId"].ToString());
            try
            {

                List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
                objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
                string InventoryuploadType = objVM.Where(x => x.uploadFormatId == uploadFormatId).Select(x => x.uploadValue).FirstOrDefault();


                fileExtn = Path.GetExtension(FileName);
                string ip = UserHostAddress;

                if (fileExtn == ".xls" || fileExtn == ".xlsx")
                {
                    fileId = this.objStockDetailsService.InsertFileUploadLog(FileName.ToString(), FileName.ToString(), LoginID.ToString(), UserHostAddress, uploadFormatId.ToString(), InventoryuploadType);

                    // File.Move(Filepath, path + fileId.ToString() + fileExtn);
                    System.IO.File.Move(Filepath, path + fileId.ToString() + fileExtn);

                    List<InventoryUpload> objLst = UploadInventory(LoginID, InventoryuploadType, fileExtn, path, fileId);

                    if (objLst.Count > 0)
                    {

                        List<InventoryUpload> objValidLst = objLst.Where(x => (x.LotStatus != null && x.LotStatus.ToLower() == "valid")).ToList();
                        List<InventoryUpload> objInValidLst = objLst.Where(x => (x.LotStatus != null && x.LotStatus.ToLower().Contains("invalid"))).ToList();
                        DataTable dtValid = ListtoDataTable.ToDataTable<InventoryUpload>(objValidLst);
                        DataTable dtNotValid = ListtoDataTable.ToDataTable<InventoryUpload>(objInValidLst);
                        ExportToExcel.SaveExcel(path, fileId.ToString() + "_Valid", "Valid", dtValid);
                        ExportToExcel.SaveExcel(path, fileId.ToString() + "_InValid", "InValid", dtNotValid);

                        SendMailOnUploadEvent(LoginID, InventoryuploadType,
                                              (path + fileId.ToString() + "_Valid" + ".xlsx"),
                                              (path + fileId.ToString() + "_InValid" + ".xlsx"),
                                              (path + fileId.ToString() + "_backup" + ".xlsx"),
                                              dtValid.Rows.Count,
                                              dtNotValid.Rows.Count,
                                              fileId);
                    }

                    ErrorLog.TestLog("InventoryFileUpload", "File upload ");

                }
                else
                {
                    ErrorLog.TestLog("InventoryFileUpload", "File Extension ");

                }


            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryFileUpload", path, ex);
            }
        }


        private List<InventoryUpload> UploadInventory(int LoginID, string InventoryuploadType, string fileExtn, string path, int fileId)
        {
            string procName = "";
            int RowCount = 0;
            int AdminID = Convert.ToInt32(ConfigurationManager.AppSettings["AdminID"].ToString());
            List<InventoryUpload> objinv = new List<InventoryUpload>();
            CustomerListView objCD1 = this.objUDSvc.GetCustomerByLoginID(LoginID);

            this.commonFunction = new CommonFunction();
            if (fileId > 0)
            {
                DataTable ds = commonFunction.GetDataFromExcel2(path + fileId.ToString() + fileExtn);

                procName = "proc_InventoryUploadCheckOrder_Antwerp";
                DataTable pds = ParseToString(ds);
                List<FTPInventoryUpload> objftp = new List<FTPInventoryUpload>();
                objftp = objStockDetailsService.FTPInventoryFileUpload(pds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);
                if (objftp != null)
                {
                    for (int i = 0; i < objftp.Count(); i++)
                    {
                        if (objftp[i].ActionName.ToString() == "FullOrder" && objftp[i].OrderNo.ToString() != null && objftp[i].OrderNo.ToString() != "")
                        {
                            int OrderID = Convert.ToInt32(objftp[i].OrderNo);
                            OrderInfoViewModel objinfo = this.objOrderService.OrderInfo(OrderID);
                            int CustomerID = this.objOrderService.OrderListView().Where(x => x.orderDetailsId == OrderID).Select(x => x.loginID).FirstOrDefault();
                            RowCount = this.objOrderService.OrderCancel(OrderID, AdminID, CustomerID);
                            if (RowCount > 0 && objinfo != null)
                            {
                                this.objOrderService.SendForOrder(objinfo, CustomerID, ConfigurationManager.AppSettings["AntwerpEmailTemplate_CancelOrder"].ToString(), "", true);
                            }
                        }
                        else if (objftp[i].ActionName.ToString() == "FullMemo" && objftp[i].OrderNo.ToString() != null && objftp[i].OrderNo.ToString() != "")
                        {
                            int OrderID = Convert.ToInt32(objftp[i].OrderNo);
                            int RowCount1 = 0;
                            OrderInfoViewModel objInfo = this.objMemoService.MemoInfo(OrderID);

                            RowCount1 = this.objMemoService.CancelFullMemo(OrderID, LoginID);
                            if (objInfo != null && RowCount1 > 0)
                            {
                                this.objMemoService.SendMailMemo(OrderID, objCD1.emailId, objCD1.firstName, objInfo.UserDetail.loginID, "List of inventory CancelMemo from memo-", "", ConfigurationManager.AppSettings["AntwerpEmailTemplate_CancelMemo"].ToString(), objInfo);
                                bool log = this.objUDSvc.UserActivitylogs(LoginID, "Cancel full memo", OrderID.ToString());
                            }
                        }
                        else if (objftp[i].ActionName.ToString() == "PartialOrder" && objftp[i].OrderNo.ToString() != null && objftp[i].OrderNo.ToString() != "")
                        {
                            int OrderID = Convert.ToInt32(objftp[i].OrderNo);
                            string LotNos = objftp[i].LotNOs;
                            int CustomerID = this.objOrderService.OrderListView().Where(x => x.orderDetailsId == OrderID).Select(x => x.loginID).FirstOrDefault();
                            RowCount = this.objOrderService.OrderPartialCancel(OrderID, LotNos, AdminID, CustomerID);


                        }
                        else if (objftp[i].ActionName.ToString() == "PartialMemo" && objftp[i].OrderNo.ToString() != null && objftp[i].OrderNo.ToString() != "")
                        {

                            string LotNos = objftp[i].LotNOs;
                            int FileNo = fileId;
                            int OrderID = Convert.ToInt32(objftp[i].OrderNo);
                            OrderInfoViewModel objInfo = this.objMemoService.MemoInfo(OrderID);
                            MemoDetail obj = this.objMemoService.CancelPartialMemo(OrderID, LotNos, LoginID);
                             
                                fileUploadLogModel objFile = this.objMemoService.GetFileByID(FileNo);
                                if (objFile != null)
                                {
                                    string path1 = ConfigurationManager.AppSettings["INVUpload"].ToString();
                                    DataTable dtValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "Valid").ToList());
                                    DataTable dtNotValid = ListtoDataTable.ToDataTable<InventoryUpload>(obj.Inv.Where(x => x.LotStatus == "InValid").ToList());
                                    ExportToExcel.SaveExcel(path1, FileNo.ToString() + "_Valid", "Valid", dtValid);
                                    ExportToExcel.SaveExcel(path1, FileNo.ToString() + "_InValid", "InValid", dtNotValid);
                                    objFile.validInv = obj.Inv.Where(x => x.LotStatus == "Valid").Count();
                                    objFile.invalidInv = obj.Inv.Where(x => x.LotStatus == "InValid").Count();
                                    this.objMemoService.UpdateFile(objFile);
                                    if (obj.Counts.OrderId > 0)
                                    {
                                        this.objMemoService.UpdateFile(obj.Counts.OrderId, FileNo);
                                    }
                                }
                            
                            if (objInfo != null && obj.Counts != null && obj.Counts.OrderId > 0)
                            {
                                this.objMemoService.SendMailMemo(obj.Counts.OrderId, objCD1.emailId, objCD1.firstName, objInfo.UserDetail.loginID, "List of inventory Partial Canceled Memo from memo-", "", ConfigurationManager.AppSettings["AntwerpEmailTemplate_CancelMemo"].ToString(), objInfo, LotNos);
                                bool log = this.objUDSvc.UserActivitylogs(LoginID, "Partial canceled memo", LotNos);
                            }


                        }

                    }

                    procName = "proc_InventoryUpload_Antwerp";
                    objinv = objStockDetailsService.FTPInventoryUploadandModify(procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);

                }
                return objinv;


            }
            return null;
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


        private void SendMailOnUploadEvent(int LoginID, string EventName, string ValidFileName, string InValidFileName, string BackUpFile, int ValidCount, int InValidCount, int FileID)
        {
            bool SentMail = true;
            string Subject = string.Format("List of inventory \"{0}\" from - Rosyblueonline.com", EventName);
            string Message = "<p>Please find the attached file to view stones updated with {0}.</p><p>No. of stones: {1}</p> ";

            CustomerListView objCD = this.objUDSvc.GetCustomerByLoginID(LoginID);

            Message = string.Format(Message, EventName, ValidCount);
            if (SentMail)
            {
                objStockDetailsService.SendUploadEventMail(ConfigurationManager.AppSettings["AntwerpEmailTemplate_EventSendMail"], objCD.emailId, objCD.firstName, Subject, Message, ValidFileName, InValidFileName, ConfigurationManager.AppSettings["CCemail"].ToString());
            }

        }




    }
}
