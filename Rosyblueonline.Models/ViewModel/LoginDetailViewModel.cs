using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rosyblueonline.Models.ViewModel
{
    public class RegistrationViewModel
    {
        [Required]
        public string companyName { get; set; }
        [Required]
        public string address01 { get; set; }
        [Required]
        public int countryID { get; set; }

        
        public int TypeId { get; set; }


        [Required]
        public int stateID { get; set; }
        [Required]
        public string cityName { get; set; }
        [Required]
        public string ZIP { get; set; }

        public bool isManufacturer { get; set; }
        public bool isWholeSaler { get; set; }
        public bool isRetailer { get; set; }
        public bool isOther { get; set; }
        public string otherType { get; set; }


        public string phoneCode01 { get; set; }
        public string phone01 { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public DateTime? dateOfEstablishment { get; set; }
        [Required]
        public string emailId { get; set; }
        public string website { get; set; }

        //[Required]
        public string otp { get; set; }


        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string accNumber { get; set; }
        [Required]
        public string bankName { get; set; }
        [Required]
        public string branchName { get; set; }
        [Required]
        public string branchAddress { get; set; }


        public string tinNo { get; set; }
        public string pan { get; set; }
        public string gstNo { get; set; }

        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; } 
        public string kycDocName { get; set; }
         
        public string kycDocNo { get; set; }
        public string kycDocFile { get; set; }

        public DateTime? dateOfDocExpiry { get; set; }

        [Required]
        public string Captchacode { get; set; }
        public int createdBy { get; set; }
        public int LoginID { get; set; }
        public List<SelectOptionsViewModel> Country { get; set; }
        public List<SelectOptionsViewModel> State { get; set; }

        public List<SelectOptionsViewModel> RTypes { get; set; }

        public List<SelectOptionsViewModel> RIdentityType { get; set; }
        public int? userStatus { get; set; }

        public string  DocRandomID { get; set; }
    }

    public class UserRegistrationViewModel
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string companyName { get; set; }
        [Required]
        public string address01 { get; set; }
        [Required]
        public int countryID { get; set; }
         
        public int TypeId { get; set; }


        [Required]
        public int stateID { get; set; }
        [Required]
        public string cityName { get; set; }
        [Required]
        public string ZIP { get; set; }
        public string Mobile { get; set; }
        public string phoneCode01 { get; set; }
        public string phone01 { get; set; }
        [Required]
        public string emailId { get; set; }
        [Required]
        public string accNumber { get; set; }
        [Required]
        public string bankName { get; set; }
        [Required]
        public string branchName { get; set; }
        [Required]
        public string branchAddress { get; set; }
        public int LoginID { get; set; }
        public int? userStatus { get; set; }
        public List<SelectOptionsViewModel> Country { get; set; }
        public List<SelectOptionsViewModel> State { get; set; }

        public List<SelectOptionsViewModel> RTypes { get; set; }
        public bool OnlyAddCustomer { get; set; } 

    }

    public class UserRegistrationViewModelViaMemo
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string companyName { get; set; }
        [Required]
        public string address01 { get; set; }
        [Required]
        public int countryID { get; set; }
         
        public int TypeId { get; set; }

        [Required]
        public int stateID { get; set; }
        //[Required]
        public string cityName { get; set; }
        //[Required]
        public string ZIP { get; set; }
        public string Mobile { get; set; }
        public string phoneCode01 { get; set; }
        public string phone01 { get; set; }
        [Required]
        public string emailId { get; set; }
        public string accNumber { get; set; }
        public string bankName { get; set; }
        public string branchName { get; set; }
        public string branchAddress { get; set; }
        public int LoginID { get; set; }
        public List<SelectOptionsViewModel> Country { get; set; }
        public List<SelectOptionsViewModel> State { get; set; }

        public List<SelectOptionsViewModel> RTypes { get; set; }
        public bool OnlyAddCustomer { get; set; }

    }

}

