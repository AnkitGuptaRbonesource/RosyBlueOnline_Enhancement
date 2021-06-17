using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository
{
    public class OrderRepository : Repository<orderDetailModel>
    {
        DataContext context = null;
        public OrderRepository(IDataContext context) : base(context)
        {
            this.context = context as DataContext;
        }

        public OrderViewModel PreBookOrder(string LotNos, int CustomerId, int ShippingMode)
        {
            OrderViewModel objCharges = new OrderViewModel();
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_PlaceOrder '" + LotNos + "',0," + CustomerId.ToString() + "," + ShippingMode.ToString() + ",0,0,0,'',0,'preBookOrder'";
            try
            {

                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();

                // Read Color from the first result set
                objCharges.Inventory = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<inventoryDetailsViewModel>(reader).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objCharges.Summary = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<SummaryCalsViewModel>(reader).FirstOrDefault();


                // Move to second result set and read Posts
                reader.NextResult();

                // Read Color from the first result set
                objCharges.Charges = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<OrderChargesViewModel>(reader).ToList();


            }
            finally
            {
                context.Database.Connection.Close();
            }

            return objCharges;
        }

        public PlaceOrderViewModel BookOrder(string LotNos, int CreatedBy, int CustomerId, int ShippingMode, int BillingID, int ShippingID)
        {
            try
            {
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_PlaceOrder '" + LotNos + "'," + CreatedBy.ToString() + "," + CustomerId.ToString() + "," +
                    ShippingMode.ToString() + ",0," + BillingID.ToString() + "," + ShippingID.ToString() + ",'',0,'BookOrder'";
                //cmd.CommandText = "proc_PlaceOrder";
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@lotnos", LotNos));
                cmd.Parameters.Add(new SqlParameter("@createdBy", CreatedBy));
                cmd.Parameters.Add(new SqlParameter("@customerId", CustomerId));
                cmd.Parameters.Add(new SqlParameter("@ShippingMode", ShippingMode));
                cmd.Parameters.Add(new SqlParameter("@orderType", "0"));
                cmd.Parameters.Add(new SqlParameter("@billingId", BillingID));
                cmd.Parameters.Add(new SqlParameter("@shippingId", ShippingID));
                cmd.Parameters.Add(new SqlParameter("@remark", ""));
                cmd.Parameters.Add(new SqlParameter("@loginDeviceId", "0"));
                cmd.Parameters.Add(new SqlParameter("@raisedEvent", "BookOrder"));
                context.Database.Connection.Open();
                var objDr = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<PlaceOrderViewModel>(objDr).FirstOrDefault();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
        public IQueryable<WS_SchedulerModel> SchedulerList()
        {
            return this.context.WS_Scheduler.AsQueryable();
        }
        //Added by Ankit 19Jun2020
        //public IQueryable<UserGeoLocationModel> UserActivityLog()
        //{
        //    return this.context.UserGeoLocation.AsQueryable();
        //}

        public IQueryable<UserActivityLogModel> CustomerActivityLog()
        {
            return this.context.UserActivityLog.AsQueryable();
        }

        public bool UserActivityloginsert(int Loginid, string Actionname, string Actiondetail)
        {
            try
            {
                List<UserActivityLogModel> objLst = new List<UserActivityLogModel>();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Proc_UserActivityLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", Loginid));
                cmd.Parameters.Add(new SqlParameter("@Flag", "Insert"));
                cmd.Parameters.Add(new SqlParameter("@ActionName", Actionname));
                cmd.Parameters.Add(new SqlParameter("@ActionDetails", Actiondetail));
                context.Database.Connection.Open();
                // var reader = cmd.ExecuteReader();
                var reader = cmd.ExecuteNonQuery();

                // objLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UserActivityLogModel>(reader).ToList();
                return objLst != null ? true : false;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
        public List<UserGeoLocationModel> UserGeoLoctionLog(int LoginId)
        {
            try
            {
                List<UserGeoLocationModel> objLst = new List<UserGeoLocationModel>();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Proc_UserActivityLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                cmd.Parameters.Add(new SqlParameter("@Flag", "Locationlog"));
                cmd.Parameters.Add(new SqlParameter("@ActionName", ""));
                cmd.Parameters.Add(new SqlParameter("@ActionDetails", ""));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UserGeoLocationModel>(reader).ToList();
                return objLst;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<UserActivityLogModel> GetCustomerLogData(int LoginId)
        {
            try
            {
                List<UserActivityLogModel> objLst = new List<UserActivityLogModel>();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Proc_UserActivityLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                cmd.Parameters.Add(new SqlParameter("@Flag", "Activitylog"));
                cmd.Parameters.Add(new SqlParameter("@ActionName", ""));
                cmd.Parameters.Add(new SqlParameter("@ActionDetails", ""));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UserActivityLogModel>(reader).ToList();
                return objLst;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public UserActivityLogModel GetPasswordResetLogData(int LoginId)
        {
            try
            {
                 UserActivityLogModel  objLsts = new  UserActivityLogModel();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Proc_UserActivityLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                cmd.Parameters.Add(new SqlParameter("@Flag", "PasswordResetLog"));
                cmd.Parameters.Add(new SqlParameter("@ActionName", ""));
                cmd.Parameters.Add(new SqlParameter("@ActionDetails", ""));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLsts = ((IObjectContextAdapter)context).ObjectContext.Translate<UserActivityLogModel>(reader).FirstOrDefault();
                return objLsts;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public UserActivityLogModel GetPasswordLogCheck(int LoginId)
        {
            try
            {
                UserActivityLogModel objLsts = new UserActivityLogModel();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Proc_UserActivityLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                cmd.Parameters.Add(new SqlParameter("@Flag", "PasswordLogCheck"));
                cmd.Parameters.Add(new SqlParameter("@ActionName", ""));
                cmd.Parameters.Add(new SqlParameter("@ActionDetails", ""));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLsts = ((IObjectContextAdapter)context).ObjectContext.Translate<UserActivityLogModel>(reader).FirstOrDefault();
                return objLsts;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public UserMenuAccessModel GetUserMenuAccessDetails(int Loginid, string MenuIdList, string CreatedBy, string QFlag)
        {
            try
            {
                UserMenuAccessModel objLst = new UserMenuAccessModel();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "Pro_GetMenuAccessDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", Loginid));
                cmd.Parameters.Add(new SqlParameter("@MenuIdList", MenuIdList));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
                cmd.Parameters.Add(new SqlParameter("@QFlag", QFlag));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UserMenuAccessModel>(reader).FirstOrDefault();
                return objLst;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
        public IQueryable<OrderListView> OrderListView()
        {
            return this.context.OrderListViews.AsQueryable();
        }

        public OrderInfoViewModel OrderInfo(int OrderID)
        {
            OrderInfoViewModel objInfo = new OrderInfoViewModel();
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_OrderInfo  '" + OrderID.ToString() + "'";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();

                // Read OrderDetail from the first result set
                objInfo.OrderDetail = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<orderDetailModel>(reader).FirstOrDefault();


                // Move to second result set and read inventory
                reader.NextResult();

                objInfo.OrderItemDetail = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<inventoryDetailsViewModel>(reader).ToList();


                // Move to third result set and read BillingAddress
                reader.NextResult();

                objInfo.BillingAddress = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<MstBillingAddressViewModel>(reader).FirstOrDefault();


                // Move to fourth result set and read OrderCharges
                reader.NextResult();

                objInfo.Charges = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<OrderChargesViewModel>(reader).ToList();


                // Move to fifth result set and read OrderCharges
                reader.NextResult();

                objInfo.UserDetail = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<UserDetailModel>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }

            return objInfo;
        }

        public int OrderPartialCancel(int OrderID, string LotNo, int LoginID, int CustomerID)
        {
            int RowCount = 0;
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_OrderConfirmation  '" + OrderID.ToString() + "','" + LotNo + "'," + LoginID.ToString() + "," + CustomerID.ToString() + ",'','','PartialCancel'";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return RowCount;
        }

        public int OrderCancel(int OrderID, int LoginID, int CustomerID)
        {
            int RowCount = 0;
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_OrderConfirmation  '" + OrderID.ToString() + "',''," + LoginID.ToString() + "," + CustomerID.ToString() + ",'','','FullyCancel'";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return RowCount;
        }

        public int OrderComplete(int OrderID, int LoginID, int CustomerID, string ShippingCompany, string TrackingNumber)
        {
            int RowCount = 0;
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_OrderConfirmation  '" + OrderID.ToString() + "',''," + LoginID.ToString() + "," + CustomerID.ToString() + ",'" + ShippingCompany + "','" + TrackingNumber + "','Confirm'";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return RowCount;
        }

        public List<inventoryDetailsViewModel> GetOrderItemsByOrderID(string OrderID, int UserID)
        {
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_GetOrderItemDetail '" + OrderID + "'," + UserID.ToString();
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<inventoryDetailsViewModel>(reader).ToList();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int OrderMerge(int LoginID, int CustomerID, string MergeOrderList)
        {
            int RowCount = 0;
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = "exec proc_OrderConfirmation 0,''," + LoginID.ToString() + "," +
                CustomerID.ToString() + ",'','','MergeOrder','" + MergeOrderList + "'";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return RowCount;
        }

        #region Memo
        public int CreateMemo(string LotNos, int LoginID, int CustomerID, bool IsSellDirect, int MemoMode, decimal SalesAvgDiscount, string Remark)
        {
            int RowCount = 0;
            var cmd = context.Database.Connection.CreateCommand();
            // @orderDetailsId,@MemoID,@LotNos,@LoginId,@CustomerID,@isConfirmed,@isSellDirect,@memoMode,@salesAvgDiscount,@remark,@RaiseEvent
            cmd.CommandText = "exec proc_inventoryMemo  0,0,'" + LotNos + "'," + LoginID.ToString() + "," + CustomerID.ToString() + ",0," + Convert.ToInt32(IsSellDirect).ToString() + "," + MemoMode + "," + SalesAvgDiscount.ToString() + ",''";
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return RowCount;
        }

        public MemoTallyStockByRfidViewModel MemoTallyStockByRfid(int LoginID, int OrderID, string RFIDs)
        {
            MemoTallyStockByRfidViewModel objTS = new MemoTallyStockByRfidViewModel();
            var cmd = context.Database.Connection.CreateCommand();
            // @orderDetailsId,@MemoID,@LotNos,@LoginId,@CustomerID,@isConfirmed,@isSellDirect,@memoMode,@salesAvgDiscount,@remark,@RaiseEvent
            cmd.CommandText = string.Format("exec proc_MemoTallyStockByRfid  {0},{1},'{2}'", LoginID.ToString(), OrderID.ToString(), RFIDs);
            try
            {
                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();

                // Read OrderDetail from the first result set
                objTS.InMemo = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<inventoryDetailsViewModel>(reader).ToList();

                reader.NextResult();

                objTS.InBox = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<inventoryDetailsViewModel>(reader).ToList();
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return objTS;
        }


        #endregion
    }
}
