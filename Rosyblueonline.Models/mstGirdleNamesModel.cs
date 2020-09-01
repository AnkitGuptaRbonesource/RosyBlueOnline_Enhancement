using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
  public  class mstGirdleNamesModel
    {
        [Key]
        public int girdleNameID { get; set; }
        public string girdleName { get; set; }
        public string girdleFullName { get; set; }
    }
}
