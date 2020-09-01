using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public sealed class _UpcomingShow
    {
        public int upcomingShowId { get; set; }
        public string upcomingShowName { get; set; }
        public string location { get; set; }
        public string showDetails { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }

    }
}
