using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Owin;

namespace Rosyblueonline_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Rosyblueonline_API",   
                        ValidAudience = "Rosyblueonline_API",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret key use for validation orra api web request"))
                    }
                });
        }
    }
}