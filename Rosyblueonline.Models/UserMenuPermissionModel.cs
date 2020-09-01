using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UserMenuPermissionModel
    {

        [Key]
        public int Id { get; set; }
        public int LoginId { get; set; }
        public int MenuId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; } 
    }
}
