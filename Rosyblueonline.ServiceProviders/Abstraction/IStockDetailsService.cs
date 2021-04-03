using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IStockDetailsService
    {
        _SearchFilterViewModel SearchFilter(int LoginID);
        List<InventoryCountViewModel> InventoryCount(params string[] parameters);
        List<inventoryDetailsViewModel> InventoryAction(params string[] Parameters);

        List<inventoryDetailsViewModel> GetInventoriesByLotID(int LoginID, string SearchText);
        SummaryCalsViewModel SummaryCalculation(params string[] parameters);
        CartCountViewModel Cart(params string[] parameters);
        WatchListCountViewModel Watch(params string[] parameters);
        List<inventoryDetailsViewModel> GetCartandWatch(params string[] Parameters);
        CompareInventoryViewModel CompareInventory(int LoginID, string LotNos);
        DashboardViewModel DashboardView(int LoginId);
        List<RecentSearchViewModel> RecentSearchView(int LoginId);
        List<mstUploadFormatViewModel> InventoryUploadTypes(params string[] parameters);
        int InsertFileUploadLog(params string[] parameters);
        List<InventoryUpload> InventoryUpload(DataTable dt, params string[] Parameters);
        IQueryable<FileUploadLogViewModel> QueryableFileUploadLog();
        StockStatusViewModel StockStatus(DataTable dt, int LoginID, decimal pageIndex, int pageSize, string RaisedEvent, string OrderBy, string OrderDirection);
        DataSet StockStatus(DataTable dt, int LoginID, string RaisedEvent, string OrderBy, string OrderDirection);
        //List<T> InventoryDownload<T>(params string[] Parameters);
        List<T> InventoryDownload<T>(params string[] Parameters) where T : class;
        //List<InventoryDownloadViewModel> InventoryDownload(params string[] Parameters);
        //List<InventoryDownloadViewModel> InventoryDownloadMemo(params string[] Parameters);
        DataTable GetDataForExcelExport(string filterText, bool NewArrival, int LoginID, bool IsSpecialSearch = true, bool IsOnlyMemo = false);
        DataTable SpecificSearchDownloadExcelExport(string filterText, bool NewArrival, int LoginID, bool IsSpecialSearch = true, bool IsOnlyMemo = false, string PermissibleDownload = "NO");
 
        DataTable GetDataForExcelExportForPrint(string filterText, int LoginID);
        DataSet GetInventoryUpdateSummary(int FileID);
        DataTable GetMemoDataForExcelExport(string filterText, int LoginID);
        void SendInventoryViaMail(SendInventoryRequestModel objInv, string TemplatePath, string imgPath, int LoginID);
        void SendUploadEventMail(string TemplatePath, string FromMail, string UserName, string Subject, string Message, string ValidFileName, string InValidFileName, string EmailCC);
        void SendUploadEventMail2(string TemplatePath, string FromMail, string UserName, string Subject, string Message, string BackupFileName, int FileID, string EmailCC);
        int BestDeals(DataTable dt, string InventoryIds, decimal DiscountPercent, string Remark, int FileId, string TokenID, int CreatedBy, string RaiseEvent);
        IQueryable<StockHistoryViewModel> QueryableStockHistory();
        List<StockSummaryViewModel> StockSummary(string Shape,string LocationID);
        int AddRemoveLabStatus(DataTable dt, int fileId, string RaiseEvent);
        List<InventoryUpload> UpdateV360ViaCertNo(DataTable dt, int fileId);
        List<MstSalesLocationModel> SalesLocationActive();
        decimal GetAvgRapOff(string LotNos);

        List<StoneDetailsStockSummaryModel> StoneDetailsStockSummary(string StartRange, string EndRange, string Shape, string salesLocationIDs, string SearchMode, string RaisedEvent);

        //List<UserActivityLogModel> GetCustomerLog(int LoginId);

        MstCustomerPermisionModel GetSizePermision(int LoginId);

        List<ORRAStockDetailsModel> GetORRAStockData(int LoginID, string RaiseEvent);

        ORRAStockDetailsValidate ORRAStockDetailsValidate(int LoginID, string LotNos, string RaiseEvent);

        List<PlaceOrderOrra> ORRAPlaceOrder(int LoginID, int OrderBlockedId, string LotNos);

        List<TanishqStockModel> TanishqStockInventory (params string[] Parameters);

        List<BuildSearchCriterias> BuildSearchCriteria(string SearchCriteria,int LoginId);

        List<TanishqPlaceOrder> TanishqPlaceOrder(int LoginID, string MergeOrderList);

        List<TanishqStockModel> TanishqSoldStockInventory(params string[] Parameters);

        List<RemoveFromCartInventory> RemoveFromCart(params string[] Parameters);

        TanishqStockDetailsValidate TanishqStockDetailsValidate(int LoginID, string LotNos, string RaiseEvent);
         
        List<FTPInventoryUpload> FTPInventoryFileUpload(DataTable dt, params string[] Parameters);

        List<InventoryUpload> FTPInventoryUploadandModify(params string[] parameters);

        List<CartItemReminderEmailListModel> CartItemReminderEmails(params string[] parameters);

        DataTable RapnetDownloadExcelExport(string id);

    }
}
