using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class MemoService : IMemoService
    {
        private readonly UnitOfWork uow = null;
        private readonly OrderService objOrdSvc = null;
        //private readonly OrderService objOrdSvc = null
        private readonly StockDetailsService objStockDetailsService = null;
        private readonly DBSQLServer db = null;
        private MailUtility objMU = null;

        public MemoService(IUnitOfWork uow, IDBSQLServer db)
        {
            this.uow = uow as UnitOfWork;
            this.db = db as DBSQLServer;
            this.objOrdSvc = new OrderService(uow, db);
            this.objStockDetailsService = new StockDetailsService(uow, db);
        }

        public OrderInfoViewModel MemoInfo(int OrderID)
        {
            return this.objOrdSvc.OrderInfo(OrderID);
        }


        public MemoDetail CreateMemo(string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark, int FileID = 0)
        {
            return this.uow.Memo.CreateMemo(LotNos, LoginID, CustomerID, isConfirmed, isSellDirect, Remark);
        }

        public MemoDetail CancelPartialMemo(int OrderID, string LotNos, int LoginID)
        {
            int[] oID = GetOrderIDFromLotNos(LotNos);
            if (oID.Length > 1)
            {
                throw new UserDefinedException("Cannot cancel item from multiple memos");
            }
            int orderItemCount = this.uow.OrderItemDetails.Queryable().Where(x => x.orderDetailsId == OrderID).Count();
            int lotNoCount = LotNos.Split(',').Count();
            if (orderItemCount == lotNoCount)
            {
                throw new UserDefinedException("Cannot remove all items in memo");
            }
            return this.uow.Memo.CancelPartialMemo(OrderID, LotNos, LoginID);
        }

        public int SplitMemo(int OrderID, string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark)
        {
            return this.uow.Memo.SplitMemo(OrderID, LotNos, LoginID, CustomerID, isConfirmed, isSellDirect, Remark);
        }

        public int SellFullMemo(int OrderID, int LoginID, int MemoMode, decimal salesAvgDiscount)
        {
            int CustomerID = this.uow.Orders.Queryable().Where(x => x.orderDetailsId == OrderID).Select(x => x.customerId).FirstOrDefault();
            return this.uow.Memo.SellFullMemo(OrderID, LoginID, CustomerID, MemoMode, salesAvgDiscount);
        }

        public int CancelFullMemo(int OrderID, int LoginID)
        {
            int CustomerID = this.uow.Orders.Queryable().Where(x => x.orderDetailsId == OrderID).Select(x => x.customerId).FirstOrDefault();
            return this.uow.Memo.CancelFullMemo(OrderID, CustomerID, LoginID);
        }

        public int MergeMemo(int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark, string MergeOrderList)
        {
            return this.uow.Memo.MergeMemo(LoginID, CustomerID, isConfirmed, isSellDirect, Remark, MergeOrderList);
        }

        public int UpdateOrder(int OrderID, int CustomerID, string Remark, int LoginID)
        {
            return objOrdSvc.UpdateOrder(OrderID, CustomerID, Remark, LoginID);
        }

        public int[] GetOrderIDFromLotNos(string LotNos)
        {
            string[] lotNos = LotNos.Split(',');
            int[] invId = this.uow.Inventory.Queryable().Where(x => lotNos.Contains(x.lotNumber)).Select(x => x.inventoryID).ToList().ToArray();
            return this.uow.OrderItemDetails.Queryable().Where(x => invId.Contains(x.inventoryId)).Select(x => x.orderDetailsId).Distinct().ToList().ToArray();
        }

        public fileUploadLogModel GetFileByID(int FileID)
        {
            return uow.FileUploadLog.Queryable().Where(x => x.fileId == FileID).FirstOrDefault();
        }

        public int UpdateFile(fileUploadLogModel obj)
        {
            uow.FileUploadLog.Edit(obj);
            return uow.Save();
        }

        public int UpdateFile(int OrderID, int FileID)
        {
            MemoFileIDDetailModel objMF = uow.MemoFileIDDetail.Queryable().Where(x => x.orderDetailsId == OrderID).FirstOrDefault();
            if (objMF != null)
            {
                objMF.fileID = FileID;
                uow.MemoFileIDDetail.Edit(objMF);
                return uow.Save();
            }
            return 0;
        }

        public int MemoReturnSale(string OrderIDs, int UserID)
        {
            int RowCount = uow.Memo.MemoReturnSale(OrderIDs, UserID);
            return RowCount;
        }

        public void SendMailMemo(int OrderID, string UserEmailID, string ToName, int CustomerId, string Subject, string MessageContent, string TemplatePath, OrderInfoViewModel objOInfo = null, string OLotNos = null,int MemoMode= 1, decimal salesAvgDiscount = 0)
        {

            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            if (objOInfo == null)
            {
                objOInfo = this.objOrdSvc.OrderInfo(OrderID);
            }

            List<string> lstOfEmailIDs = new List<string>();
            //List<string> lstOfEmailIDCCs = new List<string>();
            string EmailIDsInCC = string.Empty;
            EmailIDsInCC = ((ConfigurationManager.AppSettings["CCemail"].ToString()));

            if (!string.IsNullOrEmpty(UserEmailID))
            {
                lstOfEmailIDs.Add(UserEmailID);
            }
            if (objOInfo != null && objOInfo.BillingAddress != null)
            {
                //OrderInfoViewModel objOInfo = OrderInfo(OrderID);
                string LotNos = "";
                if (OLotNos != null) 
                {
                    LotNos = "LOTNO~" + OLotNos;
                }
                else
                {
                    List<string> objLst = objOInfo.OrderItemDetail.Select(x => x.Stock).ToList();
                    LotNos = "LOTNO~" + string.Join(",", objLst);
                }
                //6 login id of shanay meheta 
                DataTable dt = MemoMode == 1 ? this.objStockDetailsService.GetDataForExcelExport2(LotNos, false, 6) :
                                              this.objStockDetailsService.GetDataForExcelExport3(OrderID, 6, salesAvgDiscount);
                //DataTable dt = this.objStockDetailsService.GetDataForExcelExport(LotNos, false, CustomerId);

                string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt, false, true);
                sbMailTemplate = sbMailTemplate.Replace("${UserName}", ToName);
                sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objOInfo.BillingAddress.firstName + " " + objOInfo.BillingAddress.lastName);
                sbMailTemplate = sbMailTemplate.Replace("${OrderNo}", OrderID.ToString());
                sbMailTemplate = sbMailTemplate.Replace("${OrderDate}", objOInfo.OrderDetail.orderCreatedOn.ToString("dd-MM-yyyy"));
                sbMailTemplate = sbMailTemplate.Replace("${Comments}", objOInfo.OrderDetail.remark);
                sbMailTemplate = sbMailTemplate.Replace("${Count}", (dt.Rows.Count - 1).ToString());
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);
                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, Subject + " " + objOInfo.BillingAddress.companyName, true, sbMailTemplate.ToString(), null, EmailIDsInCC);
            }
        }

        public int MemoPartialReturnSale(string LotNos, int LoginId)
        {
            return this.uow.Memo.MemoPartialReturnSale(LotNos, LoginId);
        }

        public CustOrderDetailModel orderItemDetailsfrominvintory(int Inventory)
        {


            orderItemDetailModel objd = new orderItemDetailModel();
            orderDetailModel objdet = new orderDetailModel();
            CustOrderDetailModel objcust = new CustOrderDetailModel();


            objd = this.uow.orderItemDetailM.Queryable().Where(x => x.inventoryId == Inventory).FirstOrDefault(); 
            objdet=   this.uow.orderDetail.Queryable().Where(x => x.orderDetailsId == objd.orderDetailsId).FirstOrDefault();

            objcust.customerId = objdet.customerId;
            objcust.orderDetailsId = objd.orderDetailsId;
            objcust.inventoryId = Inventory;

            return objcust;


        }

        
    }
}

