using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class TokenLogModel
    {
        [Key]
        public int tokenLogId { get; set; }
        public string tokenID { get; set; }
        public int loginID { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string EmailID { get; set; }

        public int loginDeviceId { get; set; }
        public DateTime timeStamp { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [NotMapped]
        public int RoleID { get; set; }
        [NotMapped]
        public bool IsSiteBlocked { get; set; }
    }
}
