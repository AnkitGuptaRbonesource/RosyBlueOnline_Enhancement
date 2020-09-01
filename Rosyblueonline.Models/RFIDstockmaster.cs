using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class RFIDstockmaster
    {
        public string SrNo { get; set; }
        public string BoxName { get; set; }
        public string Totalstock { get; set; }
        public string Available { get; set; }
        public string OnMemo { get; set; }
        public string Sold { get; set; }
        public string OrderPending { get; set; }
        public string Cart { get; set; }
        public string Invalid { get; set; }
        public string stockId { get; set; }
        public string boxId { get; set; }
    }
}
