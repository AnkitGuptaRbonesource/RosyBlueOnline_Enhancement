using Rosyblueonline.Models;
using Rosyblueonline.Repository.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository.UnitOfWork
{ 

    public class UnitOfWork : IUnitOfWork
    {
        private DataContext db = null;
        private DbContextTransaction tran = null;
        public UnitOfWork(IDataContext db)
        {
            this.db = db as DataContext;
        }

        private Repository<LoginDetailModel> _LoginDetails = null;
        private Repository<UserDetailModel> _UserDetails = null;
        private Repository<MstCountryModel> _MstCountries = null;
        private Repository<MstStateModel> _MstStates = null;
        private Repository<MstBillingAddressModel> _MstBillingAddresses = null;
        private Repository<MstShippingAddressModel> _MstShippingAddresses = null;
        private Repository<UpcomingShowModel> _UpcomingShows = null;
        private Repository<OtpLog> _OtpLogs = null;
        private Repository<LoginDeviceModel> _LoginDevices = null;
        private Repository<TokenLogModel> _TokenLogs = null;
        private InventoryRepository _Inventory = null;
        private Repository<CustomSelectModel> _CustomSelects = null;
        private OrderRepository _Orders = null;
        private Repository<orderItemDetailModel> _OrderItemDetails = null;
        private MemoRepository _Memo = null;
        private Repository<RecentSearchModel> _RecentSearches = null;
        private Repository<CartDetailModel> _CartDetails = null;
        private Repository<WatchListModel> _WatchList = null;
        private Repository<fileUploadLogModel> _FileUploadLog = null;
        private Repository<CustomerListView> _CustomerListView = null;
        private Repository<MemoFileIDDetailModel> _MemoFileIDDetail = null;
        private Repository<mstUploadFormatModel> _MstUploadFormat = null;
        private Repository<UserDetailView> _UserDetailView = null;
        private Repository<BlueNileDiscountModel> _BlueNileDiscount = null;
        private Repository<JamesAllenDiscountModel> _JamesAllenDiscount = null;
        private Repository<JamesAllenDiscountHKModel> _JamesAllenDiscountHK = null;
        private Repository<mstChargesModel> _MstChargesModel = null;
        private Repository<BlockSiteHistoryModel> _BlockSiteHistoryModel = null;
        private Repository<mstRFIDModel> _MstRFID = null;
        private Repository<RFIDhistoryModel> _RFIDhistory = null;
        private Repository<DownloadScriptModel> _DownloadScript = null;
        private Repository<DownloadRightModel> _DownloadRights = null;
        private Repository<DownloadRightView> _DownloadRightView = null;
        private Repository<DownloadList> _DownloadList = null;
        private Repository<MstSalesLocationModel> _MstSalesLocation = null;
        private Repository<StockHistoryViewModel> _StockHistoryViewModel = null;
        private Repository<WS_SchedulerModel> _WS_Scheduler = null;
        private Repository<UserGeoLocationModel> _UserGeoLocation = null;

        private Repository<UserActivityLogModel> _UserActivityLog = null;

        private Repository<orderItemDetailModel> _orderItemDetails = null;


        private Repository<MstTypesModel> _MstTypes = null;

        private Repository<MstDocIdentityModel> _MstDocIdentity = null;
        private Repository<UserKycDocDetailsModel> _UserKycDocDetails = null;
        private Repository<orderDetailModel> _orderDetail = null;

        private Repository<MstCustomerPermisionModel> _mstCustomerPermision = null;
        private Repository<MenuMasterModel> _MenuMaster = null;

        private Repository<UserMenuPermissionModel> _UserMenuPermission = null;

        private Repository<mstFAQBankModel> _mstFAQBank = null;

        private Repository<CustomerFAQAnswersModel> _CustomerFAQAnswers = null;


        private Repository<MstRolesModel> _MstRoles = null;


        private Repository<MarketFileUploadLogModel> _MarketFileUploadLog = null;


        private Repository<QCFileUploadLogModel> _QCFileUploadLog = null;

        private Repository<DiscountMasterFileUploadLogModel> _DiscountMasterFileUploadLog = null;


        public Repository<LoginDeviceModel> LoginDevices {
            get => _LoginDevices == null ? new Repository<LoginDeviceModel>(db) : _LoginDevices; }
        public Repository<LoginDetailModel> LoginDetails { 
            get => _LoginDetails == null ? new Repository<LoginDetailModel>(db) : _LoginDetails; }
        public Repository<UserDetailModel> UserDetails { get => _UserDetails == null ? new Repository<UserDetailModel>(db) : _UserDetails; }
        public Repository<MstCountryModel> MstCountries { get => _MstCountries == null ? new Repository<MstCountryModel>(db) : _MstCountries; }
        public Repository<MstStateModel> MstStates { get => _MstStates == null ? new Repository<MstStateModel>(db) : _MstStates; }
        public Repository<UpcomingShowModel> UpcomingShows { get => _UpcomingShows == null ? new Repository<UpcomingShowModel>(db) : _UpcomingShows; }
        public Repository<OtpLog> OtpLogs { get => _OtpLogs == null ? new Repository<OtpLog>(db) : _OtpLogs; }
        public Repository<MstBillingAddressModel> MstBillingAddresses { get => _MstBillingAddresses == null ? new Repository<MstBillingAddressModel>(db) : _MstBillingAddresses; }
        public Repository<TokenLogModel> TokenLogs { get => _TokenLogs == null ? new Repository<TokenLogModel>(db) : _TokenLogs; }
        public InventoryRepository Inventory { get => _Inventory == null ? new InventoryRepository(db) : _Inventory; }
        public Repository<CustomSelectModel> CustomSelects { get => _CustomSelects == null ? new Repository<CustomSelectModel>(db) : _CustomSelects; }
        public Repository<MstShippingAddressModel> MstShippingAddresses { get => _MstShippingAddresses == null ? new Repository<MstShippingAddressModel>(db) : _MstShippingAddresses; }
        public OrderRepository Orders { get => _Orders == null ? new OrderRepository(db) : _Orders; }
        public Repository<orderItemDetailModel> OrderItemDetails { get => _OrderItemDetails == null ? new Repository<orderItemDetailModel>(db) : _OrderItemDetails; }
        public MemoRepository Memo { get => _Memo == null ? new MemoRepository(db) : _Memo; }
        public Repository<RecentSearchModel> RecentSearches { get => _RecentSearches == null ? new Repository<RecentSearchModel>(db) : _RecentSearches; }
        public Repository<CartDetailModel> CartDetails { get => _CartDetails == null ? new Repository<CartDetailModel>(db) : _CartDetails; }
        public Repository<WatchListModel> WatchList { get => _WatchList == null ? new Repository<WatchListModel>(db) : _WatchList; }
        public Repository<fileUploadLogModel> FileUploadLog { get => _FileUploadLog == null ? new Repository<fileUploadLogModel>(db) : _FileUploadLog; }
        public Repository<CustomerListView> CustomerListView { get => _CustomerListView == null ? new Repository<CustomerListView>(db) : _CustomerListView; }
        public Repository<MemoFileIDDetailModel> MemoFileIDDetail { get => _MemoFileIDDetail == null ? new Repository<MemoFileIDDetailModel>(db) : _MemoFileIDDetail; }
        public Repository<mstUploadFormatModel> MstUploadFormat { get => _MstUploadFormat == null ? new Repository<mstUploadFormatModel>(db) : _MstUploadFormat; }
        public Repository<UserDetailView> UserDetailView { get => _UserDetailView == null ? new Repository<UserDetailView>(db) : _UserDetailView; }
        public Repository<BlueNileDiscountModel> BlueNileDiscount { get => _BlueNileDiscount == null ? new Repository<BlueNileDiscountModel>(db) : _BlueNileDiscount; }
        public Repository<JamesAllenDiscountModel> JamesAllenDiscount { get => _JamesAllenDiscount == null ? new Repository<JamesAllenDiscountModel>(db) : _JamesAllenDiscount; }
        public Repository<JamesAllenDiscountHKModel> JamesAllenDiscountHK { get => _JamesAllenDiscountHK == null ? new Repository<JamesAllenDiscountHKModel>(db) : _JamesAllenDiscountHK; }
        public Repository<mstChargesModel> MstChargesModel { get => _MstChargesModel == null ? new Repository<mstChargesModel>(db) : _MstChargesModel; }
        public Repository<BlockSiteHistoryModel> BlockSiteHistoryModel { get => _BlockSiteHistoryModel == null ? new Repository<BlockSiteHistoryModel>(db) : _BlockSiteHistoryModel; }
        public Repository<mstRFIDModel> MstRFID { get => _MstRFID == null ? new Repository<mstRFIDModel>(db) : _MstRFID; }
        public Repository<RFIDhistoryModel> RFIDhistory { get => _RFIDhistory == null ? new Repository<RFIDhistoryModel>(db) : _RFIDhistory; }
        public Repository<DownloadScriptModel> DownloadScript { get => _DownloadScript == null ? new Repository<DownloadScriptModel>(db) : _DownloadScript; }
        public Repository<DownloadRightModel> DownloadRights { get => _DownloadRights == null ? new Repository<DownloadRightModel>(db) : _DownloadRights; }
        public Repository<DownloadRightView> DownloadRightView { get => _DownloadRightView == null ? new Repository<DownloadRightView>(db) : _DownloadRightView; }
        public Repository<DownloadList> DownloadList { get => _DownloadList == null ? new Repository<DownloadList>(db) : _DownloadList; }
        public Repository<MstSalesLocationModel> MstSalesLocation { get => _MstSalesLocation == null ? new Repository<MstSalesLocationModel>(db) : _MstSalesLocation; }
        public Repository<StockHistoryViewModel> StockHistoryViewModel { get => _StockHistoryViewModel == null ? new Repository<StockHistoryViewModel>(db) : _StockHistoryViewModel; }
        public Repository<WS_SchedulerModel> WS_SchedulerM  { get => _WS_Scheduler == null ? new Repository<WS_SchedulerModel>(db) : _WS_Scheduler; }

        public Repository<MarketFileUploadLogModel> MarketFileUploadLog { get => _MarketFileUploadLog == null ? new Repository<MarketFileUploadLogModel>(db) : _MarketFileUploadLog; }
        public Repository<QCFileUploadLogModel> QCFileUploadLog { get => _QCFileUploadLog == null ? new Repository<QCFileUploadLogModel>(db) : _QCFileUploadLog; }

        public Repository<DiscountMasterFileUploadLogModel> DiscountMasterFileUploadLog { get => _DiscountMasterFileUploadLog == null ? new Repository<DiscountMasterFileUploadLogModel>(db) : _DiscountMasterFileUploadLog; }


        

        public Repository<UserGeoLocationModel> UserGeoLocationM
        {
            get => _UserGeoLocation == null ? new Repository<UserGeoLocationModel>(db) : _UserGeoLocation;
        }


        public Repository<UserActivityLogModel> UserActivityLogM
        {
            get => _UserActivityLog == null ? new Repository<UserActivityLogModel>(db) : _UserActivityLog;
        }

        public Repository<orderItemDetailModel> orderItemDetailM
        {
            get => _orderItemDetails == null ? new Repository<orderItemDetailModel>(db) : _orderItemDetails;
        }


        public Repository<MstTypesModel> MstTypes { get => _MstTypes == null ? new Repository<MstTypesModel>(db) : _MstTypes; }


        public Repository<MstDocIdentityModel> MstDocIdentity { get => _MstDocIdentity == null ? new Repository<MstDocIdentityModel>(db) : _MstDocIdentity; }

        public Repository<UserKycDocDetailsModel> UserKycDocDetails { get => _UserKycDocDetails == null ? new Repository<UserKycDocDetailsModel>(db) : _UserKycDocDetails; }


        public Repository<orderDetailModel> orderDetail { get => _orderDetail == null ? new Repository<orderDetailModel>(db) : _orderDetail; }

        public Repository<MstCustomerPermisionModel> mstCustomerPermision { get => _mstCustomerPermision == null ? new Repository<MstCustomerPermisionModel>(db) : _mstCustomerPermision; }
        public Repository<MenuMasterModel> MenuMaster { get => _MenuMaster == null ? new Repository<MenuMasterModel>(db) : _MenuMaster; }

        public Repository<UserMenuPermissionModel> UserMenuPermission { get => _UserMenuPermission == null ? new Repository<UserMenuPermissionModel>(db) : _UserMenuPermission; }

        public Repository<mstFAQBankModel> mstFAQBank { get => _mstFAQBank == null ? new Repository<mstFAQBankModel>(db) : _mstFAQBank; }


        public Repository<CustomerFAQAnswersModel> CustomerFAQAnswers { get => _CustomerFAQAnswers == null ? new Repository<CustomerFAQAnswersModel>(db) : _CustomerFAQAnswers; }


        public Repository<MstRolesModel> MstRoles { get => _MstRoles == null ? new Repository<MstRolesModel>(db) : _MstRoles; }

        public int Save()
        {
            return this.db.SaveChanges();
        }

        public void BeginTransaction()
        {
            tran = this.db.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (tran != null)
            {
                tran.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (tran != null)
            {
                tran.Rollback();
            }
        }

        public List<T> ExecuteQuery<T>(string Text, params string[] Parameters) where T : class
        {
            return this.db.Database.SqlQuery<T>(string.Format(Text, Parameters)).ToList();
        }
    }
}
