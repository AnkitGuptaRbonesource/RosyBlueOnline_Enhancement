using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UserDetailModel
    {
        [Key]
        public int userDetailId { get; set; }
        public int loginID { get; set; }
        public string phone01 { get; set; }
        public string phoneCode01 { get; set; }
        public string phone02 { get; set; }
        public string phoneCode02 { get; set; }
        public string mobile { get; set; }
        public string fax01 { get; set; }
        public string fax02 { get; set; }
        public string website { get; set; }
        public string emailId { get; set; }
        public DateTime? dateOfEstablishment { get; set; }
        public System.DateTime createdOn { get; set; }
        public System.DateTime? updatedOn { get; set; }
        public string createdIP { get; set; }
        public string createdDevice { get; set; }
        public bool isManufacturer { get; set; }
        public bool isWholeSaler { get; set; }
        public bool isRetailer { get; set; }
        public bool isOther { get; set; }
        public string otherType { get; set; }
        public string bankName { get; set; }
        public string branchName { get; set; }
        public string branchAddress { get; set; }
        public string accNumber { get; set; }
        public string swiftCode { get; set; }
        public string tinNo { get; set; }
        public string pan { get; set; }
        public string gstNo { get; set; }
        public string kycDocName { get; set; }
        public string kycDocNo { get; set; }
        public string kycDocFile { get; set; }

        public int? TypeId { get; set; }

        public DateTime? dateOfDocExpiry { get; set; }

    }
}
