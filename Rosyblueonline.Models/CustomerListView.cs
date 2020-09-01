using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class CustomerListView
    {
        [Key]
        public int loginID { get; set; }
        public DateTime createdOn { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string address01 { get; set; }
        public string address02 { get; set; }
        public string cityName { get; set; }
        public string zipCode { get; set; }
        public string phone01 { get; set; }
        public string phoneCode01 { get; set; }
        public string phone02 { get; set; }
        public string phoneCode02 { get; set; }
        public string bankName { get; set; }
        public string branchName { get; set; }
        public string branchAddress { get; set; }
        public string accNumber { get; set; }
        public string mobile { get; set; }
        public string fax01 { get; set; }
        public string fax02 { get; set; }
        public string website { get; set; }
        public string emailId { get; set; }
        public DateTime? dateOfEstablishment { get; set; }
        public bool? isManufacturer { get; set; }
        public bool? isWholeSaler { get; set; }
        public bool? isRetailer { get; set; }
        public bool? isOther { get; set; }
        public string otherType { get; set; }
        public int countryId { get; set; }
        public string countryName { get; set; }
        public int stateId { get; set; }
        public string stateName { get; set; }
        public int isApproved { get; set; }
        public int? approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }
        public string ApprovedByName { get; set; }
    }
}
