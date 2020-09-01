using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstFluorescenceViewModel
    {
        [Key]
        public int fluorescenceID { get; set; }
        public string fluorescenceName { get; set; }
        public string fullfluorescenceName { get; set; }
    }
}
