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
    [CheckSession]
    public class MenuPermissionMasterController : _BaseController
    {
         
        private readonly UserDetailService objUDSvc = null;
        public MenuPermissionMasterController( IUserDetailService objUDSvc)
        { 
            this.objUDSvc = objUDSvc as UserDetailService;
        }
        // GET: MenuPermissionMaster
        public ActionResult Index()
        {

            List<MenuMasterModel> objmenu = new List<MenuMasterModel>();
            objmenu = this.objUDSvc.MenuMasterDetails();
          
            return View(objmenu);
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

        public ActionResult SaveMenuAccess(int UserId, string MenuIds)
        {

            int LoginId = GetLogin();
            UserMenuAccessModel objAccess = new UserMenuAccessModel();
            objAccess= this.objUDSvc.UserMenuAccessModel(UserId, MenuIds, LoginId.ToString(), "Insert");
            if (objAccess != null)
            {
                return Json(new Response { IsSuccess = true, Message = "", Result = objAccess });

            }
            else
            {

                return Json(new Response { IsSuccess = false, Message = "", Result = "" });
            }

        }

        public ActionResult GetMenuAccessdata(int UserId)
        {

            int LoginId = GetLogin();

            List<UserMenuPermissionModel> objAccess = new List<UserMenuPermissionModel>();
            objAccess = this.objUDSvc.GetMenuAccessdata(UserId); 
            if (objAccess != null && objAccess.Count>0)
            {
                return Json(new Response { IsSuccess = true, Message = "", Result = objAccess });

            }
            else
            {

                return Json(new Response { IsSuccess = false, Message = "", Result = "" });
            }

        }


        public ActionResult AddUpdateSearchPermission(string startSizePermitted, string rowDownloadPermitted, int SPLoginId, int OriginStatus, int AddtocartPermitted)
        {

            int LoginId = GetLogin();

            int rowcount = this.objUDSvc.AddUpdateSearchPermission(startSizePermitted, rowDownloadPermitted, SPLoginId, LoginId, OriginStatus, AddtocartPermitted);
            return Json(new Response { IsSuccess = true, Message = "", Result = rowcount });



        }




        // GET: MenuPermissionMaster/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuPermissionMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuPermissionMaster/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuPermissionMaster/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuPermissionMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuPermissionMaster/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuPermissionMaster/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
