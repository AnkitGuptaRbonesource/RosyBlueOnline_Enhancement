using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstTableBlackInclusionModel
    {
        [Key]
        public int TableBlackInclusionId { get; set; }
        public string TableBlackInclusionName { get; set; }
    }
}
