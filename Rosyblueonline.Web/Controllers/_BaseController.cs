using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    public class _BaseController : Controller
    {
        public string GetToken()
        {
            TokenLogModel objToken = (TokenLogModel)Session["Token"];
            if (objToken != null)
            {
                return objToken.tokenID;
            }
            return null;
        }

        public string GetFullName()
        {
            TokenLogModel objToken = (TokenLogModel)Session["Token"];
            if (objToken != null)
            {
                return objToken.FullName;
            }
            return null;
        }

        public int GetLogin()
        {
            TokenLogModel objToken = (TokenLogModel)Session["Token"];
            if (objToken != null)
            {
                return objToken.loginID;
            }
            return 0;
        }

        public int GetRole()
        {
            TokenLogModel objToken = (TokenLogModel)Session["Token"];
            if (objToken != null)
            {
                return objToken.RoleID;
            }
            return 0;
        }

        public string GetEmailID()
        {
            TokenLogModel objToken = (TokenLogModel)Session["Token"];
            if (objToken != null)
            {
                return objToken.EmailID;
            }
            return "";
        }



    }
}