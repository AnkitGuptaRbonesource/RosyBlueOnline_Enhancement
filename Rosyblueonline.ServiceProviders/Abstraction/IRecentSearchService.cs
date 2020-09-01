using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IRecentSearchService
    {
        int Add(RecentSearchModel obj);
        int Edit(RecentSearchModel obj);
        RecentSearchModel Get(int recentSearchID, int CreatedBy);
        List<SelectOptionsViewModel> GetForOptions(string SearchType);
        int Delete(int recentSearchID);
    }
}
