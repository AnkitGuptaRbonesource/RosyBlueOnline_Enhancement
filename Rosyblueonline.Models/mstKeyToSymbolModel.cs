﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
   public class mstKeyToSymbolModel
    {
        [Key]
        public int keyToSymbolID { get; set; }
        public string KeyToSymbol { get; set; }
    }
}
