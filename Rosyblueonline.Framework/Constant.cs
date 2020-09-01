using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    public static class Constant
    {
        public const string APPLICATION = "RosyBlue";
        public const string SUCCESS = "Success";
        public const string FAILURE = "Fail";

        public const string DevAndTestServers = "DEV";

        //public const string USERTYPE_ADMINUSER = "AdminUser";
        //public const string UserType_SUPPORTADMINUSER = "SupportAdminUser";
        //public const string UserType_CUSTOMERUSER = "CustomerUser";
        //public const string UserType_SUBCUSTOMERUSER = "SubCustomerUser";

        public enum Codes
        {
            TRANSACTION_SUCCESS = 1,
            TRANSACTION_FAIL = 2
        }

        public enum Roles
        {
            SUPERADMIN = 1,
            ADMIN = 2,
            CUSTOMER = 3,
            SUBUSER = 4,
            STOREHEAD = 5,
            STORE = 6,
            RESELLER = 7,
            SALES_PERSON = 8,
            ADMIN_SUPPORT = 9
        }
    }
}
