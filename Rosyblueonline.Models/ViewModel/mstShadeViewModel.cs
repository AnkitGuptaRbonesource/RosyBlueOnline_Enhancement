using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstShadeViewModel
    {
        [Key]
        public int shadeId { get; set; }
        public string shadeName { get; set; }
    }
}
