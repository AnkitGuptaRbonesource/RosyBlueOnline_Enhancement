using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class AddressViewModel
    {
        public int billingId { get; set; }
        public int shippingId { get; set; }
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string address01 { get; set; }
        public string address02 { get; set; }
        public string cityName { get; set; }
        public int stateID { get; set; }
        public string stateName { get; set; }
        public int countryID { get; set; }
        public string countryName { get; set; }
        public string zipCode { get; set; }
        //public System.DateTime createdOn { get; set; }
        //public System.DateTime? updatedOn { get; set; }
        public bool isActive { get; set; }
        public string Type { get; set; }
        public bool? isDefault { get; set; }
    }
}
