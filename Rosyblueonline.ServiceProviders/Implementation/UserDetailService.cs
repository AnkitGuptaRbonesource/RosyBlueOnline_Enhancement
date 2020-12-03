using AutoMapper;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rosyblueonline.Framework.Constant;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class UserDetailService : IUserDetailService
    {
        private readonly UnitOfWork uow;

        public UserDetailService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public bool RegisterUser(RegistrationViewModel obj, Roles roles, bool SelfRegistration = true)
        {
            if (CheckEmailID(obj.emailId) == false)
            {
                throw new UserDefinedException(string.Format(StringResource.AlreadyTaken, "email"));
            }
            if (obj.createdBy == 0 && SelfRegistration == true)
            {
                if (VerifyOtp(obj.emailId, obj.otp) == false)
                {
                    throw new UserDefinedException(string.Format(StringResource.Invalid, "OTP"));
                }
            }
            if (CheckUsername(obj.username) == false)
            {
                throw new UserDefinedException(string.Format(StringResource.AlreadyTaken, "User Name"));
            }

            LoginDetailModel objLD = new LoginDetailModel();
            MstBillingAddressModel objBA = new MstBillingAddressModel();
            MstShippingAddressModel objSA = new MstShippingAddressModel();
            UserDetailModel objUD = new UserDetailModel();
            this.uow.BeginTransaction();
            try
            {
                int RoleID = 0;
                Array enumValueArray = Enum.GetValues(typeof(Roles));
                foreach (int enumValue in enumValueArray)
                {
                    if (roles.ToString() == Enum.GetName(typeof(Roles), enumValue))
                    {
                        RoleID = enumValue;
                    }
                }
                // LoginDetailModel ----Start----
                objLD.username = obj.username;
                objLD.password = obj.password;
                objLD.roleID = RoleID;
                objLD.parentLoginID = 0;
                objLD.userStatus = obj.userStatus.HasValue ? obj.userStatus.Value : 0;
                objLD.locationIDs = "";
                objLD.loggedInStatus = false;
                objLD.loginAttempted = 0;
                objLD.createdOn = DateTime.Now;
                objLD.lastLoginDate = null;
                objLD.isApproved = SelfRegistration == true ? 0 : 1;
                objLD.approvedBy = SelfRegistration == true ? 0 : obj.LoginID;
                objLD.approvedOn = SelfRegistration == true ? null : (DateTime?)DateTime.Now;
                // LoginDetailModel ----End----
                this.uow.LoginDetails.Add(objLD);
                if (this.uow.Save() > 0)
                {
                    // MstBillingAddressModel ----Start----
                    objSA.companyName = objBA.companyName = obj.companyName;
                    objSA.address01 = objBA.address01 = obj.address01;
                    objSA.address02 = objBA.address02 = "";
                    objSA.countryID = objBA.countryID = obj.countryID;
                    objSA.stateID = objBA.stateID = obj.stateID;
                    objSA.cityName = objBA.cityName = obj.cityName;
                    objSA.zipCode = objBA.zipCode = obj.ZIP;
                    objSA.firstName = objBA.firstName = obj.firstName;
                    objSA.lastName = objBA.lastName = obj.lastName;
                    objSA.createdOn = objBA.createdOn = DateTime.Now;
                    objSA.loginID = objBA.loginID = objLD.loginID;
                    objSA.isActive = objBA.isActive = true;
                    objSA.isDefault = objBA.isDefault = true;
                    // MstBillingAddressModel ----End----

                    this.uow.MstBillingAddresses.Add(objBA);
                    this.uow.MstShippingAddresses.Add(objSA);

                    // UserDetailModel ----Start----
                    objUD.accNumber = obj.accNumber;
                    objUD.tinNo = obj.tinNo;
                    objUD.pan = obj.pan;
                    objUD.gstNo = obj.gstNo;
                    objUD.bankName = obj.bankName;
                    objUD.branchName = obj.branchName;
                    objUD.branchAddress = obj.branchAddress;
                    objUD.kycDocName = obj.kycDocName;
                    objUD.kycDocNo = obj.kycDocNo;

                    //objUD.isManufacturer = obj.isManufacturer;
                    //objUD.isWholeSaler = obj.isWholeSaler;
                    //objUD.isRetailer = obj.isRetailer;
                    //objUD.isOther = obj.isOther;
                    objUD.TypeId = obj.TypeId;
                    objUD.dateOfDocExpiry = obj.dateOfDocExpiry;

                    objUD.otherType = obj.otherType;
                    objUD.phoneCode01 = obj.phoneCode01;
                    objUD.phone01 = obj.phone01;
                    objUD.fax01 = obj.Fax;
                    objUD.mobile = obj.Mobile;
                    objUD.mobile = obj.Mobile;
                    objUD.dateOfEstablishment = obj.dateOfEstablishment;
                    objUD.emailId = obj.emailId;
                    objUD.website = obj.website;
                    objUD.kycDocFile = obj.kycDocFile;
                    objUD.createdOn = DateTime.Now;
                    objUD.createdDevice = "Web";
                    objUD.createdIP = "";
                    objUD.loginID = objLD.loginID;

                    // UserDetailModel ----Start----
                    this.uow.UserDetails.Add(objUD);

                     
                }
                //UserKycDocDetailsModel obkKyc = this.uow.UserKycDocDetails.Queryable().Where(x => x.LoginId == obj.DocRandomID).FirstOrDefault();
                //if (obkKyc != null)
                //{
                //    obkKyc.LoginId = Convert.ToInt32(obj.DocRandomID);
                //    this.uow.UserKycDocDetails.Edit(obkKyc);
                //    this.uow.Save();
                //}
                 
                int RowCount = uow.Save();

                DataContext entity = new DataContext();
                (from u in entity.UserKycDocDetails
                 select u).Where(x => x.LoginId == obj.DocRandomID).ToList().ForEach(x => x.LoginId = Convert.ToString(objLD.loginID));
                entity.SaveChanges();

                if (RowCount >= 2)
                {
                    this.uow.CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.uow.RollbackTransaction();
                throw ex;
            }
            return false;
        }

        public bool UpdateRegisterUser(RegistrationViewModel obj, Roles roles, int UserID)
        {
            if (obj.LoginID == 0)
            {
                throw new UserDefinedException(string.Format(StringResource.DoesNotExist, "LoginID"));
            }
            this.uow.BeginTransaction();
            UserDetailModel objUD = uow.UserDetails.Queryable().Where(x => x.loginID == obj.LoginID).FirstOrDefault();
            LoginDetailModel objLD = uow.LoginDetails.Queryable().Where(x => x.loginID == obj.LoginID).FirstOrDefault();
            MstBillingAddressModel objBA = uow.MstBillingAddresses.Queryable().Where(x => x.loginID == obj.LoginID).FirstOrDefault();
            if (objUD != null && objLD != null && objBA != null)
            {
                if (CheckEmailID(obj.emailId, objUD.userDetailId) == false)
                {
                    throw new UserDefinedException(string.Format(StringResource.AlreadyTaken, "email"));
                }

                if (CheckUsername(obj.emailId, obj.LoginID) == false)
                {
                    throw new UserDefinedException(string.Format(StringResource.AlreadyTaken, "User Name"));
                }

                //MstBillingAddressModel objBA = new MstBillingAddressModel();
                //MstShippingAddressModel objSA = new MstShippingAddressModel();

                try
                {
                    int RoleID = 0;
                    Array enumValueArray = Enum.GetValues(typeof(Roles));
                    foreach (int enumValue in enumValueArray)
                    {
                        if (roles.ToString() == Enum.GetName(typeof(Roles), enumValue))
                        {
                            RoleID = enumValue;
                        }
                    }
                    // LoginDetailModel ----Start----
                    objLD.username = obj.username;
                    if (obj.password != "" && obj.password != null)
                    {
                        objLD.password = obj.password;
                    }
                    objLD.roleID = RoleID;
                    objLD.modifiedDate = DateTime.Now;
                    objLD.modifiedBy = UserID;
                    objLD.userStatus = obj.userStatus.HasValue ? obj.userStatus.Value : 0;
                    // LoginDetailModel ----End----
                    this.uow.LoginDetails.Edit(objLD);

                    // MstBillingAddressModel ----Start----
                    objBA.companyName = obj.companyName;
                    objBA.address01 = obj.address01;
                    objBA.address02 = "";
                    objBA.countryID = obj.countryID;
                    objBA.stateID = obj.stateID;
                    objBA.cityName = obj.cityName;
                    objBA.zipCode = obj.ZIP;
                    objBA.firstName = obj.firstName;
                    objBA.lastName = obj.lastName;
                    objBA.updatedOn = DateTime.Now;
                    objBA.loginID = objLD.loginID;
                    // MstBillingAddressModel ----End----

                    this.uow.MstBillingAddresses.Edit(objBA);

                    // UserDetailModel ----Start----
                    objUD.accNumber = obj.accNumber;
                    //objUD.tinNo = obj.tinNo;
                    //objUD.pan = obj.pan;
                    //objUD.gstNo = obj.gstNo;
                    objUD.bankName = obj.bankName;
                    objUD.branchName = obj.branchName;
                    objUD.branchAddress = obj.branchAddress;
                    //objUD.kycDocName = obj.kycDocName;
                    //objUD.kycDocNo = obj.kycDocNo;
                    //objUD.isManufacturer = obj.isManufacturer;
                    //objUD.isWholeSaler = obj.isWholeSaler;
                    //objUD.isRetailer = obj.isRetailer;
                    objUD.isOther = obj.isOther;
                    objUD.otherType = obj.otherType;
                    objUD.phoneCode01 = obj.phoneCode01;
                    objUD.phone01 = obj.phone01;
                    objUD.fax01 = obj.Fax;
                    objUD.mobile = obj.Mobile;
                    //objUD.dateOfEstablishment = obj.dateOfEstablishment;
                    objUD.emailId = obj.emailId;
                    //objUD.website = obj.website;
                    //objUD.kycDocFile = obj.kycDocFile;
                    //objUD.updatedOn = DateTime.Now;
                    //objUD.loginID = objLD.loginID;

                    // UserDetailModel ----Start----
                    this.uow.UserDetails.Edit(objUD);

                    int RowCount = uow.Save();
                    if (RowCount >= 2)
                    {
                        this.uow.CommitTransaction();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.uow.RollbackTransaction();
                    throw ex;
                }
            }
            return false;
        }

        public bool RegisterUser(UserRegistrationViewModel obj, Roles roles)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRegistrationViewModel, RegistrationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var newObj = mapper.Map<UserRegistrationViewModel, RegistrationViewModel>(obj);
            return RegisterUser(newObj, roles, false);
        }

        public bool RegisterUser(UserRegistrationViewModelViaMemo obj)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRegistrationViewModelViaMemo, RegistrationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var newObj = mapper.Map<UserRegistrationViewModelViaMemo, RegistrationViewModel>(obj);
            newObj.userStatus = 1;
            return RegisterUser(newObj, Roles.CUSTOMER, false);
        }

        public bool UpdateRegisterUser(UserRegistrationViewModel obj, Roles roles, int UserID)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRegistrationViewModel, RegistrationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var newObj = mapper.Map<UserRegistrationViewModel, RegistrationViewModel>(obj);
            newObj.userStatus = 1;
            return UpdateRegisterUser(newObj, roles, UserID);
        }

        public void GenerateOTP(string EmailID)
        {
            if (!CheckEmailID(EmailID))
            {
                throw new UserDefinedException(string.Format(StringResource.AlreadyTaken, "Email id"));
            }
            int MaxAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["ReSendOtpAttempts"]);
            DateTime objDT = DateTime.Now.Date;
            int Count = uow.OtpLogs.Queryable().Where(x => x.emailId == EmailID && x.isVerified == false && objDT >= x.CreatedOn).Count();
            if (MaxAttempts < Count)
            {
                throw new UserDefinedException(StringResource.ReachedMaxAttempts);
            }
            OtpLog objOL = new OtpLog
            {
                otpID = RandomHelpers.Instance.RandomString(6, true),
                emailId = EmailID,
                isVerified = false,
                CreatedOn = DateTime.Now
            };
            uow.OtpLogs.Add(objOL);
            int RowCount = uow.Save();
            if (RowCount > 0)
            {
                MailUtility objMu = new MailUtility();
                objMu.SendMail(objOL.emailId, "Otp", true, "Your Otp is " + objOL.otpID);
            }
        }

        public bool SendForgetPassword(string EmailID, string Url)
        {
            int LoginID = 0;
            if (!CheckEmailID(EmailID, out LoginID, 0))
            {
                if (LoginID != 0)
                {
                    MailUtility objMu = new MailUtility();
                    string VerificationCode = RandomHelpers.Instance.RandomString(5, true);
                    LoginDetailModel objUD = uow.LoginDetails.GetAll().Where(x => x.loginID == LoginID).FirstOrDefault();
                    if (objUD != null)
                    {
                        objUD.verificationCode = VerificationCode;
                        objUD.modifiedDate = DateTime.Now;
                        uow.LoginDetails.Edit(objUD);
                        if (uow.Save() > 0)
                        {

                            string Body = "<html><body><p>Dear User,</p><br><p>Your current user name : <b>" + objUD.username + "</b> and Password : <b>" + objUD.password + "</b></p> <p><a href='" + Url + "?v=" + VerificationCode + "&l=" + LoginID.ToString() + "'>Click here to reset your password.</a></p></body></html>";
                            objMu.SendMail(EmailID, "Reset Password", true, Body);
                            return true;
                        }
                        return false;
                    }
                }
            }
            else
            {
                throw new UserDefinedException(string.Format(StringResource.Invalid, "Email id"));
            }
            return false;
        }

        public bool ResetForgetPassword(int ID, string Code, string Password, int ValidTimeout)
        {
            LoginDetailModel objUD = uow.LoginDetails.GetAll().Where(x => x.loginID == ID && x.verificationCode == Code).FirstOrDefault();
            if (objUD != null && objUD.modifiedDate.HasValue == true)
            {
                if (objUD.modifiedDate.Value.AddMinutes(ValidTimeout) >= DateTime.Now)
                {
                    objUD.password = Password;
                    objUD.verificationCode = null;
                    uow.LoginDetails.Edit(objUD);
                    uow.Save();
                    return true;
                }
                else
                {
                    throw new UserDefinedException("Password reset link expired");
                }
            }
            return false;
        }

        public bool VerifyOtp(string EmailID, string OTP)
        {
            OtpLog obj = uow.OtpLogs.GetAll().Where(x => x.emailId == EmailID).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            if (obj != null)
            {
                if (obj.otpID == OTP)
                {
                    obj.isVerified = true;
                    obj.UpdatedOn = DateTime.Now;
                    uow.OtpLogs.Edit(obj);
                    return uow.Save() > 0 ? true : false;
                }
            }
            return false;
        }

        public bool CheckEmailID(string EmailID, int userDetailId = 0)
        {
            UserDetailModel objUD = uow.UserDetails.GetAll().Where(x => x.emailId == EmailID && x.userDetailId != userDetailId).FirstOrDefault();
            return objUD == null ? true : false;
        }

        public bool CheckEmailID(string EmailID, out int LoginID, int userDetailId = 0)
        {
            UserDetailModel objUD = uow.UserDetails.GetAll().Where(x => x.emailId == EmailID && x.userDetailId != userDetailId).FirstOrDefault();
            if (objUD == null)
            {
                LoginID = 0;
            }
            else
            {
                LoginID = objUD.loginID;
            }
            return objUD == null ? true : false;
        }

        public bool CheckUsername(string username, int loginID = 0)
        {
            LoginDetailModel objUD = uow.LoginDetails.GetAll().Where(x => x.username == username && x.loginID != loginID).FirstOrDefault();
            return objUD == null ? true : false;
        }

        //Added by Ankit 18JUn2020
        public bool GeoLocation(LoginData obj)
        {

            UserGeoLocationModel objU  = new UserGeoLocationModel
            {
                LoginID=obj.LoginID,
                Username=obj.Username,
                DeviceName =obj.DeviceName,
                IpAddress =obj.IpAddress,
                Latitude=obj.Latitude,
                Longitude =obj.Longitude, 
                Locality = obj.Locality,
                City = obj.City,
                State = obj.State,
                Country = obj.Country,
                CreatedOn =DateTime.Now
                 
            };
            uow.UserGeoLocationM.Add(objU);
            uow.Save(); 
            return obj == null ? true : false;

        }
        //Added by Ankit 18Jun2020

        public bool UserActivitylogs(int Loginid, string Actionname, string Actiondetail)
        { 
            return this.uow.Orders.UserActivityloginsert( Loginid,   Actionname,   Actiondetail);
        }


        public  UserMenuAccessModel  UserMenuAccessModel(int Loginid, string MenuIdList, string CreatedBy, string QFlag)
        {
             UserMenuAccessModel  objAccess = uow.Orders.GetUserMenuAccessDetails(Loginid, MenuIdList, CreatedBy, QFlag);
            return objAccess;
        }
        //public bool UserActivitylog(int Loginid, string Actionname, string Actiondetail)
        //{

        //    UserActivityLogModel objL = new UserActivityLogModel
        //    {

        //        LoginId = Loginid,
        //        ActionName = Actionname,
        //        ActionDetails = Actiondetail,
        //        CreatedOn = DateTime.Now

        //    };
        //    uow.UserActivityLogM.Add(objL);
        //    uow.Save();
        //    return objL != null ? true : false;

        //}
        //Added by Ankit 19Jun2020

        //public IQueryable<UserGeoLocationModel> UserActivityDetails()
        //{
        //    //return this.uow.Orders.OrderListView();
        //    return this.uow.Orders.UserActivityLog();
        //}

        public List<UserKycDocDetailsModel> UploadMultiDoc(UserKycDocDetailsModel obj)
        {
            
          MstDocIdentityModel objMstDoc = uow.MstDocIdentity.Queryable().Where(x => x.DocId.ToString() ==  obj.KycDocId ).FirstOrDefault();


            UserKycDocDetailsModel objKyc = new UserKycDocDetailsModel
            { 
                LoginId = obj.LoginId,
                KycDocId = obj.KycDocId,
               KycDocName = objMstDoc.DocIdentityName,
                kycDocNo = obj.kycDocNo,
                kycDocFile = obj.kycDocFile,
                KycDocExpiryDate = obj.KycDocExpiryDate,
                CreatedDate = DateTime.Now,
                OrgFileName =obj.OrgFileName

            };
            uow.UserKycDocDetails.Add(objKyc);
            uow.Save();
            //return uow.UserKycDocDetails.GetAll().Where(x => x.LoginId == obj.LoginId).AsQueryable();

            return this.uow.UserKycDocDetails.Queryable().Where(x => x.LoginId == obj.LoginId).ToList();

        }
        //Added by Ankit 09July2020
        public List<UserKycDocDetailsModel> DeleteUserDoc(int UserDocId, string DocRandomID)
        {
            
            UserKycDocDetailsModel UserDoc = this.uow.UserKycDocDetails.Queryable().Where(val => val.UserDocId ==  UserDocId).Single<UserKycDocDetailsModel>();
            this.uow.UserKycDocDetails.Delete(UserDoc);
            this.uow.Save(); 

            return this.uow.UserKycDocDetails.Queryable().Where(x => x.LoginId == DocRandomID).ToList();

        }



        public int CountUploadMultiDoc(string DocKey)
        {
            return this.uow.UserKycDocDetails.Queryable().Where(x => x.LoginId == DocKey).Count();
        }
        public List<UserGeoLocationModel> GetUserGeoLoctionLog(int LoginId)
        {
            //return this.uow.Orders.OrderListView();
            return this.uow.Orders.UserGeoLoctionLog(LoginId);
        }

        
        public List<UserActivityLogModel> GetCustomerLog(int LoginId)
        {
            return uow.Orders.GetCustomerLogData(LoginId);
        }
         



        public TokenLogModel Login(LoginViewModel obj)
        {
            LoginDeviceModel objLDevice = new LoginDeviceModel();
            objLDevice.ipAddress = obj.IpAddress;
            objLDevice.deviceName = obj.DeviceName;
            objLDevice.createdOn = DateTime.Now;
            if (GetLoginAttempts(obj.IpAddress))
            {
                throw new UserDefinedException(StringResource.ReachedMaxAttempts);
            }
            LoginDetailModel objLDetails = uow.LoginDetails.Queryable().Where(x => x.username.ToLower() == obj.Username.ToLower() && x.password == obj.Password).FirstOrDefault();
            if (objLDetails == null)
            {
                uow.LoginDevices.Add(objLDevice);
                uow.Save();
                return null;
            }
            else
            {
                string FullName = uow.MstBillingAddresses.Queryable().Where(x => x.loginID == objLDetails.loginID).Select(x => x.firstName + " " + x.lastName).FirstOrDefault();
                string EmailID = uow.UserDetails.Queryable().Where(x => x.loginID == objLDetails.loginID).Select(x => x.emailId).FirstOrDefault();
                int LoginDeviceID = objLDevice.loginDeviceId;
                int LoginID = objLDetails.loginID;
                int RoleID = objLDetails.roleID;

                objLDevice.loginID = LoginID;
                uow.LoginDevices.Add(objLDevice);
                uow.Save();

                Guid objGuid = Guid.NewGuid();
                TokenLogModel objToken = new TokenLogModel
                {
                    loginDeviceId = LoginDeviceID,
                    loginID = LoginID,
                    timeStamp = DateTime.Now,
                    FullName = RoleID == 2 ? "Admin" : FullName,
                    tokenID = objGuid.ToString(),
                    EmailID = EmailID,
                    UpdatedOn = null,
                    RoleID = RoleID,
                    CreatedOn = DateTime.Now
                };
                uow.TokenLogs.Add(objToken);
                uow.Save();
                return objToken;
            }


        }

        public bool Login(string TokenID, string Password)
        {
            LoginDeviceModel objLDevice = new LoginDeviceModel();
            TokenLogModel objToken = uow.TokenLogs.GetAll().Where(x => x.tokenID == TokenID).FirstOrDefault();
            if (objToken != null)
            {
                int LoginID = objToken.loginID;
                LoginDetailModel objLD = uow.LoginDetails.GetAll().Where(x => x.loginID == LoginID).FirstOrDefault();
                if (objLD != null)
                {
                    if (objLD.password == Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public LoginDetailModel GetLoginDetailByLoginID(int LoginID)
        {
            LoginDetailModel objLogin = uow.LoginDetails.Queryable().Where(x => x.loginID == LoginID).FirstOrDefault();
            return objLogin;
        }

        public string GetLoginDetailByLoginID(string EmailID)
        {
            UserDetailModel objUD = uow.UserDetails.Queryable().Where(x => x.emailId == EmailID).FirstOrDefault();
            if (objUD != null)
            {

                int loginID = objUD.loginID;
                LoginDetailModel objLD = uow.LoginDetails.Queryable().Where(x => x.loginID == loginID).FirstOrDefault();
                objLD.verificationCode = RandomHelpers.Instance.RandomString(5, true);
                return objLD.verificationCode;
            }
            return "";
            //3005220224,3006347260,3006654512
        }

        private bool GetLoginAttempts(string IpAddress)
        {
            int LoginAttemptTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptTimeOut"]);
            int MaxLoginAttempt = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLoginAttempt"]);
            DateTime CreatedOnFrom = DateTime.Now;
            DateTime CreatedOnTo = DateTime.Now.AddMinutes(-LoginAttemptTimeOut);
            int Count = uow.LoginDevices.GetAll().Where(x => x.ipAddress == IpAddress && x.loginID == null && (x.createdOn > CreatedOnFrom && x.createdOn < CreatedOnTo)).Count();
            if (MaxLoginAttempt <= Count)
            {
                return true;
            }
            return false;
        }

        public List<AddressViewModel> GetShippingAddresses(int LoginID, bool isActive)
        {
            //return uow.MstShippingAddresses.GetAll()
            //    .Select(x=> new AddressViewModel {
            //    })
            //    .Where(x => x.loginID == LoginID && x.isActive==isActive).ToList();

            return (from sa in uow.MstShippingAddresses.Queryable()
                    join c in uow.MstCountries.Queryable() on sa.countryID equals c.countryId
                    join s in uow.MstStates.Queryable() on sa.stateID equals s.stateId
                    where sa.loginID == LoginID && sa.isActive == isActive
                    select new AddressViewModel
                    {
                        shippingId = sa.shippingId,
                        billingId = 0,
                        companyName = sa.companyName,
                        firstName = sa.firstName,
                        lastName = sa.lastName,
                        address01 = sa.address01,
                        address02 = sa.address02,
                        countryID = sa.countryID,
                        stateID = sa.stateID,
                        countryName = c.countryName,
                        stateName = s.stateName,
                        cityName = sa.cityName,
                        zipCode = sa.zipCode,
                        loginID = sa.loginID,
                        isActive = sa.isActive,
                        isDefault = sa.isDefault,
                        Type = "shipping"
                    }).ToList();
        }

        public List<AddressViewModel> GetBillingAddresses(int LoginID, bool isActive)
        {
            //return uow.MstBillingAddresses.GetAll().Where(x => x.loginID == LoginID && x.isActive == isActive).ToList();
            return (from sa in uow.MstBillingAddresses.Queryable()
                    join c in uow.MstCountries.Queryable() on sa.countryID equals c.countryId
                    join s in uow.MstStates.Queryable() on sa.stateID equals s.stateId
                    where sa.loginID == LoginID && sa.isActive == isActive
                    select new AddressViewModel
                    {
                        shippingId = 0,
                        billingId = sa.billingId,
                        companyName = sa.companyName,
                        firstName = sa.firstName,
                        lastName = sa.lastName,
                        address01 = sa.address01,
                        address02 = sa.address02,
                        countryID = sa.countryID,
                        stateID = sa.stateID,
                        countryName = c.countryName,
                        stateName = s.stateName,
                        cityName = sa.cityName,
                        zipCode = sa.zipCode,
                        loginID = sa.loginID,
                        isActive = sa.isActive,
                        isDefault = sa.isDefault,
                        Type = "billing"
                    }).ToList();
        }

        public MstBillingAddressModel GetBillingAddresses(int LoginID)
        {
            return uow.MstBillingAddresses.GetAll().Where(x => x.loginID == LoginID).FirstOrDefault();
        }

        public MstShippingAddressModel GetShippingAddressByID(int AddressID)
        {
            return uow.MstShippingAddresses.GetAll().Where(x => x.shippingId == AddressID).FirstOrDefault();
        }

        public MstBillingAddressModel GetBillingAddressByID(int AddressID)
        {
            return uow.MstBillingAddresses.GetAll().Where(x => x.billingId == AddressID).FirstOrDefault();
        }

        public int AddShippingAddress(AddressViewModel obj)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressViewModel, MstShippingAddressModel>();
            });
            IMapper mapper = config.CreateMapper();
            MstShippingAddressModel objBA = new MstShippingAddressModel();
            var newObj = mapper.Map(obj, objBA);
            newObj.createdOn = DateTime.Now;
            newObj.isActive = true;
            uow.MstShippingAddresses.Add(newObj);
            if (obj.isDefault == true)
            {
                List<MstShippingAddressModel> objLst = uow.MstShippingAddresses.Queryable().Where(x => x.loginID == newObj.loginID).ToList();
                for (int i = 0; i < objLst.Count; i++)
                {
                    objLst[i].isDefault = false;
                    uow.MstShippingAddresses.Edit(objLst[i]);
                }
            }

            return uow.Save();
        }

        public int AddBillingAddress(AddressViewModel obj)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressViewModel, MstBillingAddressModel>();
            });
            IMapper mapper = config.CreateMapper();
            MstBillingAddressModel objBA = new MstBillingAddressModel();
            var newObj = mapper.Map(obj, objBA);
            newObj.createdOn = DateTime.Now;
            newObj.isActive = true;
            uow.MstBillingAddresses.Add(newObj);
            return uow.Save();
        }

        public int UpdateShippingAddress(AddressViewModel obj)
        {
            int rowCount = 0;
            var oldObj = uow.MstShippingAddresses.GetAll().Where(x => x.shippingId == obj.shippingId).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressViewModel, MstShippingAddressModel>()
                    .ForMember(x => x.createdOn, opt => opt.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            MstShippingAddressModel objOld = GetShippingAddressByID(obj.shippingId);
            if (objOld != null)
            {
                var newObj = mapper.Map(obj, objOld);
                newObj.updatedOn = DateTime.Now;
                if (obj.isDefault == true)
                {
                    List<MstShippingAddressModel> objLst = uow.MstShippingAddresses.Queryable().Where(x => x.loginID == newObj.loginID && x.shippingId != newObj.shippingId).ToList();
                    for (int i = 0; i < objLst.Count; i++)
                    {
                        objLst[i].isDefault = false;
                        uow.MstShippingAddresses.Edit(objLst[i]);
                    }
                }
                rowCount = uow.Save();
            }
            return rowCount;
        }

        public int UpdateBillingAddress(AddressViewModel obj)
        {
            int rowCount = 0;
            var oldObj = uow.MstBillingAddresses.GetAll().Where(x => x.billingId == obj.billingId).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressViewModel, MstBillingAddressModel>()
                    .ForMember(x => x.createdOn, opt => opt.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            MstBillingAddressModel objOld = GetBillingAddressByID(obj.billingId);
            if (objOld != null)
            {
                var newObj = mapper.Map(obj, objOld);
                newObj.updatedOn = DateTime.Now;
                rowCount = uow.Save();
            }
            return rowCount;
        }

        public int UpdateAddressInActive(int AddressID, int LoginID, string Type)
        {
            int RowCount = 0;
            if (Type == "shipping")
            {
                var oldObj = uow.MstShippingAddresses.GetAll().Where(x => x.shippingId == AddressID && x.loginID == LoginID).FirstOrDefault();
                if (oldObj != null)
                {
                    oldObj.isActive = false;
                    oldObj.updatedOn = DateTime.Now;
                    uow.MstShippingAddresses.Edit(oldObj);
                    RowCount = uow.Save();
                }
            }
            else
            {
                var oldObj = uow.MstBillingAddresses.GetAll().Where(x => x.billingId == AddressID && x.loginID == LoginID).FirstOrDefault();
                if (oldObj != null)
                {
                    oldObj.isActive = false;
                    oldObj.updatedOn = DateTime.Now;
                    uow.MstBillingAddresses.Edit(oldObj);
                    RowCount = uow.Save();
                }
            }
            return RowCount;
        }

        public List<Select2Option> GetCustomers(string Filter)
        {
            return (from bill in uow.MstBillingAddresses.Queryable()
                    join l in uow.LoginDetails.Queryable() on bill.loginID equals l.loginID
                    where l.roleID == 3 && l.userStatus == 1
                    select new Select2Option
                    {
                        text = bill.companyName + "-" + bill.firstName + " " + bill.lastName,
                        id = bill.loginID
                    }).Where(x => x.text.Contains(Filter)).ToList();
        }

        public IQueryable<CustomerListView> CustomerList()
        {
            return uow.CustomerListView.Queryable();
        }

        public CustomerListView GetCustomerByLoginID(int LoginID)
        {
            return uow.CustomerListView.Queryable().Where(x => x.loginID == LoginID).FirstOrDefault();
        }

        public int ApproveCustomer(int LoginID, int isApproved, int ApprovedBy)
        {
            LoginDetailModel objLD = this.uow.LoginDetails.Queryable().Where(x => x.loginID == LoginID && x.isApproved == 0).FirstOrDefault();
            if (objLD != null)
            {
                objLD.isApproved = isApproved;
                objLD.approvedBy = ApprovedBy;
                objLD.approvedOn = DateTime.Now;
                this.uow.LoginDetails.Edit(objLD);
                return this.uow.Save();
            }
            return 0;
        }

        public void SendMailOnApproval(int CustomerId, string TemplatePath)
        {
            CustomerListView objLV = this.uow.CustomerListView.GetAll().Where(x => x.loginID == CustomerId).FirstOrDefault();
            LoginDetailModel objLD = this.uow.LoginDetails.GetAll().Where(x => x.loginID == CustomerId).FirstOrDefault();
            if (objLV != null && objLD != null)
            {

                StringBuilder sbMailTemplate = new StringBuilder();
                sbMailTemplate.Append(System.IO.File.ReadAllText(TemplatePath));
                List<string> lstOfEmailIDs = new List<string>();

                lstOfEmailIDs.Add(objLV.emailId);
                sbMailTemplate = sbMailTemplate.Replace("${CustomerName}", objLV.firstName + " " + objLV.lastName);
                sbMailTemplate = sbMailTemplate.Replace("${Username}", objLD.username);
                sbMailTemplate = sbMailTemplate.Replace("${Password}", objLD.password);
                MailUtility objMu = new MailUtility();
                objMu.SendMail(lstOfEmailIDs, "Account approved @ rosyblueonline.com", true, sbMailTemplate.ToString());
            }
        }


        public IQueryable<UserDetailView> QueryableUserDetail()
        {
            return this.uow.UserDetailView.Queryable();
        }

        public int ResetPassword(int LoginID, string OldPassword, string NewPassword)
        {
            LoginDetailModel objLDM = this.uow.LoginDetails.Queryable().Where(x => x.loginID == LoginID).FirstOrDefault();
            if (objLDM != null)
            {
                if (objLDM.password != OldPassword)
                {
                    throw new UserDefinedException("Invalid old password");
                }
                objLDM.password = NewPassword;
                objLDM.modifiedBy = LoginID;
                objLDM.modifiedDate = DateTime.Now;
                this.uow.LoginDetails.Edit(objLDM);
                return this.uow.Save();
            }
            return 0;
        }
        public IQueryable<UserDetailModel> QueryableUserDetailModel()
        {
            return this.uow.UserDetails.Queryable();
        }

        #region Block Site
        public IQueryable<BlockSiteHistoryModel> QueryableBlockSiteHistory()
        {
            return uow.BlockSiteHistoryModel.Queryable();
        }
        public BlockSiteHistoryModel GetLastBlockSiteHistory()
        {
            return QueryableBlockSiteHistory().OrderByDescending(x => x.Createdon).FirstOrDefault();
        }

        public int AddLastBlockSiteHistory(BlockSiteHistoryModel obj)
        {
            BlockSiteHistoryModel objBS = GetLastBlockSiteHistory();
            obj.Isblocked = objBS == null ? true : !objBS.Isblocked;
            obj.Createdon = DateTime.Now;
            uow.BlockSiteHistoryModel.Add(obj);
            return uow.Save();
        }


        public List<MenuMasterModel> MenuMasterDetails()
        {
            List<MenuMasterModel> objLDM = this.uow.MenuMaster.Queryable().Where(x => x.IsActive == true && x.MenuId!=1 && x.MenuId!=2).ToList();
            
            return objLDM;
        }

        public List<UserMenuPermissionModel> GetMenuAccessdata(int LoginId)
        {
            List<UserMenuPermissionModel> objLDM = this.uow.UserMenuPermission.Queryable().Where(x => x.IsActive == true && x.LoginId == LoginId).ToList();
              
            return objLDM;
        }

        public int AddUpdateSearchPermission(string startSizePermitted, string rowDownloadPermitted,int SPLoginId,int LoginId,  int OriginStatus)
        {
            MstCustomerPermisionModel objLDM = this.uow.mstCustomerPermision.Queryable().Where(x => x.customerId == SPLoginId).FirstOrDefault();
            if (objLDM != null)
            {
                objLDM.startSizePermitted = Convert.ToDouble(startSizePermitted);
                objLDM.rowDownloadPermitted = Convert.ToInt32(rowDownloadPermitted);
                objLDM.UpdateDate = DateTime.Now;
                objLDM.updatedBy = LoginId;
                objLDM.isActive = true;
                objLDM.isOriginFilterPermitted =Convert.ToBoolean(OriginStatus);
                this.uow.mstCustomerPermision.Edit(objLDM);
                return this.uow.Save();
            }
            else {

                LoginDetailModel objLDM1 = this.uow.LoginDetails.Queryable().Where(x => x.loginID == SPLoginId).FirstOrDefault();

                MstCustomerPermisionModel objBS =new  MstCustomerPermisionModel();
                objBS.customerId = SPLoginId;
                objBS.roleID = objLDM1.roleID;
                objBS.startSizePermitted = Convert.ToDouble(startSizePermitted);
                objBS.rowDownloadPermitted = Convert.ToInt32(rowDownloadPermitted);
                objBS.isActive = true;
                objBS.createDate = DateTime.Now;
                objBS.createdBy = LoginId;
                objBS.isOriginFilterPermitted = Convert.ToBoolean(OriginStatus);
                uow.mstCustomerPermision.Add(objBS);
                return uow.Save();

            }
            return 0;
        }


        #endregion
    }
}
