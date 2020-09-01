using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstLabViewModel
    {
        [Key]
        public int labID { get; set; }
        public string labName { get; set; }
    }
}
