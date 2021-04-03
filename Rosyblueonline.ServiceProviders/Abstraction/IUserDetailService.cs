using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rosyblueonline.Framework.Constant;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IUserDetailService
    {
        bool CheckEmailID(string EmailID, int userDetailId = 0);
        bool RegisterUser(RegistrationViewModel obj, bool SelfRegistration = true);
        bool RegisterUser(UserRegistrationViewModel obj);
        bool UpdateRegisterUser(RegistrationViewModel obj,  int UserID);
        void GenerateOTP(string EmailID);
        bool SendForgetPassword(string EmailID, string Url);
        bool ResetForgetPassword(int ID, string Code, string Password, int ValidTimeout);
        bool VerifyOtp(string EmailID, string OTP);
        bool CheckUsername(string username, int loginID = 0);
        TokenLogModel Login(LoginViewModel obj);
        bool Login(string TokenID, string Password);
        LoginDetailModel GetLoginDetailByLoginID(int LoginID);
        List<AddressViewModel> GetShippingAddresses(int LoginID, bool isActive);
        List<AddressViewModel> GetBillingAddresses(int LoginID, bool isActive);
        MstBillingAddressModel GetBillingAddresses(int LoginID);
        //List<AddressViewModel> GetBillingAddresses(int LoginID);
        MstShippingAddressModel GetShippingAddressByID(int AddressID);
        MstBillingAddressModel GetBillingAddressByID(int AddressID);
        List<Select2Option> GetCustomers(string Filter);
        IQueryable<CustomerListView> CustomerList();
        CustomerListView GetCustomerByLoginID(int LoginID);
        int ApproveCustomer(int LoginID, int isApproved, int ApprovedBy);
        void SendMailOnApproval(int CustomerId, string TemplatePath);
        IQueryable<UserDetailView> QueryableUserDetail();
        int ResetPassword(int LoginID, string OldPassword, string NewPassword);
        IQueryable<UserDetailModel> QueryableUserDetailModel();
        BlockSiteHistoryModel GetLastBlockSiteHistory();
        IQueryable<BlockSiteHistoryModel> QueryableBlockSiteHistory();
        int AddLastBlockSiteHistory(BlockSiteHistoryModel obj);

        bool GeoLocation(LoginData obj);
        //bool UserActivitylog(int LoginId, string ActionName, string ActionDetails);
        bool UserActivitylogs(int LoginId, string ActionName, string ActionDetails);

        //IQueryable<UserGeoLocationModel> UserActivityDetails();

        List<UserActivityLogModel> GetCustomerLog(int LoginId);
        List<UserGeoLocationModel> GetUserGeoLoctionLog(int LoginId);

        List<UserKycDocDetailsModel> UploadMultiDoc(UserKycDocDetailsModel obj);
       
        int   CountUploadMultiDoc(string DocKey);

        List<UserKycDocDetailsModel> DeleteUserDoc(int UserDocId, string DocRandomID);

    }
}
