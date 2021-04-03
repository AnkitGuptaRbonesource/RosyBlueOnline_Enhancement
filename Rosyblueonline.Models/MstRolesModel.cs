using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstRolesModel
    {
        [Key]
        public int roleID { get; set; }
        public string RoleName { get; set; }
        public bool Isactive { get; set; }

    }
}
