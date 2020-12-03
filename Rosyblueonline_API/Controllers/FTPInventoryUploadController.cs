using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
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
        public FTPInventoryUploadController(IStockDetailsService objStockDetailsService, IMemoService objMemoService)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
            this.objMemoService = objMemoService as MemoService;
        }



        string UserHostAddress = "1.1.1.1";
        string Filepath = @"D:\Ankit\UploadFormat.cleaned.xlsx";
        string GetToken = "";

        [HttpGet]
        [Route("InventoryUpload")]
        public void InventoryUpload()
        {
            int uploadFormatId = 1;

            string path = @"D:\Ankit\Inv\";
            string fileExtn = "";
            string FileName = Path.GetFileName(Filepath);
            int fileId = 0;

            try
            {

                int LoginID = 111;
                List<mstUploadFormatViewModel> objVM = new List<mstUploadFormatViewModel>();
                objVM = objStockDetailsService.InventoryUploadTypes("Upload_Types");
                string InventoryuploadType = objVM.Where(x => x.uploadFormatId == uploadFormatId).Select(x => x.uploadValue).FirstOrDefault();


                fileExtn = Path.GetExtension(FileName);
                string ip = UserHostAddress;
                 
                if (fileExtn == ".xls" || fileExtn == ".xlsx")
                {
                    fileId = this.objStockDetailsService.InsertFileUploadLog(FileName.ToString(), FileName.ToString(), LoginID.ToString(), UserHostAddress, uploadFormatId.ToString(), InventoryuploadType);
                    // Request.Files[0].SaveAs(path + fileId.ToString() + fileExtn);
                  //  System.IO.File.Move(path + fileId.ToString() + fileExtn, Filepath);

                    File.Move(Filepath, path + fileId.ToString() + fileExtn);

                    List<InventoryUpload> objLst = UploadInventory(LoginID, InventoryuploadType, fileExtn, path, fileId);
                    if (objLst.Count > 0)
                    {
                         
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
                        //SendMailOnUploadEvent(InventoryuploadType,
                        //                      (path + fileId.ToString() + "_Valid" + ".xlsx"),
                        //                      (path + fileId.ToString() + "_InValid" + ".xlsx"),
                        //                      (path + fileId.ToString() + "_backup" + ".xlsx"),
                        //                      dtValid.Rows.Count,
                        //                      dtNotValid.Rows.Count,
                        //                      fileId);
                    }

                    ErrorLog.Log("InventoryController", "File upload ", null);

                }
                else
                {
                    ErrorLog.Log("InventoryController", "File Extension ", null);

                }


            }
            catch (Exception ex)
            {
                ErrorLog.Log("InventoryController", path, ex);
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
                    return objStockDetailsService.InventoryUpload(pds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "INVENTORY_MODIFY" || InventoryuploadType == "JA_BN")
                {
                    procName = "proc_QCModifyInventory";
                    ds = RemoveBlankRows(ds);
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "CHANGE_DISCOUNT")
                {
                    //NotAppicableConfMemo
                    procName = "proc_DiscModifyInventory";
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);
                    string LotIDs = string.Join(",", objLst.Where(x => x.Stock != null && x.Stock != "").Select(x => x.Stock).ToArray<string>());
                    List<inventoryDetailsViewModel> objInvLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotIDs);
                    ExportToExcel.SaveExcel(path, fileId.ToString() + "_backup" + fileExtn, "Back", ListtoDataTable.ToDataTable<inventoryDetailsViewModel>(objInvLst));
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "CHANGE_RAPP")
                {
                    procName = "proc_RapModifyInventory";
                    List<InventoryUpload> objLst = GetLotNosFromDataTable(ds);
                    string LotIDs = string.Join(",", objLst.Where(x => x.Stock != null && x.Stock != "").Select(x => x.Stock).ToArray<string>());
                    List<inventoryDetailsViewModel> objInvLst = objStockDetailsService.GetInventoriesByLotID(LoginID, LotIDs);
                    ExportToExcel.SaveExcel(path, fileId.ToString() + "_backup" + fileExtn, "Back", ListtoDataTable.ToDataTable<inventoryDetailsViewModel>(objInvLst));
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, InventoryuploadType);
                }
                else if (InventoryuploadType == "MEMO_UPLOAD")
                {
                    return GetLotNosFromDataTable(ds);
                }

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

                        MemoDetail objMd = objMemoService.CancelPartialMemo(OrderID[0], string.Join(",", LotNos), LoginID);

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
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, "ENABLE");
                }
                else if (InventoryuploadType == "DISABLE_INV")
                {
                    procName = "proc_EnableDisableInventory";
                    return objStockDetailsService.InventoryUpload(ds, procName, Convert.ToString(LoginID), Convert.ToString(fileId), UserHostAddress, "DISABLE");
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
                    RowCount = objStockDetailsService.BestDeals(ds, "", 0, "Via File Upload", fileId, GetToken, LoginID, "Add_bestdeal_upload");
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
                    RowCount = objStockDetailsService.BestDeals(ds, "", 0, "", fileId, GetToken, LoginID, "Remove_bestdeal_upload");
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

                    RowCount = objMemoService.MemoPartialReturnSale(string.Join(",", LotNos), LoginID);
                    return objLst;
                }


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

    }
}
