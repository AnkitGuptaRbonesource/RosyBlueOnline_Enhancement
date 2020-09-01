using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class Select2Response
    {
        public List<Select2Option> results { get; set; }
        public Select2Pagination pagination { get; set; }
    }

    public class Select2Option
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class Select2Pagination
    {
        public bool more { get; set; }
    }
}
