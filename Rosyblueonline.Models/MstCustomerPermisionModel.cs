using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstCustomerPermisionModel
    {

        [Key]
        public int custPermissionID { get; set; }
        public int customerId { get; set; }
        public int roleID { get; set; }
        public double startSizePermitted { get; set; }
        public int rowDownloadPermitted { get; set; }
        public bool isActive { get; set; }
        public DateTime? createDate { get; set; }
        public int? createdBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? updatedBy { get; set; }
        public bool isOriginFilterPermitted { get; set; }
        
    }
}
