using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;

namespace Rosyblueonline.Repository.Context
{
    public interface IDataContext
    {

    }
    public class DataContext : DbContext, IDataContext
    {

        public DataContext() : base("name=RosyblueonlineEntities")
        { }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginDetailModel>().ToTable("loginDetails");
            modelBuilder.Entity<UserDetailModel>().ToTable("userDetails");
            modelBuilder.Entity<LoginDeviceModel>().ToTable("loginDevice");
            modelBuilder.Entity<MstCountryModel>().ToTable("mstCountry");
            modelBuilder.Entity<MstStateModel>().ToTable("mstStates");
            modelBuilder.Entity<UpcomingShowModel>().ToTable("upcomingShow");
            modelBuilder.Entity<OtpLog>().ToTable("otpLog");
            modelBuilder.Entity<TokenLogModel>().ToTable("tokenLog");
            modelBuilder.Entity<MstBillingAddressModel>().ToTable("mstBillingAddress");
            modelBuilder.Entity<MstShippingAddressModel>().ToTable("mstShippingAddress");
            modelBuilder.Entity<inventoryModel>().ToTable("inventory");
            modelBuilder.Entity<CustomSelectModel>().ToTable("CustomSelect");
            modelBuilder.Entity<orderDetailModel>().ToTable("orderDetails");
            modelBuilder.Entity<orderItemDetailModel>().ToTable("orderItemDetails");
            modelBuilder.Entity<OrderListView>().ToTable("OrderListView");
            modelBuilder.Entity<CustomerListView>().ToTable("CustomerListView");
            modelBuilder.Entity<RecentSearchModel>().ToTable("recentSearch");
            modelBuilder.Entity<CartDetailModel>().ToTable("cartDetails");
            modelBuilder.Entity<WatchListModel>().ToTable("watchList");
            modelBuilder.Entity<fileUploadLogModel>().ToTable("fileUploadLog");
            modelBuilder.Entity<MemoFileIDDetailModel>().ToTable("memoFileIDDetails");
            modelBuilder.Entity<mstUploadFormatModel>().ToTable("mstUploadFormat");
            modelBuilder.Entity<UserDetailView>().ToTable("UserDetailView");
            modelBuilder.Entity<BlueNileDiscountModel>().ToTable("blueNileDiscount");
            modelBuilder.Entity<JamesAllenDiscountModel>().ToTable("jamesAllenDiscount");
            modelBuilder.Entity<JamesAllenDiscountHKModel>().ToTable("jamesAllenDiscountHK");
            modelBuilder.Entity<mstChargesModel>().ToTable("mstCharges");
            modelBuilder.Entity<BlockSiteHistoryModel>().ToTable("BlockSiteHistory");
            modelBuilder.Entity<mstRFIDModel>().ToTable("mstRFID");
            modelBuilder.Entity<RFIDhistoryModel>().ToTable("RFIDhistory");
            modelBuilder.Entity<DownloadScriptModel>().ToTable("downloadScripts");
            modelBuilder.Entity<DownloadRightModel>().ToTable("DownloadRights");
            modelBuilder.Entity<DownloadRightView>().ToTable("DownloadRightView");
            modelBuilder.Entity<DownloadList>().ToTable("DownloadList");
            modelBuilder.Entity<MstSalesLocationModel>().ToTable("mstSalesLocation");
            modelBuilder.Entity<StockHistoryViewModel>().ToTable("StockHistoryView");
            modelBuilder.Entity<WS_SchedulerModel>().ToTable("WS_Scheduler");
            modelBuilder.Entity<UserGeoLocationModel>().ToTable("UserGeoLocation");
            modelBuilder.Entity<UserActivityLogModel>().ToTable("UserActivityLog");

            modelBuilder.Entity<MstTypesModel>().ToTable("MstTypes");
            modelBuilder.Entity<MstDocIdentityModel>().ToTable("MstDocIdentity");

            modelBuilder.Entity<UserKycDocDetailsModel>().ToTable("UserKycDocDetails");

