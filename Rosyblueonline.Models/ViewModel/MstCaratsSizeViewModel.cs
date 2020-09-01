using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models.ViewModel
{
  public   class MstCaratsSizeViewModel
    {
        [Key]
        public int CaratId { get; set; }
        public string CaratFrm { get; set; }
        public string CaratTo { get; set; }
     
    }
}
