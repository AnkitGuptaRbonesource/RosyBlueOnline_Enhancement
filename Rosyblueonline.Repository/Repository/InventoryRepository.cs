using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository
{
    public class InventoryRepository : Repository<inventoryModel>
    {

        DataContext context = null;
        public InventoryRepository(IDataContext context) : base(context)
        {
            this.context = context as DataContext;
        }

        public _SearchFilterViewModel Searchfilter(int LoginID)
        { 
            _SearchFilterViewModel objSF = new _SearchFilterViewModel();
             var cmd = context.Database.Connection.CreateCommand();
            //cmd.CommandText = "exec Proc_Searchfilter 'GetSearchfilter'";
            cmd.CommandText = "Proc_Searchfilter ";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@IN_PARAMETER_NAME", "GetSearchfilter"));
            cmd.Parameters.Add(new SqlParameter("@LoginID", LoginID));
            try
            {

                context.Database.Connection.Open();
                // Run the sproc
                var reader = cmd.ExecuteReader();

                // Read Color from the first result set
                objSF.VMcolor = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstColorViewModel>(reader, "Color", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMlab = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstLabViewModel>(reader, "Lab", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                // Read Color from the first result set
                objSF.VMfancyColor = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstfancyColorViewModel>(reader, "FancyColor", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMclarity = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstClarityViewModel>(reader, "Clarity", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();
                // Read Color from the first result set
                objSF.VMfluorescence = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstFluorescenceViewModel>(reader, "Fluorescence", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMkeyToSymbol = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstKeyToSymbolViewModel>(reader, "KeyToSymbol", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();
                // Read Color from the first result set
                objSF.VMgirdleNames = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstGirdleNamesViewModel>(reader, "GirdleNames", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMshade = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstShadeViewModel>(reader, "Shade", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();
                // Read Color from the first result set
                objSF.VMtableBlackInclusion = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstTableBlackInclusionViewModel>(reader, "TableBlack", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMsideBlackInclusion = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstSideBlackInclusionViewModel>(reader, "SideBlack", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();


                objSF.VMmilkyInclusion = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstMilkyInclusionViewModel>(reader, "Milky", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();


                objSF.VMeyeClean = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstEyeCleanViewModel>(reader, "EyeClean", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

                objSF.VMha = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<mstH_AViewModel>(reader, "HA", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();


                objSF.VMCaratsSize = ((IObjectContextAdapter)context)
                                    .ObjectContext
                                    .Translate<MstCaratsSizeViewModel>(reader, "CaratsSize", MergeOption.AppendOnly).ToList();

                // Move to second result set and read Posts
                reader.NextResult();
                objSF.VMStoneOrigin = ((IObjectContextAdapter)context)
                                  .ObjectContext
                                  .Translate<MstStoneOriginViewModel>(reader, "Origin", MergeOption.AppendOnly).ToList();


                // Move to second result set and read Posts
                reader.NextResult();


                objSF.VMSalesLocation = ((IObjectContextAdapter)context)
                                   .ObjectContext
                                   .Translate<SalesLocationViewModel>(reader).ToList();


                // Move to second result set and read Posts
                reader.NextResult();

            }
            finally
            {
                context.Database.Connection.Close();
            }

            return objSF;
        }

        public List<InventoryCountViewModel> InventoryCount(params string[] parameters)
        {
            try
            {
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_CustomSiteSearch ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginID", parameters[0]));
                cmd.Parameters.Add(new SqlParameter("@searchText", parameters[1]));
                cmd.Parameters.Add(new SqlParameter("@pageIndex", parameters[2]));
                cmd.Parameters.Add(new SqlParameter("@pageSize", parameters[3]));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", parameters[4]));
                cmd.Parameters.Add(new SqlParameter("@OrderDir", parameters[5]));
                cmd.Parameters.Add(new SqlParameter("@raisedEvent", parameters[6]));
                if (parameters.Length > 7)
                {
                    cmd.Parameters.Add(new SqlParameter("@raisedWhere", parameters[7]));
                }

                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<InventoryCountViewModel>(reader).ToList();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public SummaryCalsViewModel summaryCals(params string[] parameters)
        {
            SummaryCalsViewModel summaryvals = new SummaryCalsViewModel();
            try
            {
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_CustomSiteSearch ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginID", parameters[0]));
                cmd.Parameters.Add(new SqlParameter("@searchText", parameters[1]));
                cmd.Parameters.Add(new SqlParameter("@pageIndex", parameters[2]));
                cmd.Parameters.Add(new SqlParameter("@pageSize", parameters[3]));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", parameters[4]));
                cmd.Parameters.Add(new SqlParameter("@OrderDir", parameters[5]));
                cmd.Parameters.Add(new SqlParameter("@raisedEvent", parameters[6]));
                var reader = cmd.ExecuteReader();
                summaryvals = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<SummaryCalsViewModel>(reader).FirstOrDefault();
                return summaryvals;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public CartCountViewModel Cart(params string[] parameters)
        {
            try
            {
                var cmd = context.Database.Connection.CreateCommand();

                cmd.CommandText = "proc_cartWatch ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginId", parameters[0]));
                cmd.Parameters.Add(new SqlParameter("@tokenId", parameters[1]));
                cmd.Parameters.Add(new SqlParameter("@lotnumbers", parameters[2]));
                cmd.Parameters.Add(new SqlParameter("@raiseEvent", parameters[3]));
                context.Database.Connection.Open();
                var objDr = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<CartCountViewModel>(objDr).FirstOrDefault();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public WatchListCountViewModel Watch(params string[] parameters)
        {
            try
            {
                var cmd = context.Database.Connection.CreateCommand();

                cmd.CommandText = "proc_cartWatch ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginId", parameters[0]));
                cmd.Parameters.Add(new SqlParameter("@tokenId", parameters[1]));
                cmd.Parameters.Add(new SqlParameter("@lotnumbers", parameters[2]));
                cmd.Parameters.Add(new SqlParameter("@raiseEvent", parameters[3]));
                context.Database.Connection.Open();
                var objDr = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<WatchListCountViewModel>(objDr).FirstOrDefault();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<inventoryDetailsViewModel> GetCartandWatch(params string[] parameters)
        {
            try
            {
                List<inventoryDetailsViewModel> CartWatch = new List<inventoryDetailsViewModel>();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_cartWatch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginId", parameters[0]));
                cmd.Parameters.Add(new SqlParameter("@tokenId", parameters[1]));
                cmd.Parameters.Add(new SqlParameter("@lotnumbers", parameters[2]));
                cmd.Parameters.Add(new SqlParameter("@raiseEvent", parameters[3]));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                var resultDetails =
                ((IObjectContextAdapter)context)
                .ObjectContext
                .Translate<inventoryDetailsViewModel>(reader)
                .ToList();
              
                return resultDetails;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public CompareInventoryViewModel CompareInventory(int LoginID, string LotNos)
        {
            try
            {
                CompareInventoryViewModel CartWatch = new CompareInventoryViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_Compare";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginId", LoginID));
                cmd.Parameters.Add(new SqlParameter("@LotNos", LotNos));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                CartWatch.Count = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader).FirstOrDefault();
                reader.NextResult();
                CartWatch.Items = ((IObjectContextAdapter)context).ObjectContext.Translate<CompareViewModel>(reader).ToList();
                return CartWatch;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public DashboardViewModel DashboardView(int LoginId)
        {
            try
            {
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_DashboardView";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objVM.SpecificSearch = ((IObjectContextAdapter)context).ObjectContext.Translate<RecentSearchViewModel>(reader).ToList();
                reader.NextResult();
                objVM.SavedSearch = ((IObjectContextAdapter)context).ObjectContext.Translate<RecentSearchViewModel>(reader).ToList();
                reader.NextResult();
                objVM.DemandSearch = ((IObjectContextAdapter)context).ObjectContext.Translate<RecentSearchViewModel>(reader).ToList();
                reader.NextResult();
                objVM.counts = ((IObjectContextAdapter)context).ObjectContext.Translate<Counts>(reader).FirstOrDefault();
                reader.NextResult();
                objVM.criteria = ((IObjectContextAdapter)context).ObjectContext.Translate<Criteria>(reader).FirstOrDefault();
                reader.NextResult();
                objVM.stocks = ((IObjectContextAdapter)context).ObjectContext.Translate<StockStatus>(reader).ToList();
                reader.NextResult();
                objVM.RecentSearch = ((IObjectContextAdapter)context).ObjectContext.Translate<CustomerRecentSearch>(reader).ToList();
                reader.NextResult(); 
                objVM.DemandList = ((IObjectContextAdapter)context).ObjectContext.Translate<DemandList>(reader).ToList();

                return objVM;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<RecentSearchViewModel> RecentSearchView(int LoginId)
        {
            try
            {
                List<RecentSearchViewModel> objLst = new List<RecentSearchViewModel>();
                DashboardViewModel objVM = new DashboardViewModel();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_RecentSearchView";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objLst = ((IObjectContextAdapter)context).ObjectContext.Translate<RecentSearchViewModel>(reader).ToList();
                return objLst;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<StockSummaryViewModel> StockSummary(string Shape, string LocationID)
        {
            try
            {
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_StockSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Shape", Shape));
                cmd.Parameters.Add(new SqlParameter("@salesLocationIDs", LocationID));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                var resultDetails =
                ((IObjectContextAdapter)context)
                .ObjectContext
                .Translate<StockSummaryViewModel>(reader)
                .ToList();
                return resultDetails;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }


        //added byankit 11July2020
        public List<StoneDetailsStockSummaryModel> StoneDetailsStockSummary(string StartRange,string EndRange,string Shape,string salesLocationIDs,string SearchMode,string RaisedEvent)
        {
            try
            {
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_StoneDetailsStockSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;
                cmd.Parameters.Add(new SqlParameter("@StartRange", StartRange));
                cmd.Parameters.Add(new SqlParameter("@EndRange", EndRange));
                cmd.Parameters.Add(new SqlParameter("@Shape", Shape));
                cmd.Parameters.Add(new SqlParameter("@salesLocationIDs", salesLocationIDs));
                cmd.Parameters.Add(new SqlParameter("@SearchMode", SearchMode));
                cmd.Parameters.Add(new SqlParameter("@RaisedEvent", RaisedEvent)); 
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                var resultDetails =
                ((IObjectContextAdapter)context)
                .ObjectContext
                .Translate<StoneDetailsStockSummaryModel>(reader)
                .ToList();
             //   ((IObjectContextAdapter)context)
             //.ObjectContext.CommandTimeout = 180;
                return resultDetails;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
        public List<mstUploadFormatViewModel> InventoryUploadTypes(params string[] parameters)
        {
            try
            {
                context.Database.Connection.Open();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "RB_GET_PARAMETER_MASTER ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IN_PARAMETER_NAME", parameters[0]));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<mstUploadFormatViewModel>(reader).ToList();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<InventoryUpload> InventoryUpload(DataTable dt, params string[] parameters)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = parameters[0].ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 900;
                cmd.Parameters.Add(new SqlParameter("@inventoryUpload", dt));
                cmd.Parameters.Add(new SqlParameter("@createdby", Convert.ToInt32(parameters[1])));
                cmd.Parameters.Add(new SqlParameter("@FileID", Convert.ToInt32(parameters[2])));
                cmd.Parameters.Add(new SqlParameter("@LastUpdateIP", parameters[3]));
                cmd.Parameters.Add(new SqlParameter("@raiseEvent", parameters[4]));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<InventoryUpload>(reader).ToList();


            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public StockStatusViewModel StoneStatus(DataTable dt, int LoginID, decimal pageIndex, int pageSize, string RaisedEvent,string OrderBy,string OrderDirection)
        {
            try
            {
                dt.Columns[0].ColumnName = "Lotnumber";
                StockStatusViewModel obj = new StockStatusViewModel();
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_StoneStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@tblLotNumbers", dt));
                cmd.Parameters.Add(new SqlParameter("@LoginID", LoginID));
                cmd.Parameters.Add(new SqlParameter("@pageIndex", pageIndex));
                cmd.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                cmd.Parameters.Add(new SqlParameter("@RaisedEvent", RaisedEvent));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", OrderBy));
                cmd.Parameters.Add(new SqlParameter("@OrderDirection", OrderDirection));
                var reader = cmd.ExecuteReader();

                obj.TotalCount = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<int>(reader).FirstOrDefault();

                reader.NextResult();

                obj.inventories = ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<inventoryDetailsViewModel>(reader).ToList();

                return obj;

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<RFIDTempTableMiewModel> InventoryUploadRFID(DataTable dt, int LoginID, string Ipaddress, DateTime dateofUpdate, int fileId)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "InventoryUpdate_RFID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@tblRFIDupdate", dt));
                cmd.Parameters.Add(new SqlParameter("@loginId", LoginID));
                cmd.Parameters.Add(new SqlParameter("@Ipaddress", Ipaddress));
                cmd.Parameters.Add(new SqlParameter("@dateofUpdate", dateofUpdate));
                cmd.Parameters.Add(new SqlParameter("@fileId", fileId));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context).ObjectContext.Translate<RFIDTempTableMiewModel>(reader).ToList();

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<RFIDstockmaster> RFIDstockTally(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "prcRFIdstockTally";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@stockId", stockId));
                cmd.Parameters.Add(new SqlParameter("@RFId", RFId));
                cmd.Parameters.Add(new SqlParameter("@boxName", boxName));
                cmd.Parameters.Add(new SqlParameter("@Rfidmachine", Rfidmachine));
                cmd.Parameters.Add(new SqlParameter("@loginid", loginid));
                cmd.Parameters.Add(new SqlParameter("@Boxid", Boxid));
                cmd.Parameters.Add(new SqlParameter("@RaiseEvent", RaiseEvent));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<RFIDstockmaster>(reader).ToList();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<RFIDexportBox> RFIdstockBoxExport(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "prcRFIdstockTally";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@stockId", stockId));
                cmd.Parameters.Add(new SqlParameter("@RFId", RFId));
                cmd.Parameters.Add(new SqlParameter("@boxName", boxName));
                cmd.Parameters.Add(new SqlParameter("@Rfidmachine", Rfidmachine));
                cmd.Parameters.Add(new SqlParameter("@loginid", loginid));
                cmd.Parameters.Add(new SqlParameter("@Boxid", Boxid));
                cmd.Parameters.Add(new SqlParameter("@RaiseEvent", RaiseEvent));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<RFIDexportBox>(reader).ToList();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int BestDeals(DataTable dt, string inventoryIds, decimal discountPercentBD, string Remark, int fileId, string TokenID, int createdby, string RaiseEvent)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_BestDeals";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@bestDeals", dt));
                cmd.Parameters.Add(new SqlParameter("@inventoryIds", inventoryIds));
                cmd.Parameters.Add(new SqlParameter("@discountPercentBD", discountPercentBD));
                cmd.Parameters.Add(new SqlParameter("@Remark", Remark));
                cmd.Parameters.Add(new SqlParameter("@fileId", fileId));
                cmd.Parameters.Add(new SqlParameter("@TokenID", TokenID));
                cmd.Parameters.Add(new SqlParameter("@createdby", createdby));
                cmd.Parameters.Add(new SqlParameter("@RaiseEvent", RaiseEvent));

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public int AddRemoveLabStatus(DataTable dt, int fileId, string RaiseEvent)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_AddRemoveLabInventory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@LotNos", dt));
                cmd.Parameters.Add(new SqlParameter("@FileID", fileId));
                cmd.Parameters.Add(new SqlParameter("@raiseEvent", RaiseEvent));

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public List<InventoryUpload> UpdateV360ViaCertNo(DataTable dt, int fileId)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_UpdateV360ViaCertNo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@inventoryUpload", dt));
                cmd.Parameters.Add(new SqlParameter("@FileID", fileId));
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<InventoryUpload>(reader).ToList();
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public decimal GetAvgRapOff(string LotNos)
        {
            try
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                {
                    context.Database.Connection.Open();
                }
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = string.Format("Select dbo.fnGetAvgRapOff '{0}'", LotNos);
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                return ((IObjectContextAdapter)context)
                    .ObjectContext
                    .Translate<decimal>(reader).FirstOrDefault();
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


        public ORRAStockDetailsValidate ORRAStockDetailsValidate(int LoginID,string  LotNos, string RaiseEvent)
        {
            try
            {
                ORRAStockDetailsValidate objVM = new ORRAStockDetailsValidate();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_ValidateAPIInventory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginID", LoginID));
                cmd.Parameters.Add(new SqlParameter("@LotNos", LotNos));
                cmd.Parameters.Add(new SqlParameter("@raisedEvent", RaiseEvent));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objVM.StockDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<ORRAStockDetailsModel>(reader).ToList();
                reader.NextResult();
                reader.NextResult();
                objVM.OrderDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<PlaceOrderOrra>(reader).ToList();

                return objVM; 

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }


        public TanishqStockDetailsValidate TanishqStockDetailsValidate(int LoginID, string LotNos, string RaiseEvent)
        {
            try
            {
                TanishqStockDetailsValidate objVM = new TanishqStockDetailsValidate();
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "proc_ValidateAPIInventory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@loginID", LoginID));
                cmd.Parameters.Add(new SqlParameter("@LotNos", LotNos));
                cmd.Parameters.Add(new SqlParameter("@raisedEvent", RaiseEvent));
                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                objVM.StockDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<TanishqStockModel>(reader).ToList();
                reader.NextResult();
                reader.NextResult();
                objVM.OrderDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<TanishqPlaceOrder>(reader).ToList();

                return objVM;

            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

    }
}
