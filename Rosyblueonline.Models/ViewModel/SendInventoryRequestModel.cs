using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class SendInventoryRequestModel
    {
        public string filterText { get; set; }
        public string EmailName { get; set; }
        public string EMailTo { get; set; }
        public string EMailCC { get; set; }
        public string EMailBCC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        private bool _NewArrival = false;
        public bool NewArrival { get => _NewArrival; set => _NewArrival = value; }
        
    }
}
