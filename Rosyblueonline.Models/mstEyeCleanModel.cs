using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstEyeCleanModel
    {
        [Key]
        public int eyeCleanID { get; set; }
        public string eyeclean { get; set; }
    }
}
