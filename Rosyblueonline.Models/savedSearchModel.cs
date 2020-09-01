using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class savedSearchModel
    {
        [Key]
        public int savedSearchID { get; set; }
        public int createdBy { get; set; }
        public string searchCriteria { get; set; }
        public DateTime Createdon { get; set; }
        public string searchCriteriaName { get; set; }
        public bool isactive { get; set; }
        public string searchWhere { get; set; }

    }
}
