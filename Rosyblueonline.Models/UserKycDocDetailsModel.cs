using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class UserKycDocDetailsModel
    {
        [Key]
        public int UserDocId { get; set; }
        public string LoginId { get; set; }
        public string KycDocId { get; set; } 
        public string KycDocName { get; set; }
        public string kycDocNo { get; set; }
        public string kycDocFile { get; set; }
        public DateTime? KycDocExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrgFileName { get; set; }
         
    }

    public class UserDocDetailsModel
    {  
        public string DocId { get; set; }
        public string DocNo { get; set; }
        public string DocExpiryDate { get; set; }
        public string DocRandomID { get; set; } 
        public string DocFile { get; set; }

        public string OrgFileName { get; set; }
    }
}
