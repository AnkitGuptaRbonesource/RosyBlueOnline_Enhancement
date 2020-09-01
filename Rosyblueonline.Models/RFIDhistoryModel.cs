using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class RFIDhistoryModel
    {
        [Key]
        public int Rowno { get; set; }
        public string RFIDno { get; set; }
        public string CertificateNO { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Createdyby { get; set; }
        public int activityId { get; set; }
    }

    public class RFIDhistoryViewModel
    {
        public int Rowno { get; set; }
        public string RFIDno { get; set; }
        public string CertificateNO { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Createdyby { get; set; }
        public int activityId { get; set; }
        public string CreatedByName { get; set; }
     
    }
}
