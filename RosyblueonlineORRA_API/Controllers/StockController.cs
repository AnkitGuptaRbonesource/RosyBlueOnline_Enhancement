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
using Microsoft.IdentityModel.Tokens;   
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Rosyblueonline.Models.ViewModel;
using Newtonsoft.Json;

namespace RosyblueonlineORRA_API.Controllers
{
    [RoutePrefix("api/Stock")]
    public class StockController : ApiController
    {
        IOrderService objOrderService;
        IUserDetailService objUDSvc = null;
        IStockDetailsService objStockDetailsService;
        public StockController(IOrderService objOrderService, IUserDetailService objUDSvc, IStockDetailsService objStockDetailsService)
        {
            this.objOrderService = objOrderService as OrderService;
            this.objUDSvc = objUDSvc as UserDetailService;
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
        }

        DataContext db = new DataContext();
        [Authorize]
        [HttpGet]
        [Route("GetData")]
        public Response GetData()
        {
            try
            {

                List<ORRAStockDetailsModel> obj = objStockDetailsService.GetORRAStockData(6,"STOCK_FOR_ORRA_API");
                return new Response { Code = 200, IsSuccess = true, Message = "Total Rows " + obj.Count().ToString(), Result = obj };

                //List<ORRAStockDetailsModel> obj = db.Database.SqlQuery<ORRAStockDetailsModel>("Exec  prcGetReports 6, 'STOCK_FOR_ORRA_API'").ToList();
                  // return new Response { Code = 200, IsSuccess = true, Message = "Total Rows " + obj.Count().ToString(), Result = obj };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Stock", "GetDate", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ValidateStock")]
        public Response ValidateStock(string LotNos)
        {
            try
            {

                ORRAStockDetailsValidate obj = objStockDetailsService.ORRAStockDetailsValidate(241, LotNos.ToString(), "VALIDATE_N_BLOCK_API_DATA_REQUEST");
                return new Response { Code = 200, IsSuccess = true, Message = "Validate Stock", Result = obj };


                //List<ORRAStockDetailsValidate> obj = db.Database.SqlQuery<ORRAStockDetailsValidate>("Exec  proc_ValidateAPIInventory 241," + LotNos.ToString()+ ",'VALIDATE_N_BLOCK_API_DATA_REQUEST'").ToList();

                //return new Response { Code = 200, IsSuccess = true, Message = "Validate Stock", Result = obj };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Stock", "ValidateStock", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }
        [Authorize]
        [HttpGet]
        [Route("PlaceOrder")]
        public Response PlaceOrder(string LotNos,int BlockedOrderId)
        {
            try
            {
                 
                List<PlaceOrderOrra> obj = objStockDetailsService.ORRAPlaceOrder(241, BlockedOrderId , LotNos.ToString());
                
                if (obj[0].OrderId > 0)
                {
                    objOrderService.SendMailPreBookOrder(obj[0].OrderId, obj[0].CustomerId, ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderAdmin"].ToString(), "Customer order details @ www.rosyblueonline.com");
                    objOrderService.SendMailPreBookOrder(obj[0].OrderId, obj[0].CustomerId, ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderCustomer"].ToString(), "Your order details @ www.rosyblueonline.com", true);
                    return new Response { Code = 200, IsSuccess = true, Message = "Order placed", Result = obj };

                }
                return new Response { Code = 200, IsSuccess = false, Message = "No Order placed", Result = obj };


            }
            catch (Exception ex)
            {
                ErrorLog.Log("Order", "PlaceOrder", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }


        [HttpGet]
        [Route("UserLogin")]
        public Response UserLogin(string UserName, string Password)
        {
            try
            {
                LoginViewModel obj = new LoginViewModel();
                obj.Username = UserName;
                obj.Password = Password;
                obj.DeviceName = "Test";
                obj.IpAddress = "1.0.1.0";

                TokenLogModel objToken = this.objUDSvc.Login(obj);
                if (objToken != null)
                {
                    var tokenString = GenerateJSONWebToken();
                    return new Response { Code = 200, IsSuccess = true, Result = tokenString, Message = "Login Successfully !" };

                }

                return new Response { Code = 200, IsSuccess = false, Result = null, Message = "Incorrect username and password !" };
            }
            catch (Exception ex)
            {
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }

        }


        private string GenerateJSONWebToken()
        {

            string key = "Secret key use for validation orra api web request";
            var issuer = "ORRA_API";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = new JwtSecurityToken(issuer,
                            issuer,
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt_token;
        }
        [HttpGet]
        [Route("test")]
        public string test()
        {
            var query = @"
                    query ReportQuery($ReportNumber: String!) {
                        getReport(report_number: $ReportNumber){
                            report_number
                            report_date
                            report_type
                            results {
                                __typename
                                ... on DiamondGradingReportResults {
                                    shape_and_cutting_style
                                    carat_weight
                                    clarity_grade
                                    color_grade
                                }
                            }
                            quota {
                                remaining
                            }
                        }
                    }"; 
            var reportNumber = "2141438171";

            // Construct the request body to be POSTed to the graphql server
            var query_variables = new Dictionary<string, string>
            {
              { "ReportNumber", reportNumber}
                };
            var body = new Dictionary<string, object>
                {
               { "query", query },
              { "variables", query_variables }
                };

            string json = JsonConvert.SerializeObject(body);

            //string json = serializer.Serialize(body);
            return json;
        }


    }
}