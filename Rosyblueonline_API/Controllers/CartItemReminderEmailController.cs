using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http;
namespace Rosyblueonline_API.Controllers
{
    [RoutePrefix("api/CartItemReminderEmail")]
    public class CartItemReminderEmailController : ApiController
    {
        IOrderService objOrderService;
        IStockDetailsService objStockDetailsService;
        public CartItemReminderEmailController(IOrderService objOrderService, IStockDetailsService objStockDetailsService)
        {
            this.objOrderService = objOrderService as OrderService;
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
        }


        [HttpGet]
        [Route("ReminderEmail")]
        public void ReminderEmail()
        {

            try
            {
                string procName = "proc_CartItemReminderEmailList";
                List<CartItemReminderEmailListModel> objRE = new List<CartItemReminderEmailListModel>();
                objRE = objStockDetailsService.CartItemReminderEmails(procName);
                if (objRE != null)
                {
                    for (int i = 0; i < objRE.Count(); i++)
                    {

                        objOrderService.CartItemReminderEmail(objRE[i].loginID, objRE[i].emailId.ToString(), objRE[i].LotNOs.ToString(), "Customer cart items reminder @ www.rosyblueonline.com");

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Log("CartItemReminderEmail", "ReminderEmail", ex);
            }

        }

    }
}
