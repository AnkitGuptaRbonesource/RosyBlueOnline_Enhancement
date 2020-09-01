using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IChargeService
    {
        List<mstChargesModel> GetAllCharges();
    }
}
