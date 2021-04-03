using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IMarketingService
    {
        IQueryable<BlueNileDiscountModel> GetBlueNileDiscountQueryable();
        int AddBlueNileDiscount(BlueNileDiscountModel obj);
        IQueryable<JamesAllenDiscountModel> GetJamesAllenDiscountQueryable();
        int AddJamesAllenDiscount(JamesAllenDiscountModel obj);
        IQueryable<JamesAllenDiscountHKModel> GetJamesAllenDiscountHKQueryable();
        int AddJamesAllenDiscountHK(JamesAllenDiscountHKModel obj);

    }
}
