using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using SRVTextToImage;

namespace Rosyblueonline.Web.Controllers
{
    public class HomeController : _BaseController
    {
        private readonly IHomeServiceProvider objHomeServiceProvider;
        private readonly UserDetailService objUDSvc = null;
        public HomeController(IHomeServiceProvider objHomeServiceProvider, IUserDetailService objUDSvc)
        {
            this.objHomeServiceProvider = objHomeServiceProvider as HomeServiceProvider;
            this.objUDSvc = objUDSvc as UserDetailService;
        }


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.Cookies["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["CurrentCulture"].Value);
            }
        }

        public ActionResult ChangeCulture(string ddlCulture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Response.Cookies.Add(new HttpCookie("CurrentCulture", ddlCulture));

            return Json("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult UpcomingShows()
        {
            var mod = objHomeServiceProvider.GetUpcomingShow();
            return PartialView("_UpcomingShows", mod);
        }

        public ActionResult Registration()
        {
            RegistrationViewModel objVm = new RegistrationViewModel();
            objVm.Country = this.objHomeServiceProvider.GetCountry().ToList();
            objVm.State = this.objHomeServiceProvider.GetState(0).ToList();

            objVm.RTypes = this.objHomeServiceProvider.GetRTypes().ToList();  //Added by Ankit 27Jun2020

            objVm.RIdentityType = this.objHomeServiceProvider.GetRIdentityType().ToList();  //Added by Ankit 01July2020



            return View(objVm);
        }

        [HttpGet]
        public ActionResult GetCountries()
        {
            IEnumerable<SelectOptionsViewModel> objLst = this.objHomeServiceProvider.GetCountry();
            return Json(objLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetState(int CountryID)
        {
            IEnumerable<SelectOptionsViewModel> objLst = this.objHomeServiceProvider.GetState(CountryID);
            return Json(objLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListOfCustomer(string search)
        {
            var objLst = objUDSvc.GetCustomers(search);
            return Json(objLst, JsonRequestBehavior.AllowGet);
        }


        #region Registration

        [HttpPost]
        public ActionResult GenerateOtp(string EmailID)
        {
            try
            {
                //if (Regex.IsMatch(EmailID, RegexResource.Email))
                //{
                this.objUDSvc.GenerateOTP(EmailID);
                return Json(new Response { IsSuccess = true, Message = "", Result = true });
                //}
                //return Json(new Response { IsSuccess = false, Message = "Invalid EmailID" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GenerateOtp", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult VerifyOtp(string EmailID, string OTP)
        {
            try
            {
                bool Status = this.objUDSvc.VerifyOtp(EmailID, OTP);
                return Json(new Response { IsSuccess = true, Message = "", Result = Status });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "VerifyOtp", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public FileResult GetCaptchaImage()
        {
            CaptchaRandomImage cri = new CaptchaRandomImage();
            this.Session["CaptchaImageText"] = cri.GetRandomString(5);
            cri.GenerateImage(this.Session["CaptchaImageText"].ToString(), 300, 75, Color.DarkGray, Color.White);
            MemoryStream ms = new MemoryStream();
            cri.Image.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(ms, "image/png");
        }

        public ActionResult Register(RegistrationViewModel obj)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    int LoginID = GetLogin();
                    string AbsKycPath = ConfigurationManager.AppSettings["KycPath"];
                    string KycPath = string.Empty;
                    string CaptchaCode = Convert.ToString(this.Session["CaptchaImageText"]);
                    if (CaptchaCode != obj.Captchacode && LoginID == 0)
                    {
                        return Json(new Response { IsSuccess = false, Code = 100, Message = "Invalid captcha." });
                    }
                     
                    int KycCount = this.objUDSvc.CountUploadMultiDoc(obj.DocRandomID); 
                    if (KycCount ==0 )
                    {
                        return Json(new Response { IsSuccess = false, Code = 100, Message = "KYC doc not attached." });
                    }

                    //if (Request.Files.Count == 0)
                    //{
                    //    return Json(new Response { IsSuccess = false, Code = 100, Message = "KYC doc not attached" });
                    //}
                    //else
                    //{
                    //    KycPath = Server.MapPath(AbsKycPath);
                    //    string[] FileNames = Request.Files[0].FileName.Split('.');
                    //    //FileNames.Count()-1
                    //    Guid objGuid = Guid.NewGuid();
                    //    string FileName = objGuid.ToString() + "." + FileNames[FileNames.Length - 1];
                    //    Request.Files[0].SaveAs(KycPath + FileName);
                    //    obj.kycDocFile = FileName;
                    //}
                    obj.createdBy = LoginID;
                    bool status = this.objUDSvc.RegisterUser(obj, Constant.Roles.CUSTOMER);
                    if (status == true)
                    {
                        return Json(new Response { IsSuccess = true, Code = 200, Result = status });
                    }
                }
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = "Validation error" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "Register", ex);
                return Json(new Response { IsSuccess = false, Code = 500, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult CheckUserName(string UserName, int loginID)
        {
            try
            {
                bool Select = this.objUDSvc.CheckUsername(UserName, loginID);
                return Json(new Response { IsSuccess = true, Message = string.Format(StringResource.AlreadyTaken, "User Name"), Result = Select });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "CheckUserName", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
            //return Json(Select, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ForgetPassword(string EmailID)
        {
            try
            {
                string Domain = ConfigurationManager.AppSettings["Domain"].ToString();
                bool sentMail = this.objUDSvc.SendForgetPassword(EmailID, Domain + "/Home/ForgetPasswordReset");
                return Json(new Response { IsSuccess = true, Message = sentMail == true ? "" : "Mail not sent", Result = sentMail });
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(UserDefinedException))
                {
                    ErrorLog.Log("HomeController", "ForgetPassword", ex);
                }
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult ForgetPasswordReset(string v, int l)
        {
            return View();
        }

        public ActionResult ResetForgetPassword(int ID, string Code, string Password)
        {
            try
            {
                int ValidTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["ForgetPassword_ValidTimeout"].ToString());
                bool result = this.objUDSvc.ResetForgetPassword(ID, Code, Password, ValidTimeout);
                return Json(new Response { IsSuccess = true, Message = result == true ? "" : "Password not reset", Result = result });
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(UserDefinedException))
                {
                    ErrorLog.Log("HomeController", "ResetForgetPassword", ex);
                }
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        #endregion

        #region Login

        //// public ActionResult Login(LoginViewModel obj)

        public ActionResult Login(LoginData objd)

        {
            try
            {

                LoginViewModel obj = new LoginViewModel();
                obj.Username = objd.Username;
                obj.Password = objd.Password;
                obj.DeviceName = objd.DeviceName;
                obj.IpAddress = Request.UserHostAddress;

                objd.IpAddress = obj.IpAddress;

                //string LocationName = "Mumbai";
                //string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key=AIzaSyDWb21fb6IJQYHxyqorDXuQujgEAgbSzLY", objd.Latitude, objd.Longitude);
                //XElement xml = XElement.Load(url);
                //if (xml.Element("status").Value == "OK")
                //{
                //    LocationName = string.Format("{0}",
                //        xml.Element("result").Element("formatted_address").Value);
                //} 
                // objd.LocationName = LocationName;



                TokenLogModel objToken = this.objUDSvc.Login(obj);
              //  Session["Token"] = objToken;
                if (objToken != null)
                {
                    objd.LoginID = objToken.loginID;

                    if (objToken.RoleID == 3)
                    {
                        bool location = this.objUDSvc.GeoLocation(objd);
                        bool log = this.objUDSvc.UserActivitylogs(objToken.loginID, "User Login", "Login Successfully.");
                    }
                    BlockSiteHistoryModel objBS = this.objUDSvc.GetLastBlockSiteHistory();
                    objToken.IsSiteBlocked= objBS == null ? false : objBS.Isblocked;
                    if (objToken.IsSiteBlocked == true && objToken.RoleID == 3)
                    {
                        Session["Token"] = null;

                    }
                    else {
                        Session["Token"] = objToken;

                        UserMenuAccessModel  objAccess = new UserMenuAccessModel();
                        objAccess = this.objUDSvc.UserMenuAccessModel(objToken.loginID,"","", "MenuAccessDetails"); 
                        Session["MenuAccess"] = objAccess;


                    }
                    return Json(new Response { IsSuccess = true, Message = "", Result = objToken });
                }
                else
                {
                    // bool log = this.objUDSvc.UserActivitylog(objToken.loginID, "User Login", "Login Failed.");

                    return Json(new Response { IsSuccess = false, Message = StringResource.InvalidPassword, Result = objToken });
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GenerateOtp", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult LogOut()
        {
            TokenLogModel objToken = Session["Token"] as TokenLogModel;
            if (objToken != null)
            {
                Session["Token"] = null;
                Response.Cookies.Remove("TokenID");
                bool log = this.objUDSvc.UserActivitylogs(objToken.loginID, "User Logout", "Logout Successfuly.");

            }
            return Redirect("/Home/Index");
        }

        //public ActionResult ForgetPassword(string EmailID)
        //{
        //}

        #endregion

        #region Address Manager

        [HttpGet]
        [CheckSession]
        public ActionResult GetBillingAddresses()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<AddressViewModel> objLstBA = objUDSvc.GetBillingAddresses(LoginID, true);
                    return Json(new Response { IsSuccess = true, Message = "200", Result = objLstBA }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GetBillingAddresses", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [CheckSession]
        public ActionResult GetShippingAddresses()
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    List<AddressViewModel> objLstSA = objUDSvc.GetShippingAddresses(LoginID, true);
                    return Json(new Response { IsSuccess = true, Message = "200", Result = objLstSA }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GetShippingAddresses", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddAddress(AddressViewModel obj)
        {
            try
            {
                int RowCount = 0;
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    obj.isActive = true;
                    obj.loginID = LoginID;
                    if (obj.Type == "billing")
                    {
                        RowCount = obj.billingId == 0 ? objUDSvc.AddBillingAddress(obj) : objUDSvc.UpdateBillingAddress(obj);
                    }
                    else
                    if (obj.Type == "shipping")
                    {
                        RowCount = obj.shippingId == 0 ? objUDSvc.AddShippingAddress(obj) : objUDSvc.UpdateShippingAddress(obj);
                    }
                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Added " + obj.Type + " address", "Added Successfully.");


                    return Json(new Response { IsSuccess = true, Message = "200", Result = RowCount });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "AddAddress", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetByAddressID(int AddressID, string Type)
        {
            try
            {
                IAddressModel objAdd = null;
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    if (Type == "billing")
                    {
                        objAdd = objUDSvc.GetBillingAddressByID(AddressID);
                    }
                    else
                    if (Type == "shipping")
                    {
                        objAdd = objUDSvc.GetShippingAddressByID(AddressID);
                    }
                    return Json(new Response { IsSuccess = true, Message = "200", Result = objAdd }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GetByAddressID", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteAddress(int AddressID, string Type)
        {
            try
            {
                int RowCount = 0;
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    RowCount = objUDSvc.UpdateAddressInActive(AddressID, LoginID, Type);

                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Remove address", AddressID.ToString());

                    return Json(new Response { IsSuccess = true, Message = "200", Result = RowCount });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "DeleteAddress", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult ApproveCustomer(int LoginID, int isApproved)
        {
            try
            {
                int ApprovedLoginID = GetLogin();
                if (ApprovedLoginID > 0)
                {
                    int RowCount = this.objUDSvc.ApproveCustomer(LoginID, isApproved, ApprovedLoginID);
                    if (RowCount > 0)
                    {
                        this.objUDSvc.SendMailOnApproval(LoginID, Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate_CustomerRegistration"].ToString()));
                    }
                    return Json(new Response { IsSuccess = true, Message = "", Result = RowCount });
                }
                return Json(new Response { Code = 200, IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "ApproveCustomer", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult CheckSession()
        {
            TokenLogModel objToken = Session["Token"] as TokenLogModel;
            if (objToken != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }
        public ActionResult BlockedSite()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult TermsOfUse()
        {
            return View();
        }
        public ActionResult PricingDiscountPolicy()
        {
            return View();
        }

        //Added BY Ankit 01JUly2020

        public ActionResult MultipleDocUpload(UserDocDetailsModel obj)
        {
            try
            {
                //bool Status = this.objUDSvc.VerifyOtp(EmailID, OTP);
                //return Json(new Response { IsSuccess = true, Message = "", Result = Status });

                UserKycDocDetailsModel Kycobj = new UserKycDocDetailsModel();
                int LoginID = GetLogin();
                string AbsKycPath = ConfigurationManager.AppSettings["KycPath"];
                string KycPath = string.Empty;
                string OrgFileName = string.Empty;
                if (Request.Files.Count == 0)
                {
                    return Json(new Response { IsSuccess = false, Code = 100, Message = "KYC doc not attached" });
                }
                else
                {
                    KycPath = Server.MapPath(AbsKycPath);
                    string[] FileNames = Request.Files[0].FileName.Split('.');
                    Guid objGuid = Guid.NewGuid();
                    string FileName = objGuid.ToString() + "." + FileNames[FileNames.Length - 1];
                    Request.Files[0].SaveAs(KycPath + FileName);
                    Kycobj.kycDocFile = FileName;
                    OrgFileName = Request.Files[0].FileName;
                }
                Kycobj.LoginId = Convert.ToString(LoginID);
                Kycobj.KycDocId = obj.DocId;
                Kycobj.kycDocNo = obj.DocNo;
                //Kycobj.KycDocExpiryDate = DateTime.Parse(obj.DocExpiryDate);
                //Kycobj.KycDocExpiryDate= Convert.ToDateTime(obj.DocExpiryDate, System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                Kycobj.KycDocExpiryDate= DateTime.ParseExact(obj.DocExpiryDate, "dd-MM-yyyy", null); 
                Kycobj.LoginId = obj.DocRandomID;
                Kycobj.OrgFileName = OrgFileName;

                LoginDetailModel objLD = new LoginDetailModel();
                List<UserKycDocDetailsModel> objLstDoc = this.objUDSvc.UploadMultiDoc(Kycobj);


                return Json(objLstDoc.AsEnumerable().Select(r => new
                {
                    UserDocId= r.UserDocId,
                    LoginId= r.LoginId,
                    KycDocId= r.KycDocId,
                    KycDocName= r.KycDocName,
                    kycDocNo= r.kycDocNo,
                    kycDocFile= r.kycDocFile,
                    KycDocExpiryDate= r.KycDocExpiryDate.GetValueOrDefault().ToString("dd-MM-yyyy"),
                    OrgFileName = r.OrgFileName,

                }), JsonRequestBehavior.AllowGet);

                //return Json(objLstDoc, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "MultipleDocUpload", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult GenerateRandomId()
        {
            string ID = RandomHelpers.Instance.RandomString(6, true);
            return Json(new Response { IsSuccess = true, Message = "", Result = ID });

        }
         
        public ActionResult DeleteUserDoc(int UserDocId,string DocRandomID)
        {
            try
            { 
 
                List<UserKycDocDetailsModel> objLstDoc = this.objUDSvc.DeleteUserDoc(UserDocId, DocRandomID);
                if (objLstDoc.Count > 0)
                {
                    return Json(objLstDoc.AsEnumerable().Select(r => new
                    {
                        UserDocId = r.UserDocId,
                        LoginId = r.LoginId,
                        KycDocId = r.KycDocId,
                        KycDocName = r.KycDocName,
                        kycDocNo = r.kycDocNo,
                        kycDocFile = r.kycDocFile,
                        KycDocExpiryDate = r.KycDocExpiryDate.GetValueOrDefault().ToString("dd-MM-yyyy"),
                        OrgFileName = r.OrgFileName,

                    }), JsonRequestBehavior.AllowGet);
                }
                return Json(objLstDoc, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                ErrorLog.Log("HomeController", "GetUserDocDetails", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}