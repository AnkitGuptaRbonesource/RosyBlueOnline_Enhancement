using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.Adapters;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using System.Web.Mvc;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.UnitOfWork;
using System.Data;
using Rosyblueonline.Repository.Context;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class StockDetailsService : IStockDetailsService
    {
        readonly UnitOfWork uow = null;
        readonly DBSQLServer dBSQL = null;
        MailUtility objMU = null;
        public StockDetailsService(IUnitOfWork uow, IDBSQLServer dBSQL)
        {
            this.uow = uow as UnitOfWork;
            this.dBSQL = dBSQL as DBSQLServer;
        }

        #region StockDetails
        public _SearchFilterViewModel SearchFilter(int LoginID)
        {
            _SearchFilterViewModel objSearchFilterViewModel = new _SearchFilterViewModel();
            return this.uow.Inventory.Searchfilter(LoginID);
        }
        public List<InventoryCountViewModel> InventoryCount(params string[] parameters)
        {
            return this.uow.Inventory.InventoryCount(parameters);
        }
        public List<inventoryDetailsViewModel> InventoryAction(params string[] Parameters)
        {
            if (Parameters.Length > 7)
            {
                return this.uow.ExecuteQuery<inventoryDetailsViewModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}'", Parameters);
            }
            else
            {
                return this.uow.ExecuteQuery<inventoryDetailsViewModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','NormalSearch'", Parameters);
            }
        }

        public List<inventoryDetailsViewModel> GetInventoriesByLotID(int LoginID, string SearchText)
        {
            return this.uow.ExecuteQuery<inventoryDetailsViewModel>("Exec proc_CustomSiteSearch " + LoginID.ToString() + ",'LOTNO~" + SearchText + "',0,10000,'LotNumber','asc','SpecificSearch','SpecialSearch'");
        }
        public SummaryCalsViewModel SummaryCalculation(params string[] parameters)
        {
            SummaryCalsViewModel objSearchFilterViewModel = new SummaryCalsViewModel();
            return this.uow.Inventory.summaryCals(parameters);
        }

        public List<T> InventoryDownload<T>(params string[] Parameters) where T : class
        {
            if (Parameters.Length-1 > 7)
            {
                return this.uow.ExecuteQuery<T>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}','{8}'", Parameters);

            }
            else if (Parameters.Length >6)
            {
                return this.uow.ExecuteQuery<T>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}'", Parameters);
            }
            else
            {
                return this.uow.ExecuteQuery<T>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}'", Parameters);
            }
        }



        public List<InventoryDownloadMemoViewModel> InventoryDownloadMemo(params string[] Parameters)
        {
            if (Parameters.Length > 7)
            {
                return this.uow.ExecuteQuery<InventoryDownloadMemoViewModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}'", Parameters);
            }
            else
            {
                return this.uow.ExecuteQuery<InventoryDownloadMemoViewModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}'", Parameters);
            }
        }

        #endregion

        #region CartAndWatchList

        public CartCountViewModel Cart(params string[] Parameters)
        {
            return this.uow.Inventory.Cart(Parameters);
        }
        public WatchListCountViewModel Watch(params string[] Parameters)
        {
            return this.uow.Inventory.Watch(Parameters);
        }
        public List<inventoryDetailsViewModel> GetCartandWatch(params string[] Parameters)
        {
            return this.uow.Inventory.GetCartandWatch(Parameters);
        }
        public CompareInventoryViewModel CompareInventory(int LoginID, string LotNos)
        {
            return this.uow.Inventory.CompareInventory(LoginID, LotNos);
        }

        #endregion

        #region Memo

        #endregion

        public DashboardViewModel DashboardView(int LoginId)
        {
            return uow.Inventory.DashboardView(LoginId);
        }

        public List<RecentSearchViewModel> RecentSearchView(int LoginId)
        {
            return uow.Inventory.RecentSearchView(LoginId);
        }

        public List<StockSummaryViewModel> StockSummary(string Shape, string LocationID)
        {
            return uow.Inventory.StockSummary(Shape, LocationID);
        }
        //Added by Ankit 11July2020
        public List<StoneDetailsStockSummaryModel> StoneDetailsStockSummary(string StartRange, string EndRange, string Shape, string salesLocationIDs, string SearchMode, string RaisedEvent)
        {
            return uow.Inventory.StoneDetailsStockSummary(StartRange, EndRange, Shape, salesLocationIDs, SearchMode, RaisedEvent);
        } 

        public List<MstSalesLocationModel> SalesLocationActive()
        {
            return uow.MstSalesLocation.Queryable().Where(x => x.isActive == true).ToList();
        }

        #region InventoryUpload
        public List<mstUploadFormatViewModel> InventoryUploadTypes(params string[] parameters)
        {
            return this.uow.Inventory.InventoryUploadTypes(parameters);
        }

        public int InsertFileUploadLog(params string[] parameters)
        {
            int fileId = 0;
            fileUploadLogModel objFU = new fileUploadLogModel();
            this.uow.BeginTransaction();
            try
            {
                objFU.fileName = parameters[0].ToString();
                objFU.filePath = parameters[1].ToString();
                objFU.createdBy = Convert.ToInt32(parameters[2]);
                objFU.createdOn = DateTime.Now;
                objFU.completedOn = DateTime.Now;
                objFU.ipAddress = parameters[3].ToString();
                objFU.uploadType = Convert.ToInt32(parameters[4]);
                objFU.refData = Convert.ToString(parameters[5]);
                objFU.uploadStatus = 1;
                objFU.validInv = 0;
                objFU.invalidInv = 0;
                this.uow.FileUploadLog.Add(objFU);
                if (this.uow.Save() > 0)
                {
                    fileId = objFU.fileId;
                }
                this.uow.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.uow.RollbackTransaction();
                throw ex;
            }

            return fileId;
        }

        public List<InventoryUpload> InventoryUpload(DataTable dt, params string[] parameters)
        {
            return this.uow.Inventory.InventoryUpload(dt, parameters);
        }

        public IQueryable<FileUploadLogViewModel> QueryableFileUploadLog()
        {
            return from ful in this.uow.FileUploadLog.Queryable()
                   join uf in this.uow.MstUploadFormat.Queryable() on ful.refData equals uf.uploadValue
                   select new FileUploadLogViewModel
                   {
                       fileId = ful.fileId,
                       fileName = ful.fileName,
                       filePath = ful.filePath,
                       createdBy = ful.createdBy,
                       createdOn = ful.createdOn,
                       completedOn = ful.completedOn,
                       ipAddress = ful.ipAddress,
                       uploadType = ful.uploadType,
                       refData = ful.refData,
                       uploadStatus = ful.uploadStatus,
                       validInv = ful.validInv,
                       invalidInv = ful.invalidInv,
                       uploadFullName = uf.uploadFullName
                   };

        }

        public List<InventoryUpload> UpdateV360ViaCertNo(DataTable dt, int fileId)
        {
            return this.uow.Inventory.UpdateV360ViaCertNo(dt, fileId);
        }
        #endregion

        #region Stock Status
        public StockStatusViewModel StockStatus(DataTable dt, int LoginID, decimal pageIndex, int pageSize, string RaisedEvent, string OrderBy, string OrderDirection)
        {
            return this.uow.Inventory.StoneStatus(dt, LoginID, pageIndex, pageSize, RaisedEvent, OrderBy, OrderDirection);
        }

        public DataSet StockStatus(DataTable dt, int LoginID, string RaisedEvent, string OrderBy, string OrderDirection)
        {
            this.dBSQL.Parameters.Clear();
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@tblLotNumbers", dt));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LoginID", LoginID));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageIndex", "0"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageSize", "500000"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RaisedEvent", RaisedEvent));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderBy", OrderBy));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderDirection", OrderDirection));
            DataSet ds = this.dBSQL.ExecuteCommand("proc_StoneStatusForExport", CommandType.StoredProcedure);
            return ds;
        }


        #endregion


        public DataTable SpecificSearchDownloadExcelExport(string filterText, bool NewArrival, int LoginID, bool IsSpecialSearch = true, bool IsOnlyMemo = false, string PermissibleDownload="NO")
        {
            //string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
            List<SpecificSearchDownloadViewModel> objInvVM = new List<SpecificSearchDownloadViewModel>();
            
            if (IsOnlyMemo == true)
            {
                objInvVM = InventoryDownload<SpecificSearchDownloadViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "SpecificSearchDownload", "OnlyMemo", PermissibleDownload);
            }
            else
            {

                objInvVM = NewArrival == false ? InventoryDownload<SpecificSearchDownloadViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "SpecificSearchDownload", (IsSpecialSearch ? "SpecialSearch" : "NormalSearch"), PermissibleDownload) :
                                                  InventoryDownload<SpecificSearchDownloadViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "SpecificSearchDownload", "NewArrival", PermissibleDownload);

            }
            DataTable dt = Rosyblueonline.Framework.ListtoDataTable.ToDataTable<SpecificSearchDownloadViewModel>(objInvVM);
            //dt.Columns["LotNumber"].ColumnName = "Stock";
            //dt.Columns["Carat"].ColumnName = "Weight";
            //dt.Columns["Measurement"].ColumnName = "Sizes";
            //dt.Columns["diaLength"].ColumnName = "Length";
            //dt.Columns["diaWidth"].ColumnName = "Width";
            //dt.Columns["diaDepth"].ColumnName = "Depth";
            //dt.Columns["HA"].ColumnName = "HeartAndArrows";
            dt.Columns["Cut"].ColumnName = "Cut Grade";
            //dt.Columns["Fluorescence"].ColumnName = "Fluorescence_Intensity";
            //dt.Columns["Rap"].ColumnName = "Rapnet_Price";
            dt.Columns["RapAmount"].ColumnName = "Rap Amount";
            dt.Columns["Discount"].ColumnName = "Rapnet Discount %";
            dt.Columns["Price"].ColumnName = "Pricect";
            dt.Columns["CertificateNo"].ColumnName = "Certificate_#";
            dt.Columns["DepthPerc"].ColumnName = "Depth %";
            dt.Columns["TablePerc"].ColumnName = "Table %";
            //dt.Columns["Girdle"].ColumnName = "Girdle Thin";
            dt.Columns["CrownHeight"].ColumnName = "Crown Height";
            dt.Columns["CrownAngle"].ColumnName = "Crown Angle";
            dt.Columns["PavilionDepth"].ColumnName = "Pavilion Depth";
            dt.Columns["PavilionAngle"].ColumnName = "Pavilion Angle";
            dt.Columns["StarLength"].ColumnName = "StarLength";
            dt.Columns["GirdlePerc"].ColumnName = "Girdle %";
            dt.Columns["Keytosymbol"].ColumnName = "Keytosymbol";
            //dt.Columns["SalesLocation"].ColumnName = "SalesLocation";
            return dt;
        }



        public DataTable GetDataForExcelExport(string filterText, bool NewArrival, int LoginID, bool IsSpecialSearch = true, bool IsOnlyMemo = false )
        {
            //string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
            List<InventoryDownloadViewModel> objInvVM = new List<InventoryDownloadViewModel>();

           

           objInvVM = NewArrival == false ? InventoryDownload<InventoryDownloadViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "EmailSearch", (IsSpecialSearch ? "SpecialSearch" : "NormalSearch")) :
                                                  InventoryDownload<InventoryDownloadViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "EmailSearch", "NewArrival");
             
            DataTable dt = Rosyblueonline.Framework.ListtoDataTable.ToDataTable<InventoryDownloadViewModel>(objInvVM);
            //dt.Columns["LotNumber"].ColumnName = "Stock";
            //dt.Columns["Carat"].ColumnName = "Weight";
            //dt.Columns["Measurement"].ColumnName = "Sizes";
            //dt.Columns["diaLength"].ColumnName = "Length";
            //dt.Columns["diaWidth"].ColumnName = "Width";
            //dt.Columns["diaDepth"].ColumnName = "Depth";
            //dt.Columns["HA"].ColumnName = "HeartAndArrows";
            dt.Columns["Cut"].ColumnName = "Cut Grade";
            //dt.Columns["Fluorescence"].ColumnName = "Fluorescence_Intensity";
            //dt.Columns["Rap"].ColumnName = "Rapnet_Price";
            dt.Columns["RapAmount"].ColumnName = "Rap Amount";
            dt.Columns["Discount"].ColumnName = "Rapnet Discount %";
            dt.Columns["Price"].ColumnName = "Pricect";
            //dt.Columns["CertificateNo"].ColumnName = "Certificate_#";
            dt.Columns["DepthPerc"].ColumnName = "Depth %";
            dt.Columns["TablePerc"].ColumnName = "Table %";
            //dt.Columns["Girdle"].ColumnName = "Girdle Thin";
            dt.Columns["CrownHeight"].ColumnName = "Crown Height";
            dt.Columns["CrownAngle"].ColumnName = "Crown Angle";
            dt.Columns["PavilionDepth"].ColumnName = "Pavilion Depth";
            dt.Columns["PavilionAngle"].ColumnName = "Pavilion Angle";
            dt.Columns["StarLength"].ColumnName = "StarLength";
            dt.Columns["GirdlePerc"].ColumnName = "Girdle %";
            dt.Columns["Keytosymbol"].ColumnName = "Keytosymbol";
            //dt.Columns["SalesLocation"].ColumnName = "SalesLocation";
            return dt;
        }

        public DataTable GetDataForExcelExportEmail(string filterText, bool NewArrival, int LoginID, bool IsSpecialSearch = true, bool IsOnlyMemo = false)
        {
            //string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
            List<InventoryDownloadEmailViewModel> objInvVM = new List<InventoryDownloadEmailViewModel>();



            objInvVM = NewArrival == false ? InventoryDownload<InventoryDownloadEmailViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "EmailSearch", (IsSpecialSearch ? "SpecialSearch" : "NormalSearch")) :
                                                   InventoryDownload<InventoryDownloadEmailViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "EmailSearch", "NewArrival");

            DataTable dt = Rosyblueonline.Framework.ListtoDataTable.ToDataTable<InventoryDownloadEmailViewModel>(objInvVM);
          
            dt.Columns["Cut"].ColumnName = "Cut Grade";  
            dt.Columns["Discount"].ColumnName = "Rapnet Discount %";
            dt.Columns["Price"].ColumnName = "Pricect"; 
            dt.Columns["DepthPerc"].ColumnName = "Depth %";
            dt.Columns["TablePerc"].ColumnName = "Table %"; 
            dt.Columns["CrownHeight"].ColumnName = "Crown Height";
            dt.Columns["CrownAngle"].ColumnName = "Crown Angle";
            dt.Columns["PavilionDepth"].ColumnName = "Pavilion Depth";
            dt.Columns["PavilionAngle"].ColumnName = "Pavilion Angle";
            dt.Columns["StarLength"].ColumnName = "StarLength";
            dt.Columns["GirdlePerc"].ColumnName = "Girdle %";
            dt.Columns["Keytosymbol"].ColumnName = "Keytosymbol"; 
            return dt;
        }
        public DataTable GetDataForExcelExport2(string filterText, bool NewArrival, int LoginID)
        {
            ////proc_CustomSiteSearch 11,'LOTNO~D167321138938,D167321148813,D167321271739,D167321320951,D167321423459,D167322002845,D167322035924,D167322068033,D167322201548,D167322613648,D167322705023,D167322864231,D167323068035,D167323175661,D167323190899,D167323271446,D167323271611,D167323271760,D167323864563,D167326023881,D167326065073,D167326201207,D167326705026,D167326772230,D167326811583,D167326811588,D167328148666,D167328148702,D167328614088,D167328705021,D167328811571,D16M3E58210,D16M3E58211,D16M3E58215,D16M3E58216,D16M3E58218',0,500000,'LotNumber','asc','MailBody','SpecialSearch'
            this.dBSQL.Parameters.Clear();
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@loginID", LoginID));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@searchText", filterText));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageIndex", "0"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageSize", "500000"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderBy", "LotNumber"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderDir", "asc"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@raisedEvent", "MailBody"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@raisedWhere", "SpecialSearch"));
            DataSet ds = this.dBSQL.ExecuteCommand("proc_CustomSiteSearch", CommandType.StoredProcedure);
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns["RowNum"].ColumnName = "Sr.No.";
                dt.Columns["Stock"].ColumnName = "Lot Number";
                dt.Columns["Weight"].ColumnName = "Size";
                dt.Columns["Certificate"].ColumnName = "Lab";
                dt.Columns["TablePerc"].ColumnName = "Table (%)";
                dt.Columns["DepthPerc"].ColumnName = "Dept (%)";
                dt.Columns["SalesLocation"].ColumnName = "Location";
                dt.Columns["RapnetPrice"].ColumnName = "Rap ($)";
                dt.Columns["Discount"].ColumnName = "Disc (%)";
                dt.Columns["Price"].ColumnName = "Price ($)/ct";
                dt.Columns["Amount"].ColumnName = "Amount (US$)";
                DataRow FooterRow = dt.NewRow();

                double Size = 0, Rap = 0, Disc = 0, Price = 0, Amount = 0, RapAmount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Amount += Convert.ToDouble(dt.Rows[i]["Amount (US$)"]);
                    Size += Convert.ToDouble(dt.Rows[i]["Size"]);
                    RapAmount += Convert.ToDouble(dt.Rows[i]["RapAmount"]);
                }
                Price = Math.Round((Amount / Size), 2);
                Disc = Math.Round((100 - (Amount / (RapAmount / 100))), 2);
                Rap = Math.Round((RapAmount / Size), 2);

                FooterRow[1] = "Total";
                FooterRow[3] = Math.Round(Size, 3);
                FooterRow[14] = Rap;
                FooterRow[15] = Disc;
                FooterRow[16] = Price;
                FooterRow[17] = Amount;

                dt.Rows.Add(FooterRow);
                dt.Columns.Remove("RapAmount");
            }
            return dt;
        }

        public DataTable GetDataForExcelExport3(int OrderID, int CustomerID, decimal salesAvgDiscount)
        {
            ////proc_CustomSiteSearch 11,'LOTNO~D167321138938,D167321148813,D167321271739,D167321320951,D167321423459,D167322002845,D167322035924,D167322068033,D167322201548,D167322613648,D167322705023,D167322864231,D167323068035,D167323175661,D167323190899,D167323271446,D167323271611,D167323271760,D167323864563,D167326023881,D167326065073,D167326201207,D167326705026,D167326772230,D167326811583,D167326811588,D167328148666,D167328148702,D167328614088,D167328705021,D167328811571,D16M3E58210,D16M3E58211,D16M3E58215,D16M3E58216,D16M3E58218',0,500000,'LotNumber','asc','MailBody','SpecialSearch'
            this.dBSQL.Parameters.Clear();
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@orderId", OrderID));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@customerId", CustomerID));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SaleAvgDiscount", salesAvgDiscount));
            DataSet ds = this.dBSQL.ExecuteCommand("SellMemo_Mail", CommandType.StoredProcedure);
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns["RowNum"].ColumnName = "Sr.No.";
                dt.Columns["Stock"].ColumnName = "Lot Number";
                dt.Columns["Weight"].ColumnName = "Size";
                dt.Columns["Certificate"].ColumnName = "Lab";
                dt.Columns["TablePerc"].ColumnName = "Table (%)";
                dt.Columns["DepthPerc"].ColumnName = "Dept (%)";
                dt.Columns["SalesLocation"].ColumnName = "Location";
                dt.Columns["RapnetPrice"].ColumnName = "Rap ($)";
                dt.Columns["Discount"].ColumnName = "Disc (%)";
                dt.Columns["Price"].ColumnName = "Price ($)/ct";
                dt.Columns["Amount"].ColumnName = "Amount (US$)";
                DataRow FooterRow = dt.NewRow();

                double Size = 0, Rap = 0, Disc = 0, Price = 0, Amount = 0, RapAmount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["RowNum"] = i + 1;
                    Amount += Convert.ToDouble(dt.Rows[i]["Amount (US$)"]);
                    Size += Convert.ToDouble(dt.Rows[i]["Size"]);
                    RapAmount += Convert.ToDouble(dt.Rows[i]["RapAmount"]);
                }
                Price = Math.Round((Amount / Size), 2);
                //Disc = Math.Round((100 - (Amount / (RapAmount / 100))), 2);
                //Disc = Math.Round((Price / (RapAmount / Convert.ToDouble(dt.Rows.Count)) * 100) - 100,2);
                Disc = Math.Round((Amount / RapAmount) * 100 - 100, 2);
                //Rap = Math.Round((RapAmount / Size), 2);
                Rap = Math.Round((Amount / Size), 2);
                FooterRow[1] = "Total";
                FooterRow[3] = Math.Round(Size, 3);
                FooterRow[14] = Rap;
                FooterRow[15] = Disc;
                FooterRow[16] = Price;
                FooterRow[17] = Amount;

                dt.Rows.Add(FooterRow);
                dt.Columns.Remove("RapAmount");
            }
            return dt;
        }

        public DataTable GetDataForExcelExportForPrint(string filterText, int LoginID)
        {
            this.dBSQL.Parameters.Clear();
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@loginID", LoginID));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@searchText", filterText));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageIndex", "0"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pageSize", "500000"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderBy", "LotNumber"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderDir", "asc"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@raisedEvent", "PrintOption"));
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@raisedWhere", "SpecialSearch"));
            DataSet ds = this.dBSQL.ExecuteCommand("proc_CustomSiteSearch", CommandType.StoredProcedure);
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.Columns["RowNum"].ColumnName = "Sr.No.";
                //dt.Columns["Stock"].ColumnName = "Lot Number";
                //dt.Columns["Weight"].ColumnName = "Size";
                //dt.Columns["Certificate"].ColumnName = "Lab";
                //dt.Columns["TablePerc"].ColumnName = "Table (%)";
                //dt.Columns["DepthPerc"].ColumnName = "Dept (%)";
                //dt.Columns["SalesLocation"].ColumnName = "Location";
                //dt.Columns["RapnetPrice"].ColumnName = "Rap ($)";
                //dt.Columns["Discount"].ColumnName = "RapNet Disc (%)";
                //dt.Columns["Price"].ColumnName = "Price ($)/ct";
                //dt.Columns["Amount"].ColumnName = "Amount (US$)";
                DataRow FooterRow = dt.NewRow();

                double Size = 0, Rap = 0, Disc = 0, Price = 0, Amount = 0, RapAmount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Amount += Convert.ToDouble(dt.Rows[i]["Amount"]);
                    Size += Convert.ToDouble(dt.Rows[i]["Weight"]);
                    RapAmount += Convert.ToDouble(dt.Rows[i]["RapAmount"]);
                }
                Price = Amount / Size;
                Disc = Math.Round((100 - (Amount / (RapAmount / 100))), 2);
                Rap = Math.Round((RapAmount / Size), 2);

                FooterRow[1] = dt.Rows.Count;
                FooterRow[3] = Size;
                FooterRow[16] = Rap;
                FooterRow[18] = Disc;
                FooterRow[19] = Math.Round((Amount / Size), 2);
                FooterRow[20] = Amount;
                //FooterRow[21] = Math.Round((100 - (Amount / RapAmount * 100)));
                //FooterRow[22] = Math.Round((Size / Amount), 2);
                //FooterRow[23] = Math.Round(Amount,2);

                dt.Rows.Add(FooterRow);
                dt.Columns.Remove("RapAmount");
                DataTable dtNew = dt.Clone();
                dtNew.ImportRow(dt.Rows[dt.Rows.Count - 1]);
                for (int i = 0; i < (dt.Rows.Count == 0 ? 0 : dt.Rows.Count - 1); i++)
                {
                    dtNew.ImportRow(dt.Rows[i]);
                }
                return dtNew;
            }
            return dt;
        }

        public DataTable GetMemoDataForExcelExport(string filterText, int LoginID)
        {
            //string path = Server.MapPath(ConfigurationManager.AppSettings["INVUpload"].ToString());
            List<InventoryDownloadMemoViewModel> objInvVM = new List<InventoryDownloadMemoViewModel>();
            //objInvVM = InventoryDownload<InventoryDownloadMemoViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "SpecificSearch", "MEMOSearch");
            objInvVM = InventoryDownload<InventoryDownloadMemoViewModel>(LoginID.ToString(), filterText, "0", "500000", "LotNumber", "asc", "MEMOSearch", "MEMOSearch");
            DataTable dt = Rosyblueonline.Framework.ListtoDataTable.ToDataTable<InventoryDownloadMemoViewModel>(objInvVM);
            //dt.Columns["LotNumber"].ColumnName = "Stock";
            //dt.Columns["Carat"].ColumnName = "Weight";
            //dt.Columns["Measurement"].ColumnName = "Sizes";
            //dt.Columns["diaLength"].ColumnName = "Length";
            //dt.Columns["diaWidth"].ColumnName = "Width";
            //dt.Columns["diaDepth"].ColumnName = "Depth";
            //dt.Columns["HA"].ColumnName = "HeartAndArrows";
            dt.Columns["Cut"].ColumnName = "Cut Grade";
            //dt.Columns["Fluorescence"].ColumnName = "Fluorescence_Intensity";
            //dt.Columns["ExcelLab"].ColumnName = "Lab";
            //dt.Columns["Rap"].ColumnName = "Rapnet_Price";
            dt.Columns["RapAmount"].ColumnName = "Rap Amount";
            dt.Columns["Discount"].ColumnName = "Rapnet Discount %";
            dt.Columns["Price"].ColumnName = "Pricect";
            dt.Columns["CertificateNo"].ColumnName = "Certificate_#";
            dt.Columns["DepthPerc"].ColumnName = "Depth %";
            dt.Columns["TablePerc"].ColumnName = "Table %";
            //dt.Columns["Girdle"].ColumnName = "Girdle Thin";
            dt.Columns["CrownHeight"].ColumnName = "Crown Height";
            dt.Columns["CrownAngle"].ColumnName = "Crown Angle";
            dt.Columns["PavilionDepth"].ColumnName = "Pavilion Depth";
            dt.Columns["PavilionAngle"].ColumnName = "Pavilion Angle";
            dt.Columns["StarLength"].ColumnName = "StarLength";
            dt.Columns["GirdlePerc"].ColumnName = "Girdle %";
            dt.Columns["Keytosymbol"].ColumnName = "Keytosymbol";
            dt.Columns["refdata"].ColumnName = "MemoTo";
            dt.Columns["DNA"].ColumnName = "Video";


            //dt.Columns["SalesLocation"].ColumnName = "SalesLocation";
            return dt;
        }

        public DataSet GetInventoryUpdateSummary(int FileID)
        {
            this.dBSQL.Parameters.Clear();
            this.dBSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FileID", FileID));
            DataSet ds = this.dBSQL.ExecuteCommand("proc_GetMailSummaryRAPOrDISC", CommandType.StoredProcedure);
            return ds;
        }

        public void SendInventoryViaMail(SendInventoryRequestModel objInv, string TemplatePath, string imgPath, int LoginID)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));

            List<string> lstOfEmailIDs = new List<string>();
            lstOfEmailIDs.Add(objInv.EMailTo);

            DataTable dt = GetDataForExcelExport(objInv.filterText, false, LoginID);
            if (dt.Columns.Count > 0)
            {
                dt.Columns.RemoveAt(0);
                dt.Columns.RemoveAt(dt.Columns.Count - 1);
            }
            byte[] st = ExportToExcel.InventoryExportToExcel(dt, imgPath, true);
            List<MailAttachment> objLst = new List<MailAttachment>();
            var objMA = new MailAttachment();
            objMA.FileBytes = st;
            objMA.FileName = "InventoryExport.xlsx";
            objLst.Add(objMA);
            sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objInv.EmailName);
            sbMailTemplate = sbMailTemplate.Replace("${Message}", objInv.Message);

            if (this.objMU == null)
            {
                this.objMU = new MailUtility();
            }
            objMU.SendMail(lstOfEmailIDs, objInv.Subject, true, sbMailTemplate.ToString(), objLst, objInv.EMailCC, objInv.EMailBCC);
        }

        public void SendUploadEventMail(string TemplatePath, string FromMail, string UserName, string Subject, string Message, string ValidFileName, string InValidFileName, string EmailCC)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            List<string> lstOfEmailIDs = new List<string>();
            lstOfEmailIDs.Add(FromMail);
            List<MailAttachment> objLst = new List<MailAttachment>();
            if (System.IO.File.Exists(ValidFileName))
            {
                objLst.Add(new MailAttachment { FileName = "Valid.xlsx", FileBytes = System.IO.File.ReadAllBytes(ValidFileName) });
            }
            if (System.IO.File.Exists(InValidFileName))
            {
                objLst.Add(new MailAttachment { FileName = "InValid.xlsx", FileBytes = System.IO.File.ReadAllBytes(InValidFileName) });
            }
            sbMailTemplate = sbMailTemplate.Replace("${UserName}", UserName);
            sbMailTemplate = sbMailTemplate.Replace("${Message}", Message);

            if (this.objMU == null)
            {
                this.objMU = new MailUtility();
            }
            objMU.SendMail(lstOfEmailIDs, Subject, true, sbMailTemplate.ToString(), objLst, EmailCC);
        }

        public void SendUploadEventMail2(string TemplatePath, string FromMail, string UserName, string Subject, string Message, string BackupFileName, int FileID, string EmailCC)
        {
            StringBuilder sbMailTemplate = new StringBuilder();
            sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
            List<string> lstOfEmailIDs = new List<string>();
            lstOfEmailIDs.Add(FromMail);
            List<MailAttachment> objLst = new List<MailAttachment>();
            if (System.IO.File.Exists(BackupFileName + ".xlsx"))
            {
                objLst.Add(new MailAttachment { FileName = "beforeChange.xlsx", FileBytes = System.IO.File.ReadAllBytes(BackupFileName + ".xlsx") });
            }

            DataSet ds = GetInventoryUpdateSummary(FileID);
            if (ds.Tables.Count > 0)
            {
                string Table1 = CommonFunction.ConvertDataTableToHTML(ds.Tables[0]);
                string Table2 = CommonFunction.ConvertDataTableToHTML(ds.Tables[1]);
                sbMailTemplate = sbMailTemplate.Replace("${UserName}", UserName);
                sbMailTemplate = sbMailTemplate.Replace("${TotalNoOfStones}", ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["No.Of Stones"].ToString() : "");
                sbMailTemplate = sbMailTemplate.Replace("${UpdatedNoOfStones}", ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0]["No.Of Stones"].ToString() : "");
                sbMailTemplate = sbMailTemplate.Replace("${Table1}", Table1);
                sbMailTemplate = sbMailTemplate.Replace("${Table2}", Table2);


                if (this.objMU == null)
                {
                    this.objMU = new MailUtility();
                }
                objMU.SendMail(lstOfEmailIDs, Subject, true, sbMailTemplate.ToString(), objLst, EmailCC);
            }
        }

        #region BestDeal
        public int BestDeals(DataTable dt, string InventoryIds, decimal DiscountPercent, string Remark, int FileId, string TokenID, int CreatedBy, string RaiseEvent)
        {
            return this.uow.Inventory.BestDeals(dt, InventoryIds, DiscountPercent, Remark, FileId, TokenID, CreatedBy, RaiseEvent);
        }
        #endregion

        #region Add & Remove Lab Status
        public int AddRemoveLabStatus(DataTable dt, int fileId, string RaiseEvent)
        {
            return this.uow.Inventory.AddRemoveLabStatus(dt, fileId, RaiseEvent);
        }
        #endregion

        #region Stock History
        public IQueryable<StockHistoryViewModel> QueryableStockHistory()
        {
            return this.uow.StockHistoryViewModel.Queryable();
        }
        #endregion

        public decimal GetAvgRapOff(string LotNos)
        {
            return this.uow.Inventory.GetAvgRapOff(LotNos);
        }

        public MstCustomerPermisionModel GetSizePermision(int LoginId)
        {
            return uow.mstCustomerPermision.Queryable().Where(x => x.isActive == true && x.customerId== LoginId && x.roleID==3).FirstOrDefault();
        }

        //public List<UserActivityLogModel> GetCustomerLog(int LoginId)
        //{
        //    return uow.Inventory.GetCustomerLogData(LoginId);
        //}

        public List<ORRAStockDetailsModel> GetORRAStockData(int LoginID, string RaiseEvent)
        {
            return this.uow.ExecuteQuery<ORRAStockDetailsModel>("Exec prcGetReports " + LoginID.ToString() + ","+ RaiseEvent.ToString() );
        }

        public ORRAStockDetailsValidate ORRAStockDetailsValidate(int LoginID, string LotNos, string RaiseEvent)
        {
            return uow.Inventory.ORRAStockDetailsValidate(LoginID, LotNos, RaiseEvent);
        }
        public List<PlaceOrderOrra> ORRAPlaceOrder(int LoginID, int OrderBlockedId, string LotNos)
        {
            return this.uow.ExecuteQuery<PlaceOrderOrra>("Exec proc_PlaceOrderFromAPI " + LoginID.ToString() + "," + OrderBlockedId.ToString() + "," + LotNos.ToString());
        }


        public List<TanishqStockModel> TanishqStockInventory(params string[] Parameters)
        {
            if (Parameters.Length > 7)
            {
                return this.uow.ExecuteQuery<TanishqStockModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','{7}'", Parameters);
            }
            else
            {
                return this.uow.ExecuteQuery<TanishqStockModel>("Exec proc_CustomSiteSearch '{0}','{1}',{2},{3},'{4}','{5}','{6}','NormalSearch'", Parameters);
            }
        }


        public List<BuildSearchCriterias> BuildSearchCriteria(string SearchCriteria,int LoginId)
        {
            return this.uow.ExecuteQuery<BuildSearchCriterias>("Exec proc_BuildCriteria_API " + LoginId.ToString() + "," + SearchCriteria.ToString());

        }

        public List<TanishqPlaceOrder> TanishqPlaceOrder(int LoginID,  string MergeOrderList)
        {
            return this.uow.ExecuteQuery<TanishqPlaceOrder>("Exec proc_CartOrderPlacement_API " + LoginID.ToString() + "," + MergeOrderList.ToString());
        }

        public List<TanishqStockModel> TanishqSoldStockInventory(params string[] Parameters)
        {
            
           return this.uow.ExecuteQuery<TanishqStockModel>("Exec proc_GetSoldInventoryDetails_API '{0}'", Parameters);
            
        }

        public List<RemoveFromCartInventory> RemoveFromCart(params string[] Parameters)
        {

            return this.uow.ExecuteQuery<RemoveFromCartInventory>("Exec proc_UnblockCartStone_API '{0}','{1}'", Parameters);

        }


        public TanishqStockDetailsValidate TanishqStockDetailsValidate(int LoginID, string LotNos, string RaiseEvent)
        {
            return uow.Inventory.TanishqStockDetailsValidate(LoginID, LotNos, RaiseEvent);
        }


        public List<FTPInventoryUpload> FTPInventoryFileUpload(DataTable dt, params string[] parameters)
        {
            return this.uow.Inventory.FTPInventoryFileUpload(dt, parameters);
        }
        public List<InventoryUpload> FTPInventoryUploadandModify(params string[] parameters)
        {
            return this.uow.Inventory.FTPInventoryUploadandModify(parameters);
        }



        

    }
}
