 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http; 
using System.IO;
using System.Web;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosyblueonline.Framework; 
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Models.ViewModel;

namespace RosyblueORRA_API.Controllers.webapi

{
    [RoutePrefix("api/Stock")]
    public class StockController : ApiController 
    {
        IStockDetailsService objStockDetailsService;

        public StockController(IStockDetailsService objStockDetailsService)
        {
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;


        } 

        [HttpGet]
        [Route("test")]
        public Response test()
        {
            inventoryDetailsViewModel obj = new inventoryDetailsViewModel();

            return new Response { Code = 500, IsSuccess = false, Result =2 };

        }
    }
}
