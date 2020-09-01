using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class RecentSearchService : IRecentSearchService
    {
        readonly UnitOfWork uow = null;
        public RecentSearchService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public int Add(RecentSearchModel obj)
        {
            this.uow.RecentSearches.Add(obj);
            return this.uow.Save();
        }

        public int Edit(RecentSearchModel obj)
        {
            RecentSearchModel objOld = this.uow.RecentSearches.Queryable().Where(x => x.recentSearchID == obj.recentSearchID).FirstOrDefault();
            if (objOld != null)
            {
                objOld.searchCriteria = obj.searchCriteria;
                objOld.searchCriteriaName = obj.searchCriteriaName;
                objOld.searchType = obj.searchType;
                objOld.displayCriteria = obj.displayCriteria;
                this.uow.RecentSearches.Edit(objOld);
                return this.uow.Save();
            }
            return 0;
        }

        public RecentSearchModel Get(int recentSearchID,int CreatedBy)
        {
            return this.uow.RecentSearches.Queryable().Where(x => x.recentSearchID == recentSearchID && x.createdBy == CreatedBy).FirstOrDefault();
        }

        public List<SelectOptionsViewModel> GetForOptions(string SearchType)
        {
            return this.uow.RecentSearches.Queryable().Where(x => x.searchType == SearchType).AsEnumerable().Select(x => new SelectOptionsViewModel { Value = x.recentSearchID, Text = x.searchCriteriaName, Text2 = x.searchCriteria }).ToList();
        }

        public int Delete(int recentSearchID)
        {
            var objRec = this.uow.RecentSearches.Queryable().Where(x => x.recentSearchID == recentSearchID).FirstOrDefault();
            if (objRec != null)
            {
                this.uow.RecentSearches.Delete(objRec);
                return this.uow.Save();
            }
            return 0;
        }

    }
}
