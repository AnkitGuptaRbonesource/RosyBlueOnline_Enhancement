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

namespace Rosyblueonline_API.Controllers
{
    [RoutePrefix("api/Tanishq")]
    public class TanishqController : ApiController
    {

        IOrderService objOrderService;
        IUserDetailService objUDSvc = null;
        IStockDetailsService objStockDetailsService;
        public TanishqController(IOrderService objOrderService, IUserDetailService objUDSvc, IStockDetailsService objStockDetailsService)
        {
            this.objOrderService = objOrderService as OrderService;
            this.objUDSvc = objUDSvc as UserDetailService;
            this.objStockDetailsService = objStockDetailsService as StockDetailsService;
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
                obj.DeviceName = "TanishqApi";
                obj.IpAddress = "1.0.1.0";

                TokenLogModel objToken = this.objUDSvc.Login(obj);
                if (objToken != null && objToken.loginID== 12227)
                {
                    var tokenString = GenerateJSONWebToken();
                    return new Response { Code = 200, IsSuccess = true, Result = tokenString, Message = "Login Successfully !" };

                }

                return new Response { Code = 200, IsSuccess = false, Result = null, Message = "Incorrect username and password !" };
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "UserLogin", ex); 
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }

        }


        private string GenerateJSONWebToken()
        {

            string key = "Secret key use for validation orra api web request";
            var issuer = "Rosyblueonline_API";

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


        [Authorize]
        [HttpGet]
        [Route("GetStock")]
        public Response GetStock(string SearchCriteria)
        {
            try
            {

                List<BuildSearchCriterias> objS = new List<BuildSearchCriterias>();
                objS = objStockDetailsService.BuildSearchCriteria(SearchCriteria, 12227);
 
                List<TanishqStockModel> objT = new List<TanishqStockModel>(); 
                objT = objStockDetailsService.TanishqStockInventory("12227", objS[0].SearchCriteriaFinal, "0", "5000000", "LotNumber", "asc", "SpecificSearch","");
                return new Response { Code = 200, IsSuccess = true, Message = "Total Rows " + objT.Count().ToString(), Result = objT };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "GetStock", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }


        [Authorize]
        [HttpGet]
        [Route("AddToCart")]
        public Response AddToCart(string LotNos)
        {
            try
            {

                TanishqStockDetailsValidate obj = objStockDetailsService.TanishqStockDetailsValidate(12227, LotNos.ToString(), "TANISHQ_VALIDATE_N_BLOCK_API_DATA_REQUEST");
                return new Response { Code = 200, IsSuccess = true, Message = "AddToCart", Result = obj };
                 
            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "AddToCart", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }

        [Authorize]
        [HttpGet]
        [Route("PlaceOrder")]
        public Response PlaceOrder(string MergeOrderList)
        {
            try
            {

                List<TanishqPlaceOrder> obj = objStockDetailsService.TanishqPlaceOrder(12227, MergeOrderList);

                if (obj != null)
                {
                    if (obj[0].OrderId > 0)
                    { 
                       
                        string CCEmail = ConfigurationManager.AppSettings["TanishqCCEmail"].ToString();
                        string AdminCCEmail = ConfigurationManager.AppSettings["TanishqAdminCCEmail"].ToString();
                        string BCCEmail = "";

                        objOrderService.SendMailForApiOrderBook(obj[0].OrderId, obj[0].CustomerId, ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderAdmin"].ToString(), "Customer order details @ www.rosyblueonline.com", CCEmail, BCCEmail, false);
                        objOrderService.SendMailForApiOrderBook(obj[0].OrderId, obj[0].CustomerId, ConfigurationManager.AppSettings["EmailTemplate_PlaceOrderCustomer"].ToString(), "Your order details @ www.rosyblueonline.com", CCEmail, BCCEmail, true);
                        return new Response { Code = 200, IsSuccess = true, Message = "Order placed", Result = obj };

                    }
                }
                return new Response { Code = 200, IsSuccess = false, Message = "No Order placed", Result = obj };


            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "PlaceOrder", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }


        [Authorize]
        [HttpGet]
        [Route("GetSoldInventoryDetails")]
        public Response GetSoldInventoryDetails()
        {
            try
            { 
                List<TanishqStockModel> objT = new List<TanishqStockModel>();
                objT = objStockDetailsService.TanishqSoldStockInventory("12227");
                return new Response { Code = 200, IsSuccess = true, Message = "Total Rows " + objT.Count().ToString(), Result = objT };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "GetSoldInventoryDetails", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }

        [Authorize]
        [HttpGet]
        [Route("RemoveFromCart")]
        public Response RemoveFromCart(string LotNos)
        {
            try
            {

                List<RemoveFromCartInventory> obj = objStockDetailsService.RemoveFromCart(LotNos.ToString(), "12227" );
                return new Response { Code = 200, IsSuccess = true, Message = "RemoveFromCart", Result = obj };

            }
            catch (Exception ex)
            {
                ErrorLog.Log("Tanishq", "RemoveFromCart", ex);
                return new Response { Code = 500, IsSuccess = false, Message = ex.Message };
            }
        }


    }
}
