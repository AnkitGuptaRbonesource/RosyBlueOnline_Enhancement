using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class mstRFIDModel
    {
        [Key]
        public int RowNo { get; set; }
        public string RfidNo { get; set; }
        public int CurrentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Createdby { get; set; }
        public bool Isactive { get; set; }
    }

    
}
