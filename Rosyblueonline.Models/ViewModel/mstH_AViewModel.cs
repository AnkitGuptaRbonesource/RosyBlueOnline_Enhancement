using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstH_AViewModel
    {
        [Key]
        public int HAid { get; set; }
        public string HA { get; set; }
        public string HaFullname { get; set; }
    }
}
