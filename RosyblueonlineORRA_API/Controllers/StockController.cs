using Rosyblueonline.Framework;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation; 
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using System.Data.SqlClient;
using Rosyblueonline.Models;
using Rosyblueonline.Repository.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosyblueonlineORRA_API.Controllers
{
    [RoutePrefix("api/Stock")]
    public class StockController : ApiController
    {
        //IStockDetailsService objStockDetailsService;
        //public StockController(IStockDetailsService objStockDetailsService)
        //{
        //    this.objStockDetailsService = objStockDetailsService as StockDetailsService;
        //}

        // DBSQLServer db = new DBSQLServer();
        //  DataContext db1 = new DataContext();
        // readonly UnitOfWork uow = null;
        // DBSQLServer dBSQL = new DBSQLServer();
          DataContext db = new  DataContext();
        [HttpGet]
        [Route("GetData")]
        public Response GetData()
        {
            try
            {
                // ORRAStockDetailsModel obj = new ORRAStockDetailsModel();
                //  List<ORRAStockDetailsModel> obj = this.objStockDetailsService.GetORRAStockData(6, "STOCK_FOR_ORRA_API");
                //string LoginID = "6";
                //string RaiseEvent = "STOCK_FOR_ORRA_API";

                // return this.db.Database.SqlQuery<T>(string.Format(Text, Parameters)).ToList();

                List<ORRAStockDetailsModel> obj =  db.Database.SqlQuery<ORRAStockDetailsModel>("Exec  prcGetReports 6, 'STOCK_FOR_ORRA_API'").ToList();

                //  List<ORRAStockDetailsModel> obj =this.uow.ExecuteQuery<ORRAStockDetailsModel>("Exec  prcGetReports 6, 'STOCK_FOR_ORRA_API'");

                return new Response { Code = 200, IsSuccess = true, Message = "Total Rows "+obj.Count().ToString(), Result = obj };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Stock", "GetDate", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }

    }
}