using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstMilkyInclusionModel
    {
        [Key]
        public int milkyId { get; set; }
        public string milkyName { get; set; }
    }
}
