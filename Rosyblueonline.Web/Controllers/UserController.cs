using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Rosyblueonline.Framework.Constant;

namespace Rosyblueonline.Web.Controllers
{
    public class UserController : _BaseController
    {
        private readonly UserDetailService objUDSvc = null;
        private readonly IHomeServiceProvider objHSSvc = null;
        private readonly IDownloadScriptService objDSSvc = null;
        public UserController(IUserDetailService objUDSvc, IHomeServiceProvider objHSSvc, IDownloadScriptService objDSSvc)
        {
            this.objUDSvc = objUDSvc as UserDetailService;
            this.objHSSvc = objHSSvc as HomeServiceProvider;
            this.objDSSvc = objDSSvc as DownloadScriptService;
        }

        // GET: User
        public ActionResult Index(bool OnlyAddCustomer = false)
        {
            UserRegistrationViewModel objVm = new UserRegistrationViewModel();
            objVm.Country = this.objHSSvc.GetCountry().ToList();
            objVm.State = this.objHSSvc.GetState(0).ToList();
            objVm.OnlyAddCustomer = OnlyAddCustomer;
            return View(objVm);
        }

        public ActionResult Add()
        {
            return new JsonResult();
        }

        [HttpPost]
        public ActionResult Registration(UserRegistrationViewModel obj, Roles roles)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    bool status = this.objUDSvc.RegisterUser(obj, roles);
                    if (status == true)
                    {
                        return Json(new Response { IsSuccess = true, Code = 200, Result = status });
                    }
                }
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = "Validation error" });
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "Registration", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult RegistrationViaMemo(UserRegistrationViewModelViaMemo obj)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    bool status = this.objUDSvc.RegisterUser(obj);
                    if (status == true)
                    {
                        return Json(new Response { IsSuccess = true, Code = 200, Result = status });
                    }
                }
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = "Validation error" });
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "RegistrationViaMemo", ex);
                throw ex;
            }
        }

        //[HttpPost]
        //public ActionResult Registration(UserRegistrationViewModelViaMemo obj)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid == true)
        //        {
        //            bool status = this.objUDSvc.RegisterUser(obj);
        //            if (status == true)
        //            {
        //                return Json(new Response { IsSuccess = true, Code = 200, Result = status });
        //            }
        //        }
        //        return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = "Validation error" });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (typeof(UserDefinedException) == ex.GetType())
        //        {
        //            return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
        //        }
        //        ErrorLog.Log("UserController", "Registration Via Memo", ex);
        //        throw ex;
        //    }

        //}

        [HttpPost]
        public ActionResult UpdateRegistration(UserRegistrationViewModel obj, Roles roles)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    bool status = this.objUDSvc.UpdateRegisterUser(obj, roles, LoginID);
                    if (status == true)
                    {
                        bool log = this.objUDSvc.UserActivitylogs(LoginID, "Update Profile", "Update Successfully.");

                        return Json(new Response { IsSuccess = true, Code = 200, Result = status });
                    }
                }
                return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = "Validation error" });
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "UpdateRegistration", ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UserDetailForGrid(DataTableViewModel objReq, int RoleID = 0)
        {
            try
            {
                if (objReq != null)
                {
                    DataTableResponse<UserDetailView> objResp = new DataTableResponse<UserDetailView>();
                    IQueryable<UserDetailView> query = this.objUDSvc.QueryableUserDetail();
                    if (RoleID != 0)
                    {
                        query = query.Where(x => x.roleID == RoleID);
                    }
                    if (!string.IsNullOrEmpty(objReq.search.value))
                    {
                        query = query.Where(x => (x.firstName + " " + x.lastName).Contains(objReq.search.value) ||
                                                 x.companyName.Contains(objReq.search.value) ||
                                                 x.mobile.Contains(objReq.search.value) ||
                                                 x.username.Contains(objReq.search.value) ||
                                                 x.countryName.Contains(objReq.search.value));
                    }
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "firstName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.firstName);
                                else
                                    query = query.OrderByDescending(x => x.firstName);
                                break;
                            case "companyName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.companyName);
                                else
                                    query = query.OrderByDescending(x => x.companyName);
                                break;
                            case "roleID":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.roleID);
                                else
                                    query = query.OrderByDescending(x => x.roleID);
                                break;
                            case "countryName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.countryName);
                                else
                                    query = query.OrderByDescending(x => x.countryName);
                                break;
                            case "username":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.username);
                                else
                                    query = query.OrderByDescending(x => x.username);
                                break;

                            default:
                            case "loginID":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.loginID);
                                else
                                    query = query.OrderByDescending(x => x.loginID);
                                break;
                        }
                    }
                    objResp.draw = objReq.draw;
                    objResp.recordsFiltered = query.Count();
                    objResp.data = query.Skip(objReq.start).Take(objReq.length).ToList();
                    return Json(objResp);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "UserDetailForGrid", ex);
                throw ex;
            }
        }

        [CheckSession]
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ResetPassword(string oldpassword, string password)
        {
            try
            {
                int LoginID = GetLogin();
                if (LoginID > 0)
                {
                    int RowCount = this.objUDSvc.ResetPassword(LoginID, oldpassword, password);

                    bool log = this.objUDSvc.UserActivitylogs(LoginID, "Reset Password", "Reset Successfully.");

                    return Json(new Response { IsSuccess = true, Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") });
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "ResetPassword", ex);
                throw ex;
            }
        }

        [CheckSession]
        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUserDetail(int qLoginID = 0)
        {
            try
            {
                int LoginID = qLoginID == 0 ? GetLogin() : qLoginID;
                if (LoginID > 0)
                {
                    UserDetailModel objUD = this.objUDSvc.QueryableUserDetailModel().Where(x => x.loginID == LoginID).FirstOrDefault();
                    AddressViewModel objBA = this.objUDSvc.GetBillingAddresses(LoginID, true).FirstOrDefault();
                    LoginDetailModel objLD = this.objUDSvc.GetLoginDetailByLoginID(LoginID);
                    objLD.password = "";
                    return Json(new Response { IsSuccess = true, Result = new { UserDetail = objUD, Address = objBA, Login = objLD } }, JsonRequestBehavior.AllowGet);
                }
                return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "GetUserDetail", ex);
                throw ex;
            }
        }

        //[HttpGet]
        //public ActionResult GetUserDetail(int LoginID = 0)
        //{
        //    if (LoginID > 0)
        //    {
        //        UserDetailModel objUD = this.objUDSvc.QueryableUserDetailModel().Where(x => x.loginID == LoginID).FirstOrDefault();
        //        AddressViewModel objBA = this.objUDSvc.GetBillingAddresses(LoginID, true).FirstOrDefault();
        //        LoginDetailModel objLD = this.objUDSvc.GetLoginDetailByLoginID(LoginID);
        //        objLD.password = "";
        //        return Json(new Response { IsSuccess = true, Result = new { UserDetail = objUD, Address = objBA, Login = objLD } }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new Response { IsSuccess = false, Message = string.Format(StringResource.Invalid, "Session") }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult BlockSite()
        {
            BlockSiteHistoryModel objBS = this.objUDSvc.GetLastBlockSiteHistory();
            bool isSiteBlocked = objBS == null ? false : objBS.Isblocked;
            return View(isSiteBlocked);
        }

        public ActionResult UpdateBlockSite(string Password)
        {
            try
            {
                int LoginID = base.GetLogin();
                int RoleID = base.GetRole();
                string TokenID = base.GetToken();
                bool isLoggedIn = this.objUDSvc.Login(TokenID, Password);
                if (isLoggedIn)
                {
                    int RowCount = this.objUDSvc.AddLastBlockSiteHistory(new BlockSiteHistoryModel
                    {
                        Blockid = 0,
                        Createdon = DateTime.Now,
                        Isblocked = false,
                        Loginid = LoginID,
                        Roleid = RoleID,
                        TokenID = TokenID
                    });
                    return Json(new Response { IsSuccess = true, Result = RowCount });
                }
                return Json(new Response { IsSuccess = false, Message = "Invalid password" });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("UserController", "UpdateBlockSite", ex);
                return Json(new Response { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult ManageRights()
        {
            ManageRightsViewModel objMR = new ManageRightsViewModel();
            objMR.Users = this.objUDSvc.QueryableUserDetail().Where(x => x.roleID == 8 || x.roleID == 9).Select(x => new SelectOptionsViewModel { Text = x.firstName + " " + x.lastName, Value = x.loginID.ToString() }).ToList();
            objMR.Menus = this.objDSSvc.GetForDisplay();
            objMR.Data = this.objDSSvc.GetDownloadRightView().Where(x => x.roleID == 8 || x.roleID == 9).ToList();
            return View(objMR);
        }

        [HttpGet]
        [CheckSession]
        public ActionResult GetRightForUser(int LoginID)
        {
            List<DownloadRightModel> objLst = this.objDSSvc.GetByLoginID(LoginID);
            return Json(objLst, JsonRequestBehavior.AllowGet);
        }

        [CheckSession]
        public ActionResult PutRightForUser(List<DownloadRightModel> objLst)
        {
            try
            {
                int RowCount = this.objDSSvc.UpdateRights(objLst);
                return Json(new Response { Code = 200, IsSuccess = true, Result = RowCount });
            }
            catch (Exception ex)
            {
                ErrorLog.Log("UserController", "PutRightForUser", ex);
                return Json(new Response { Code = 500, IsSuccess = false, Message = ex.Message });
            }

        }


        [HttpPost]
        public JsonResult UserMenuDetailForGrid(DataTableViewModel objReq, int RoleID = 0)
        {
            try
            {
                if (objReq != null)
                {
                    DataTableResponse<UserDetailView> objResp = new DataTableResponse<UserDetailView>();
                    IQueryable<UserDetailView> query = this.objUDSvc.QueryableUserDetail().Where(x => x.roleID != 3 && x.roleID != 4);
                    if (RoleID != 0)
                    {
                        query = query.Where(x => x.roleID == RoleID);
                    }
                    if (!string.IsNullOrEmpty(objReq.search.value))
                    {
                        query = query.Where(x => (x.firstName + " " + x.lastName).Contains(objReq.search.value) ||
                                                 x.companyName.Contains(objReq.search.value) ||
                                                 x.mobile.Contains(objReq.search.value) ||
                                                 x.username.Contains(objReq.search.value) ||
                                                 x.countryName.Contains(objReq.search.value));
                    }
                    objResp.recordsTotal = query.Count();
                    for (int i = 0; i < objReq.order.Count; i++)
                    {
                        int idx = Convert.ToInt32(objReq.order[i].column);
                        switch (objReq.columns[idx].data)
                        {
                            case "firstName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.firstName);
                                else
                                    query = query.OrderByDescending(x => x.firstName);
                                break;
                            case "companyName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.companyName);
                                else
                                    query = query.OrderByDescending(x => x.companyName);
                                break;
                            case "roleID":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.roleID);
                                else
                                    query = query.OrderByDescending(x => x.roleID);
                                break;
                            case "countryName":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.countryName);
                                else
                                    query = query.OrderByDescending(x => x.countryName);
                                break;
                            case "username":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.username);
                                else
                                    query = query.OrderByDescending(x => x.username);
                                break;

                            default:
                            case "loginID":
                                if (objReq.order[i].dir == "asc")
                                    query = query.OrderBy(x => x.loginID);
                                else
                                    query = query.OrderByDescending(x => x.loginID);
                                break;
                        }
                    }
                    objResp.draw = objReq.draw;
                    objResp.recordsFiltered = query.Count();
                    objResp.data = query.Skip(objReq.start).Take(objReq.length).ToList();
                    return Json(objResp);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (typeof(UserDefinedException) == ex.GetType())
                {
                    return Json(new Response { IsSuccess = false, Code = 500, Result = "", Message = ex.Message });
                }
                ErrorLog.Log("UserController", "UserDetailForGrid", ex);
                throw ex;
            }
        }

    }
}
