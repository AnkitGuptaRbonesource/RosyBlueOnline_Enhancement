using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class CompareInventoryViewModel
    {
        public int Count { get; set; }
        public List<CompareViewModel> Items { get; set; }
    }

    public class CompareViewModel
    {
        public string FHeader { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
    }
}
