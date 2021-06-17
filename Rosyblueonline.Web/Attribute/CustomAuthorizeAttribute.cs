using Rosyblueonline.Models;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rosyblueonline.Web.Attribute
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string AllowPage;

        private DataContext db = null;
        public CustomAuthorizeAttribute(string PageAccess)
        {
            this.AllowPage = PageAccess;

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            TokenLogModel objToken = new TokenLogModel();
            objToken = (TokenLogModel)httpContext.Session["Token"];

            if (objToken == null)
            {
                return authorize;
            }
            var LoginId = Convert.ToString(objToken.loginID);


            if (!string.IsNullOrEmpty(LoginId))
                using (var context = new DataContext())
                {



                    var MenuIds = (from u in context.MenuMaster
                                  where u.MenuName == AllowPage
                                  select new
                                  { u.MenuId }).FirstOrDefault();

                    int MenuId = Convert.ToInt32(MenuIds.MenuId);
                    int LoginIds = Convert.ToInt32(LoginId);
                    var MenuDetails = (from u in context.UserMenuPermission
                                       where u.MenuId == MenuId && u.LoginId == LoginIds && u.IsActive == true
                                       select new
                                       {
                                           u.MenuId
                                       }).FirstOrDefault();
                    authorize= MenuDetails == null ? false : true;

                }

            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        { 
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }


    }
}