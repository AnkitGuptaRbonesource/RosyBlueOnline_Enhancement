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
    public class OrderService : IOrderService
    {
        UnitOfWork uow = null;
        MailUtility objMU = null;
        StockDetailsService objStockDetailsService = null;
        DBSQLServer db = null;
        public OrderService(IUnitOfWork uow, IDBSQLServer db)
        {
            this.uow = uow as UnitOfWork;
            this.db = db as DBSQLServer;
            this.objStockDetailsService = new StockDetailsService(uow, db);
        }

        public OrderViewModel PreBookOrder(string LotNos, int CustomerId, int ShippingMode)
        {
            OrderViewModel objOVM = this.uow.Orders.PreBookOrder(LotNos, CustomerId, ShippingMode);
            //SendMailToAdmin(objOVM, CustomerId);
            return objOVM;
        }

        public PlaceOrderViewModel BookOrder(string LotNos, int CreatedBy, int CustomerId, int ShippingMode, int BillingID, int ShippingID)
        {
            return this.uow.Orders.BookOrder(LotNos, CreatedBy, CustomerId, ShippingMode, BillingID, ShippingID);
        }

        public IQueryable<OrderListView> OrderListView()
        {
            return this.uow.Orders.OrderListView();
        }

        public OrderInfoViewModel OrderInfo(int OrderID)
        {
            return this.uow.Orders.OrderInfo(OrderID);
        }


        public int OrderPartialCancel(int OrderID, string LotNo, int LoginID, int CustomerID)
        {
            string[] lotNos = LotNo.Split(',');
            int orderItemCount = this.uow.OrderItemDetails.GetAll().Where(x => x.orderDetailsId == OrderID).Count();
            if (lotNos.Length == orderItemCount)
            {
                throw new UserDefinedException(StringResource.CannotRemoveAllItems);
            }
            return this.uow.Orders.OrderPartialCancel(OrderID, LotNo, LoginID, CustomerID);
        }

        public int OrderCancel(int OrderID, int LoginID, int CustomerID)
        {
            return this.uow.Orders.OrderCancel(OrderID, LoginID, CustomerID);
        }

        public int OrderComplete(int OrderID, int LoginID, int CustomerID, string ShippingCompany, string TrackingNumber)
        {
            return this.uow.Orders.OrderComplete(OrderID, LoginID, CustomerID, ShippingCompany, TrackingNumber);
        }

        public List<inventoryDetailsViewModel> GetOrderItemsByOrderID(string OrderID, int UserID)
        {
            return this.uow.Orders.GetOrderItemsByOrderID(OrderID, UserID);
        }

        public int UpdateOrder(int OrderID, int CustomerID, string Remark, int LoginID)
        {
            orderDetailModel objOD = this.uow.Orders.Queryable().Where(x => x.orderDetailsId == OrderID).FirstOrDefault();
            MstBillingAddressModel objBA = this.uow.MstBillingAddresses.Queryable().Where(x => x.loginID == CustomerID).FirstOrDefault();
            MstShippingAddressModel objSA = this.uow.MstShippingAddresses.Queryable().Where(x => x.loginID == CustomerID).FirstOrDefault();
            if (objOD != null && objBA != null && objSA != null)
            {
                objOD.billingId = objBA.billingId;
                objOD.shippingId = objSA.shippingId;
                objOD.customerId = CustomerID;
                objOD.remark = Remark;
                objOD.modifiedBy = LoginID;
                objOD.modifiedOn = DateTime.Now;
                this.uow.Orders.Edit(objOD);
                return this.uow.Save();
            }
            return 0;
        }

        public int OrderMerge(int LoginID, string MergeOrderList)
        {
            string[] sOrderIDs = MergeOrderList.Split(',');
            List<int> OrderIDs = new List<int>();
            for (int i = 0; i < sOrderIDs.Length; i++)
            {
                OrderIDs.Add(Convert.ToInt32(sOrderIDs[i]));
            }
            var arrIds = OrderIDs.ToArray();
            List<int> Cust = this.uow.Orders.Queryable().Where(x => arrIds.Contains(x.orderDetailsId)).Select(x => x.customerId).Distinct().ToList();
            if (Cust.Count > 1)
            {
                throw new UserDefinedException("Cannot merge orders of different customers.");
            }
            return this.uow.Orders.OrderMerge(LoginID, Cust[0], MergeOrderList);
        }

        public void SendMailPreBookOrder(int OrderID, int CustomerId, string TemplatePath, string Subject, bool SentToCustomer = false)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            OrderInfoViewModel objOInfo = OrderInfo(OrderID);
            List<string> lstOfEmailIDs = new List<string>();
            if (SentToCustomer && objOInfo != null && objOInfo.UserDetail != null)
            {
                lstOfEmailIDs.Add(objOInfo.UserDetail.emailId);
            }
            else
            {
                int CountLocation = 0;
                for (int i = 0; i < objOInfo.OrderItemDetail.Count; i++)
                {
                    if (objOInfo.OrderItemDetail[i].SalesLocation.ToLower().Trim() == "belgium")
                    {
                        CountLocation = CountLocation + 1;
                    }
                }
                if (CountLocation == 0)
                {
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["Email_ID"].ToString());
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["CCemail"].ToString());
                }
                else if (CountLocation == objOInfo.OrderItemDetail.Count)
                {
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["AntwerpEmail_ID"].ToString());

                }
                else
                {
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["AntwerpEmail_ID"].ToString());
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["Email_ID"].ToString());
                    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["CCemail"].ToString());

                }

            }
            if (objOInfo != null && objOInfo.BillingAddress != null)
            {
                List<string> objLst = objOInfo.OrderItemDetail.Select(x => x.Stock).ToList();
                string LotNos = "LOTNO~" + string.Join(",", objLst);
                DataTable dt = this.objStockDetailsService.GetDataForExcelExportEmail(LotNos, false, CustomerId);
                if (dt.Columns.Count > 0)
                {
                    dt.Columns.Remove("Sizes");
                    dt.Columns.Remove("CertificateNo");
                    dt.Columns.Remove("Reportdate");
                    dt.Columns.Remove("EyeClean");
                    dt.Columns.Remove("Shade");
                    dt.Columns.Remove("TableBlack");
                    dt.Columns.Remove("SideBlack");
                    dt.Columns.Remove("Milky");
                    dt.Columns.Remove("CuletSize");
                    dt.Columns.Remove("OpensName");
                    dt.Columns.Remove("GroupName");
                    dt.Columns.Remove("MemberComments");
                    dt.Columns.Remove("refdata");
                    dt.Columns.Remove("V360");
                    dt.Columns.Remove("Video");
                    if (SentToCustomer == true)
                    {
                        dt.Columns.Remove("SalesLocation");
                    }


                }
                DataTable dtOrderCharges = objOInfo.ConvertOrderChangesInDatetable();
                string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt, false, true);
                string htmlTableForOrderChargesDetail = CommonFunction.ConvertDataTableToHTML(dtOrderCharges, false, true);
                sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objOInfo.BillingAddress.firstName + " " + objOInfo.BillingAddress.lastName);
                sbMailTemplate = sbMailTemplate.Replace("${OrderNo}", OrderID.ToString());
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATAFORCHARGES}", htmlTableForOrderChargesDetail);
                sbMailTemplate = sbMailTemplate.Replace("${CompanyName}", objOInfo.BillingAddress.companyName);
                sbMailTemplate = sbMailTemplate.Replace("${Address}", objOInfo.BillingAddress.address01 + " " + objOInfo.BillingAddress.address02);
                sbMailTemplate = sbMailTemplate.Replace("${CountryName}", objOInfo.BillingAddress.countryName);
                sbMailTemplate = sbMailTemplate.Replace("${phoneCode01}", objOInfo.UserDetail.phoneCode01);
                sbMailTemplate = sbMailTemplate.Replace("${phone01}", objOInfo.UserDetail.phone01);
                sbMailTemplate = sbMailTemplate.Replace("${emailId}", objOInfo.UserDetail.emailId);
                sbMailTemplate = sbMailTemplate.Replace("${bankName}", objOInfo.UserDetail.bankName);
                sbMailTemplate = sbMailTemplate.Replace("${branchName}", objOInfo.UserDetail.branchName);
                sbMailTemplate = sbMailTemplate.Replace("${branchAddress}", objOInfo.UserDetail.branchAddress);
                sbMailTemplate = sbMailTemplate.Replace("${accNumber}", objOInfo.UserDetail.accNumber);
                sbMailTemplate = sbMailTemplate.Replace("${swiftCode}", objOInfo.UserDetail.swiftCode);

                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, Subject, true, sbMailTemplate.ToString());
            }

        }

        public void SendForOrder(OrderInfoViewModel objOInfo, int CustomerId, string TemplatePath, string Message, bool SentToCustomer = false)
        {

            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            List<string> lstOfEmailIDs = new List<string>();
            if (SentToCustomer && objOInfo != null && objOInfo.UserDetail != null)
            {
                lstOfEmailIDs.Add(objOInfo.UserDetail.emailId);
            }
            else
            {
                lstOfEmailIDs.Add(ConfigurationManager.AppSettings["Email_ID"].ToString());
                lstOfEmailIDs.Add(ConfigurationManager.AppSettings["CCemail"].ToString());
            }
            if (objOInfo != null && objOInfo.BillingAddress != null)
            {
                List<string> objLst = objOInfo.OrderItemDetail.Select(x => x.Stock).ToList();
                string LotNos = "LOTNO~" + string.Join(",", objLst);
                DataTable dt = this.objStockDetailsService.GetDataForExcelExport(LotNos, false, CustomerId);
                if (dt.Columns.Count > 0)
                {
                    dt.Columns.Remove("Weight");
                    if (SentToCustomer == true)
                    {
                        dt.Columns.Remove("SalesLocation");
                    }
                }
                DataTable dtOrderCharges = objOInfo.ConvertOrderChangesInDatetable();

                string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt, false, true);
                string htmlTableForOrderChargesDetail = CommonFunction.ConvertDataTableToHTML(dtOrderCharges, false, true);
                sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objOInfo.BillingAddress.firstName + " " + objOInfo.BillingAddress.lastName);
                sbMailTemplate = sbMailTemplate.Replace("${OrderNo}", objOInfo.OrderDetail.orderDetailsId.ToString());
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATAFORCHARGES}", htmlTableForOrderChargesDetail);
                sbMailTemplate = sbMailTemplate.Replace("${CompanyName}", objOInfo.BillingAddress.companyName);
                sbMailTemplate = sbMailTemplate.Replace("${Address}", objOInfo.BillingAddress.address01 + " " + objOInfo.BillingAddress.address02);
                sbMailTemplate = sbMailTemplate.Replace("${CountryName}", objOInfo.BillingAddress.countryName);
                sbMailTemplate = sbMailTemplate.Replace("${phoneCode01}", objOInfo.UserDetail.phoneCode01);
                sbMailTemplate = sbMailTemplate.Replace("${phone01}", objOInfo.UserDetail.phone01);
                sbMailTemplate = sbMailTemplate.Replace("${emailId}", objOInfo.UserDetail.emailId);
                sbMailTemplate = sbMailTemplate.Replace("${bankName}", objOInfo.UserDetail.bankName);
                sbMailTemplate = sbMailTemplate.Replace("${branchName}", objOInfo.UserDetail.branchName);
                sbMailTemplate = sbMailTemplate.Replace("${branchAddress}", objOInfo.UserDetail.branchAddress);
                sbMailTemplate = sbMailTemplate.Replace("${accNumber}", objOInfo.UserDetail.accNumber);
                sbMailTemplate = sbMailTemplate.Replace("${swiftCode}", objOInfo.UserDetail.swiftCode);
                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, "Customer order details @ www.rosyblueonline.com", true, sbMailTemplate.ToString());
            }

        }



        //public void SendMailConfirmOrder(int OrderID, int CustomerId, string TemplatePath)
        //{
        //    List<string> lstOfEmailIDs = new List<string>();
        //    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["Email_ID"].ToString());
        //    lstOfEmailIDs.Add(ConfigurationManager.AppSettings["CCemail"].ToString());
        //    StringBuilder sbMailTemplate = new StringBuilder();
        //    sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
        //    OrderInfoViewModel objOInfo = OrderInfo(OrderID);
        //    if (objOInfo != null && objOInfo.BillingAddress != null)
        //    {
        //        //OrderInfoViewModel objOInfo = OrderInfo(OrderID);
        //        List<string> objLst = objOInfo.OrderItemDetail.Select(x => x.Stock).ToList();
        //        string LotNos = "LOTNO~" + string.Join(",", objLst);
        //        DataTable dt = this.objStockDetailsService.GetDataForExcelExport(LotNos, false, CustomerId);
        //        string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt);

        //        sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objOInfo.BillingAddress.firstName + " " + objOInfo.BillingAddress.lastName);
        //        sbMailTemplate = sbMailTemplate.Replace("${OrderNo}", OrderID.ToString());
        //        sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);
        //        if (this.objMU == null)
        //        {
        //            this.objMU = new MailUtility();
        //        }
        //        objMU.SendMail(lstOfEmailIDs, "Customer order details @ www.rosyblueonline.com", true, sbMailTemplate.ToString());
        //    }
        //}

        public DataTable OrderDetailForDownload(string OType, string OStatus, int FilterCustomerID)
        {
            DataSet ds = this.db.ExecuteCommand(("exec proc_GetOrderDetailForDownload '" + OType + "','" + OStatus + "'," + FilterCustomerID.ToString()), CommandType.Text);
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            return dt;
        }

        public MemoTallyStockByRfidViewModel MemoTallyStockByRfid(int LoginID, int OrderID, string RFIDs)
        {
            return this.uow.Orders.MemoTallyStockByRfid(LoginID, OrderID, RFIDs);
        }


        public void SendMailForApiOrderBook(int OrderID, int CustomerId, string TemplatePath, string Subject, string CCEmail, string BCCEmail, bool SentToCustomer = false)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            OrderInfoViewModel objOInfo = OrderInfo(OrderID);
            List<string> lstOfEmailIDs = new List<string>();
            if (SentToCustomer && objOInfo != null && objOInfo.UserDetail != null)
            {
                lstOfEmailIDs.Add(objOInfo.UserDetail.emailId);
            }
            else
            {
                lstOfEmailIDs.Add(ConfigurationManager.AppSettings["Email_ID"].ToString());
                lstOfEmailIDs.Add(ConfigurationManager.AppSettings["CCemail"].ToString());
            }
            if (objOInfo != null && objOInfo.BillingAddress != null)
            {
                List<string> objLst = objOInfo.OrderItemDetail.Select(x => x.Stock).ToList();
                string LotNos = "LOTNO~" + string.Join(",", objLst);
                DataTable dt = this.objStockDetailsService.GetDataForExcelExport(LotNos, false, CustomerId);
                if (dt.Columns.Count > 0)
                {
                    dt.Columns.Remove("Sizes");
                    dt.Columns.Remove("CertificateNo");
                    dt.Columns.Remove("Reportdate");
                    dt.Columns.Remove("EyeClean");
                    dt.Columns.Remove("Shade");
                    dt.Columns.Remove("TableBlack");
                    dt.Columns.Remove("SideBlack");
                    dt.Columns.Remove("Milky");
                    dt.Columns.Remove("CuletSize");
                    dt.Columns.Remove("OpensName");
                    dt.Columns.Remove("GroupName");
                    dt.Columns.Remove("MemberComments");
                    dt.Columns.Remove("refdata");
                    dt.Columns.Remove("V360");
                    dt.Columns.Remove("Video");
                    if (SentToCustomer == true)
                    {
                        dt.Columns.Remove("SalesLocation");
                    }
                }
                DataTable dtOrderCharges = objOInfo.ConvertOrderChangesInDatetable();
                string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt, false, true);
                string htmlTableForOrderChargesDetail = CommonFunction.ConvertDataTableToHTML(dtOrderCharges, false, true);
                sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objOInfo.BillingAddress.firstName + " " + objOInfo.BillingAddress.lastName);
                sbMailTemplate = sbMailTemplate.Replace("${OrderNo}", OrderID.ToString());
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATAFORCHARGES}", htmlTableForOrderChargesDetail);
                sbMailTemplate = sbMailTemplate.Replace("${CompanyName}", objOInfo.BillingAddress.companyName);
                sbMailTemplate = sbMailTemplate.Replace("${Address}", objOInfo.BillingAddress.address01 + " " + objOInfo.BillingAddress.address02);
                sbMailTemplate = sbMailTemplate.Replace("${CountryName}", objOInfo.BillingAddress.countryName);
                sbMailTemplate = sbMailTemplate.Replace("${phoneCode01}", objOInfo.UserDetail.phoneCode01);
                sbMailTemplate = sbMailTemplate.Replace("${phone01}", objOInfo.UserDetail.phone01);
                sbMailTemplate = sbMailTemplate.Replace("${emailId}", objOInfo.UserDetail.emailId);
                sbMailTemplate = sbMailTemplate.Replace("${bankName}", objOInfo.UserDetail.bankName);
                sbMailTemplate = sbMailTemplate.Replace("${branchName}", objOInfo.UserDetail.branchName);
                sbMailTemplate = sbMailTemplate.Replace("${branchAddress}", objOInfo.UserDetail.branchAddress);
                sbMailTemplate = sbMailTemplate.Replace("${accNumber}", objOInfo.UserDetail.accNumber);
                sbMailTemplate = sbMailTemplate.Replace("${swiftCode}", objOInfo.UserDetail.swiftCode);

                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, Subject, true, sbMailTemplate.ToString(), null, CCEmail, BCCEmail);
            }

        }

        public void CartItemReminderEmail(int LoginId, string EmailID, string LotNos, string Subject)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(ConfigurationManager.AppSettings["CartItemReminderEmail"].ToString()));
            List<string> lstOfEmailIDs = new List<string>();
            lstOfEmailIDs.Add(EmailID);
            if (LotNos != null && LotNos != "")
            {
                LotNos = "LOTNO~" + LotNos;
                DataTable dt = this.objStockDetailsService.GetDataForExcelExportEmail(LotNos, false, LoginId);
                if (dt.Columns.Count > 0)
                {
                    dt.Columns.Remove("Sizes");
                    dt.Columns.Remove("CertificateNo");
                    dt.Columns.Remove("Reportdate");
                    dt.Columns.Remove("EyeClean");
                    dt.Columns.Remove("Shade");
                    dt.Columns.Remove("TableBlack");
                    dt.Columns.Remove("SideBlack");
                    dt.Columns.Remove("Milky");
                    dt.Columns.Remove("CuletSize");
                    dt.Columns.Remove("OpensName");
                    dt.Columns.Remove("GroupName");
                    dt.Columns.Remove("MemberComments");
                    dt.Columns.Remove("refdata");
                    dt.Columns.Remove("V360");
                    dt.Columns.Remove("Video");
                    dt.Columns.Remove("SalesLocation");

                }
                string htmlTableForOrderDetail = CommonFunction.ConvertDataTableToHTML(dt, false, true);
                sbMailTemplate = sbMailTemplate.Replace("${TABLEDATA}", htmlTableForOrderDetail);

                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, Subject, true, sbMailTemplate.ToString());
            }

        }



    }
}
