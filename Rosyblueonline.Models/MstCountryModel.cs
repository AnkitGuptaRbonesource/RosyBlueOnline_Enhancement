using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstCountryModel
    {
        [Key]
        public int countryId { get; set; }
        public string countryName { get; set; }
        public int phonecode { get; set; }
        public string sortname { get; set; }
        public bool isActive { get; set; }
    }
}
