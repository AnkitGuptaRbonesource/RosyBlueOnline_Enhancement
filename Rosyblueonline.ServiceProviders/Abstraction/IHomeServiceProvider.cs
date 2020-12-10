using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.Framework;
using System.Web.Mvc;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IHomeServiceProvider
    {
        IEnumerable<SelectOptionsViewModel> GetCountry();
        IEnumerable<SelectOptionsViewModel> GetState(int ID);
        List<UpcomingShowModel> GetUpcomingShow();
        UserCountViewModel GetUserCounts(int LoginID);

        IEnumerable<SelectOptionsViewModel> GetRTypes();

        IEnumerable<SelectOptionsViewModel> GetRIdentityType();

        MstCustomerPermisionModel AddToCartPermitted(int LoginID);

    }
}
