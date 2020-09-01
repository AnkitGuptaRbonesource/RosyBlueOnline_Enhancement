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

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class HomeServiceProvider : IHomeServiceProvider
    {
        readonly UnitOfWork uow = null;

        public HomeServiceProvider(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        //public Response IsUserNameExists(string LoginName)
        //{
        //    try
        //    {
        //        bool result = _IsUserNameExists(LoginName);
        //        return new Response { IsSuccess = true, Message = Constant.SUCCESS, Result = result };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response { IsSuccess = false, Message = Constant.FAILURE };
        //    }
        //}

        //private bool _IsUserNameExists(string LoginName)
        //{
        //    return this.uow.LoginDetails.Any(a => a.loginName == LoginName);
        //}

        public Response IsUserEmailIdExists(string EmailId)
        {
            try
            {
                bool result = _IsUserEmailIDExists(EmailId);
                return new Response { IsSuccess = result, Message = Constant.SUCCESS, Result = result };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false, Message = Constant.FAILURE };
            }
        }

        private bool _IsUserEmailIDExists(string EmailId)
        {
            return this.uow.UserDetails.Any(a => a.emailId == EmailId);
        }

        public IEnumerable<SelectOptionsViewModel> GetCountry()
        {
            List<SelectOptionsViewModel> countries = this.uow.MstCountries.Queryable()
                .Where(x => x.isActive == true)
                .OrderBy(n => n.countryName)
                .AsEnumerable()
                .Select(x => new SelectOptionsViewModel { Value = (object)x.countryId, Text = x.countryName, Text2 = x.phonecode.ToString() }).ToList();

            return countries;
        }

        public IEnumerable<SelectOptionsViewModel> GetState(int CountryID)
        {
            List<SelectOptionsViewModel> states = uow.MstStates.Queryable()
                .Where(z => z.countryId == CountryID && z.isActive == true)
                .OrderBy(n => n.stateName).AsEnumerable()
                .Select(x => new SelectOptionsViewModel { Value = x.stateId, Text = x.stateName })
                .ToList();

            return states;
        }

        //Added by Ankit 27Jun2020
        public IEnumerable<SelectOptionsViewModel> GetRTypes()
        {
            List<SelectOptionsViewModel> RTypes = this.uow.MstTypes.Queryable()
                .Where(x => x.IsActive == true)
                .OrderBy(n => n.TypeName)
                .AsEnumerable()
                .Select(x => new SelectOptionsViewModel { Value = (object)x.TypeId, Text = x.TypeName  }).ToList();

            return RTypes;
        }
        public IEnumerable<SelectOptionsViewModel> GetRIdentityType()
        {
            List<SelectOptionsViewModel> RIdentityType = this.uow.MstDocIdentity.Queryable()
                .Where(x => x.IsActive == true)
                .OrderBy(n => n.DocIdentityName)
                .AsEnumerable()
                .Select(x => new SelectOptionsViewModel { Value = (object)x.DocId, Text = x.DocIdentityName }).ToList();

            return RIdentityType;
        }


        public List<UpcomingShowModel> GetUpcomingShow()
        {
            return this.uow.UpcomingShows.Queryable().Where(z => z.endDate > DateTime.Now).ToList();
        }

        public UserCountViewModel GetUserCounts(int LoginID)
        {
            UserCountViewModel objUC = new UserCountViewModel();
            objUC.Cart = uow.CartDetails.Queryable().Where(x => x.loginID == LoginID && x.isBlocked == false).Count();
            objUC.Orders = uow.Orders.Queryable().Where(x => x.createdBy == LoginID && x.orderType == 14).Count();
            objUC.OrdersPending = uow.Orders.Queryable().Where(x => x.createdBy == LoginID && x.orderType == 14 && x.orderStatus == 10).Count();
            objUC.OrdersCompleted = uow.Orders.Queryable().Where(x => x.createdBy == LoginID && x.orderType == 14 && x.orderStatus == 11).Count();
            objUC.WatchList = uow.WatchList.Queryable().Where(x => x.loginID == LoginID).Count();
            return objUC;
        }

        

        //public List<T> NewarrivalandRevised()
        //{
        //}

        //public IResponse ValidateRegisteredUser(LoginDetailsModel loginDetailsModel)
        //{
        //    var user = new RegisterNewUserAdapter().ConvertEntityToModel(new Repository<loginDetail>().GetAll().Where(x => x.loginName == loginDetailsModel.loginName && x.password == loginDetailsModel.password).FirstOrDefault());
        //    if (user != null)
        //    {
        //        if (user.loginID > 0)
        //        {
        //            return new Response { Code = user.loginID, IsSuccess = true, Message = Constant.SUCCESS };
        //        }
        //        else
        //        {
        //            return new Response { Code = user.loginID, IsSuccess = false, Message = Constant.FAILURE };
        //        }
        //    }
        //    else
        //    {
        //        return new Response { Code = 0, IsSuccess = false, Message = "Wrong UserName and Password" };
        //    }

        //}

        //public Response InsertNewUserRegistration(RegistrationViewModel registrationModel)
        //{
        //    try
        //    {
        //        GenericAdapter objGA = new GenericAdapter();
        //        LoginDetailModel objLD = objGA.Convert<LoginDetailViewModel, LoginDetailModel>(registrationModel.loginDetailsModel);
        //        if (_IsUserNameExists(objLD.loginName) == true)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = "Username already exists"
        //            };
        //        }
        //        if (_IsUserEmailIDExists(registrationModel.userDetailsModel.emailId) == true)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = "Email id already exists"
        //            };
        //        }

        //        objLD.createdOn = DateTime.Now;
        //        uow.LoginDetails.Add(objLD);
        //        uow.Save();

        //        UserDetailModel objUD = registrationModel.userDetailsModel;
        //        objUD.createdOn = DateTime.Now;
        //        objUD.loginID = objLD.loginID;
        //        uow.UserDetails.Add(objUD);

        //        MstBillingAddressModel objBA = registrationModel.mstBillingAddressModel;
        //        objBA.createdOn = DateTime.Now;
        //        objBA.loginID = objLD.loginID;
        //        uow.UserDetails.Add(objUD);

        //        uow.Save();

        //        uow.CommitTransaction();

        //        return new Response { IsSuccess = true, Message = "User Created." };
        //    }
        //    catch (Exception ex)
        //    {
        //        uow.RollbackTransaction();
        //        return new Response { IsSuccess = false, Message = Constant.FAILURE };
        //    }
        //}


    }
}
