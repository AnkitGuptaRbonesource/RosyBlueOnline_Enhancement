using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    public class DownloadController : Controller
    {
        private readonly DownloadTemplateService objSvc = null;
        public DownloadController(IDownloadTemplateService objSvc)
        {
            this.objSvc = objSvc as DownloadTemplateService;
        }

        // GET: Download
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Template()
        {
            var Cols = objSvc.GetColumnForDisplay();
            return View(Cols);
        }
    }
}