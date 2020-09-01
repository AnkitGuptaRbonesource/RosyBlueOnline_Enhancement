using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class RecentSearchModel
    {
        [Key]
        public int recentSearchID { get; set; }
        public int createdBy { get; set; }
        public string searchCriteria { get; set; }
        public DateTime Createdon { get; set; }
        public string searchCriteriaName { get; set; }
        public string searchType { get; set; }

        [NotMapped]
        public int totalFound { get; set; }
        public bool? isactive { get; set; }
        public string searchWhere { get; set; }
        public string displayCriteria { get; set; }
    }

    public class RecentSearchViewModel
    {
  
        public int recentSearchID { get; set; }
        public string searchCriteria { get; set; }
        public DateTime Createdon { get; set; }
        public string searchCriteriaName { get; set; }
        public string searchType { get; set; }
        public int TotalFound { get; set; }
        public string displayCriteria { get; set; }
    }
}
