using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  
using System.ComponentModel.DataAnnotations;


namespace Rosyblueonline.Models.ViewModel
{
  public  class MstStoneOriginViewModel
    { 
        [Key]
        public int originID { get; set; }
        public string originName { get; set; }
       
    }
}
