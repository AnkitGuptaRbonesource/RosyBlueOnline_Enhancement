using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UserDetailView
    {
        [Key]
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public int roleID { get; set; }
        public string mobile { get; set; }
        public string username { get; set; }
        public string countryName { get; set; }
        public string emailId { get; set; }
        public double? startSizePermitted { get; set; }
        public int? rowDownloadPermitted { get; set; }

    }
}
