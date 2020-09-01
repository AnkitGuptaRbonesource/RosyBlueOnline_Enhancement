using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class UserCountViewModel
    {
        public int Cart { get; set; }
        public int WatchList { get; set; }
        public int Orders { get; set; }
        public int OrdersPending { get; set; }
        public int OrdersCompleted { get; set; }
        
    }
}
