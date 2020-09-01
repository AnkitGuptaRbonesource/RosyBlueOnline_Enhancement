using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class customerDemandModel
    {   
        [Key]
        public int demandID { get; set; }
        public int createdBy { get; set; }
        public string demandCriteria { get; set; }
        public DateTime createdon { get; set; }
        public string demandName { get; set; }
        public bool isactive { get; set; }
        public string searchWhere { get; set; }

    }
}
