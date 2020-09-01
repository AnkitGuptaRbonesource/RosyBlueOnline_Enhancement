using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.Controllers
{
    public class DiamondSearchController : Controller
    {
        readonly StockDetailsService objSDS = null;
        public DiamondSearchController(IStockDetailsService objSDS)
        {
            this.objSDS = objSDS as StockDetailsService;
        }

        // GET: DiamondSearch
        public ActionResult Index()
        {
            return View();
        }

        // GET: DiamondSearch
        public ActionResult DiamondView(string id)
        {
            inventoryDetailsViewModel obj = null;
            if (!string.IsNullOrEmpty(id))
            {
                List<inventoryDetailsViewModel> objLst = this.objSDS.InventoryAction("6", "INV~" + id, "0", "50", "LotNumber", "asc", "SpecificSearch", "SpecialSearch");
                if (objLst.Count > 0)
                {
                    obj = objLst[0];
                }
            }
            return View(obj);
        }
    }
}