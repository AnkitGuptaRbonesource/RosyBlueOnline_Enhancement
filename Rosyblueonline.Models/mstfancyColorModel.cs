using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstfancyColorModel
    {
        [Key]
        public int fancyColorID { get; set; }
        public string fancyColor { get; set; }
        public string fancyColorFullname { get; set; }
        public string fancyColor_ID { get; set; }
    }
}
