using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;

namespace Rosyblueonline.Repository
{
    public class MemoRepository : Repository<MemoDetailModel>
    {
        readonly DataContext context = null;
        public MemoRepository(IDataContext context) : base(context)
        {
            this.context = context as DataContext;
        }

        public MemoDetail CreateMemo(string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark)
        {
            try
            {
                MemoDetail objMD = new MemoDetail();

                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo 0,'" + LotNos + "'," +
                                LoginID.ToString() + "," + CustomerID.ToString() + "," +
                                isConfirmed.ToString() + "," + isSellDirect + ",0,0,'" +
                                Remark + "','putOnMemo'";

                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();

                objMD.Inv = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<InventoryUpload>(reader).ToList();

                reader.NextResult();

                objMD.Counts = ((IObjectContextAdapter)context)
                                .ObjectContext
                                .Translate<PlaceOrderViewModel>(reader).FirstOrDefault();

                return objMD;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public MemoDetail CancelPartialMemo(int OrderID, string LotNos, int LoginID)
        {
            try
            {
                MemoDetail objMD = new MemoDetail();
                //int RowCount = 0;
                context.Database.Connection.Open();
                int CustomerID = this.context.orderDetails.Where(x => x.orderDetailsId == OrderID).Select(x => x.customerId).FirstOrDefault();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo " + OrderID.ToString() + ",'" + LotNos + "'," +
                                LoginID.ToString() + "," + CustomerID.ToString() + ",0,0,0,0,'','cancelPartialMemo'";


                //// Run the sproc
                //var reader = cmd.ExecuteReader();
                //// Read OrderDetail from the first result set
                //RowCount = ((IObjectContextAdapter)context)
                //    .ObjectContext
                //    .Translate<int>(reader).FirstOrDefault();
                //return RowCount;

                var reader = cmd.ExecuteReader();

                objMD.Inv = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<InventoryUpload>(reader).ToList();

                reader.NextResult();

                objMD.Counts = ((IObjectContextAdapter)context)
                                .ObjectContext
                                .Translate<PlaceOrderViewModel>(reader).FirstOrDefault();

                return objMD;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int SplitMemo(int OrderID, string LotNos, int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark)
        {
            try
            {
                int newOrderID = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo " + OrderID.ToString() + ",'" + LotNos + "'," +
                                LoginID.ToString() + "," + CustomerID.ToString() + "," + isConfirmed.ToString() + "," + isSellDirect.ToString() + ",0,0,'" + Remark + "','SplitMemo'";


                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                newOrderID = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
                return newOrderID;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int MergeMemo(int LoginID, int CustomerID, int isConfirmed, int isSellDirect, string Remark, string MergeOrderList)
        {
            try
            {
                int RowCount = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo 0,''," +
                                LoginID.ToString() + "," + CustomerID.ToString() + "," + isConfirmed.ToString() + "," + isSellDirect.ToString() + ",0,0,'" + Remark + "','MergeMemo','" + MergeOrderList + "'";


                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
                return RowCount;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int SellFullMemo(int OrderID, int LoginID, int CustomerID, int MemoMode, decimal salesAvgDiscount)
        {
            try
            {
                int RowCount = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo " + OrderID.ToString() +
                    ",''," + LoginID.ToString() + "," + CustomerID + ",0,0," + MemoMode.ToString() + "," + salesAvgDiscount.ToString("N2")
                    + ",'','SellFullMemo'";


                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
                return RowCount;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int CancelFullMemo(int OrderID, int CustomerID, int LoginID)
        {
            try
            {
                int RowCount = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_inventoryMemo " + OrderID.ToString() + ",''," + LoginID.ToString() + "," + CustomerID + ",0,0,0,0,'','cancelFullMemo'";


                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
                return RowCount;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int MemoReturnSale(string OrderIDs, int UserID)
        {
            try
            {
                int RowCount = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_MemoReturnSale '" + OrderIDs.ToString() + "'," + UserID.ToString();
                // Run the sproc
                var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                RowCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();
                return RowCount;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int MemoPartialReturnSale(string LotNos, int LoginId)
        {
            try
            {
                //int RowCount = 0;
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "exec proc_MemoPartialReturnSale '" + LotNos.ToString() + "'," + LoginId.ToString();
                // Run the sproc
                //var reader = cmd.ExecuteReader();
                // Read OrderDetail from the first result set
                //RowCount = ((IObjectContextAdapter)context)
                //    .ObjectContext
                //    .Translate<int>(reader).FirstOrDefault();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

    }
}
