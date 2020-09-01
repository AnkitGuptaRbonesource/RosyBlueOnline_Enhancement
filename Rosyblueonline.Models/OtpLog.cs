using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class OtpLog
    {
        [Key]
        public int otpLogId { get; set; }
        public string otpID { get; set; }
        public string emailId { get; set; }
        public bool isVerified { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
