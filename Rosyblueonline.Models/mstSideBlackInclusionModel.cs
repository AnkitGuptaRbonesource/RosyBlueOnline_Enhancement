using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstSideBlackInclusionModel
    {
        [Key]
        public int sideBlackInclusionsId { get; set; }
        public string sideBlackInclusionsName { get; set; }
    }
}
