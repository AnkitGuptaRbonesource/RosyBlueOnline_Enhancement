using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UpcomingShowModel
    {
        [Key]
        public int upcomingShowId { get; set; }
        public string upcomingShowName { get; set; }
        public string location { get; set; }
        public string showDetails { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public string updatedByIP { get; set; }
    }
}
