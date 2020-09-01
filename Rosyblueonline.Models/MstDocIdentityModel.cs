using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class MstDocIdentityModel
    {
        [Key]
        public int DocId { get; set; }
        public string DocIdentityName { get; set; }
        public bool IsActive { get; set; }
    }
}
