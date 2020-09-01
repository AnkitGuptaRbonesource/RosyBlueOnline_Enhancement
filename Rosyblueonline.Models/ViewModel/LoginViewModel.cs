using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string DeviceName { get; set; }
    }

    public class LoginData
    { 
        public string Username { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string DeviceName { get; set; } 
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LocationName { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public int LoginID { get; set; }
    }

}
