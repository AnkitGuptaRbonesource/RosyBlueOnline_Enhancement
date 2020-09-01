using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstColorModel
    {
        [Key]
        public int colorID { get; set; }
        public string colorName { get; set; }
        public int colorOrderID { get; set; }
        public string color_ID { get; set; }
    }
}
