using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstTypesModel
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; } 
        public bool IsActive { get; set; }

    }
}
