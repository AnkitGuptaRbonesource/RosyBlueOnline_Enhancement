using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
   public class customerDemandViewModel
    {
        public int demandID { get; set; }
        public int createdBy { get; set; }
        public string demandCriteria { get; set; }
        public DateTime createdon { get; set; }
        public string demandName { get; set; }
        public bool isactive { get; set; }
        public string searchWhere { get; set; }
    }
}
