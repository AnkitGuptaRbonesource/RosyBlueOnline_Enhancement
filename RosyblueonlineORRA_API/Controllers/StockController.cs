using Rosyblueonline.Framework;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using System.Configuration;
using Rosyblueonline.ServiceProviders.Abstraction;

namespace RosyblueonlineORRA_API.Controllers
{
    [RoutePrefix("api/Stock")]
    public class StockController : ApiController
    {
        IOrderService objOrderService;
        public StockController(IOrderService objOrderService)
        {
            this.objOrderService = objOrderService as OrderService;
        }


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

        [HttpGet]
        [Route("PlaceOrder")]
        public Response PlaceOrder(string LotNos)
        {
            try
            {
                //UnitOfWork uow = null;
                //DBSQLServer db1 = null; 
                //OrderService objOrderService  = new OrderService(uow, db1);
                
                PlaceOrderOrra  obj = db.Database.SqlQuery<PlaceOrderOrra>("Exec  proc_PlaceOrderFromAPI 0,"+LotNos.ToString()).FirstOrDefault();
                if (obj.OrderId > 0)
                {
                    objOrderService.SendMailPreBookOrder(obj.OrderId, obj.CustomerId , ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderAdmin"].ToString(), "Customer order details @ www.rosyblueonline.com");
                    objOrderService.SendMailPreBookOrder(obj.OrderId, obj.CustomerId,  ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderCustomer"].ToString(), "Your order details @ www.rosyblueonline.com", true);

                }
                return new Response { Code = 200, IsSuccess = true, Message = "Order placed", Result = obj };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Order", "PlaceOrder", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }



    }
}