            modelBuilder.Entity<MstCustomerPermisionModel>().ToTable("mstCustomerPermision"); 
            modelBuilder.Entity<MenuMasterModel>().ToTable("MenuMaster");
            modelBuilder.Entity<UserMenuPermissionModel>().ToTable("UserMenuPermission");
            

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoginDetailModel> LoginDetails { get; set; }
        public DbSet<UserDetailModel> UserDetails { get; set; }
        public DbSet<LoginDeviceModel> LoginDevices { get; set; }
        public DbSet<MstCountryModel> MstCountries { get; set; }
        public DbSet<MstStateModel> MstStates { get; set; }
        public DbSet<UpcomingShowModel> UpcomingShows { get; set; }
        public DbSet<OtpLog> OtpLogs { get; set; }
        public DbSet<TokenLogModel> TokenLogs { get; set; }
        public DbSet<MstBillingAddressModel> BillingAddresses { get; set; }
        public DbSet<inventoryModel> Inventory { get; set; }
        public DbSet<CustomSelectModel> CustomSelects { get; set; }
        public DbSet<MstShippingAddressModel> ShippingAddresses { get; set; }
        public DbSet<orderDetailModel> orderDetails { get; set; }
        public DbSet<orderItemDetailModel> orderItemDetails { get; set; }
        public DbSet<OrderListView> OrderListViews { get; set; }
        public DbSet<CustomerListView> CustomerListViews { get; set; }
        public DbSet<RecentSearchModel> RecentSearches { get; set; }
        public DbSet<CartDetailModel> CartDetails { get; set; }
        public DbSet<WatchListModel> WatchLists { get; set; }
        public DbSet<fileUploadLogModel> FileUploadLogModels { get; set; }
        public DbSet<MemoFileIDDetailModel> MemoFileIDDetails { get; set; }
        public DbSet<mstUploadFormatModel> MstUploadFormats { get; set; }
        public DbSet<UserDetailView> UserDetailViews { get; set; }
        public DbSet<DownloadScriptModel> DownloadScript { get; set; }
        public DbSet<DownloadRightModel> DownloadRights { get; set; }
        public DbSet<BlueNileDiscountModel> BlueNileDiscount { get; set; }
        public DbSet<JamesAllenDiscountModel> JamesAllenDiscount { get; set; }
        public DbSet<JamesAllenDiscountHKModel> JamesAllenDiscountHK { get; set; }
        public DbSet<mstChargesModel> MstCharges { get; set; }
        public DbSet<BlockSiteHistoryModel> BlockSiteHistories { get; set; }
        public DbSet<mstRFIDModel> mstRFIDs { get; set; }
        public DbSet<RFIDhistoryModel> RFIDhistories { get; set; }
        public DbSet<DownloadRightView> DownloadRightViews { get; set; }
        public DbSet<DownloadList> DownloadLists { get; set; }
        public DbSet<MstSalesLocationModel> MstSalesLocation { get; set; }
        public DbSet<StockHistoryViewModel> StockHistoryViewModel { get; set; }
        public DbSet<WS_SchedulerModel> WS_Scheduler { get; set; }

        public DbSet<UserGeoLocationModel> UserGeoLocation { get; set; }

        public DbSet<UserActivityLogModel> UserActivityLog { get; set; }

        public DbSet<MstTypesModel> MstTypes { get; set; }

        public DbSet<MstDocIdentityModel> MstDocIdentity { get; set; }

        public DbSet<UserKycDocDetailsModel> UserKycDocDetails { get; set; }

        public DbSet<MstCustomerPermisionModel> mstCustomerPermision { get; set; }

        public DbSet<MenuMasterModel> MenuMaster { get; set; }
        public DbSet<UserMenuPermissionModel> UserMenuPermission { get; set; }
        


        // need to exclude the below
        public DbSet<mstColorModel> Color { get; set; }
        public DbSet<mstLabModel> Lab { get; set; }
        public DbSet<mstfancyColorModel> FancyColor { get; set; }
        public DbSet<mstClarityModel> Clarity { get; set; }
        public DbSet<mstFluorescenceModel> Fluorescence { get; set; }
        public DbSet<mstKeyToSymbolModel> KeyToSymbol { get; set; }
        public DbSet<mstGirdleNamesModel> GirdleNames { get; set; }
        public DbSet<mstShadeModel> Shade { get; set; }
        public DbSet<mstTableBlackInclusionModel> TableBlack { get; set; }
        public DbSet<mstSideBlackInclusionModel> SideBlack { get; set; }
        public DbSet<mstMilkyInclusionModel> Milky { get; set; }
        public DbSet<mstEyeCleanModel> EyeClean { get; set; }
        public DbSet<mstH_AModel> HA { get; set; }
        public DbSet<MstCaratsSizeViewModel> CaratsSize { get; set; }

        public DbSet<MstStoneOriginViewModel> Origin { get; set; }
    }
}
