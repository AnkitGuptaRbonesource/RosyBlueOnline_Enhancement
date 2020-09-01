using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Rosyblueonline.Models
{
    public class LoginDetailModel
    {
        [Key]
        public int loginID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string verificationCode { get; set; }
        public int roleID { get; set; }
        public int salesLocationID { get; set; }
        public int parentLoginID { get; set; }
        public int userStatus { get; set; }
        public string locationIDs { get; set; }
        public bool? loggedInStatus { get; set; }
        public int loginAttempted { get; set; }
        public DateTime? lastLoginDate { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int? modifiedBy { get; set; }
        public int isApproved { get; set; }
        public int? approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }

    }
}
