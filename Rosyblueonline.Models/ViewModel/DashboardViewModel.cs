using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class DashboardViewModel
    {
        public List<RecentSearchViewModel> SpecificSearch { get; set; }
        public List<RecentSearchViewModel> SavedSearch { get; set; }
        public List<RecentSearchViewModel> DemandSearch { get; set; }
        public Counts counts { get; set; }
        public Criteria criteria { get; set; }
        public List<StockStatus> stocks { get; set; }
        public List<StockSummaryViewModel> StockSummary { get; set; }
        public List<CustomerRecentSearch> RecentSearch { get; set; }
        public List<DemandList> DemandList { get; set; }
    }

    public class Counts
    {
        public int RoundShape { get; set; }

        public int FancyShape { get; set; }

        public int FancyCount { get; set; }

        public int NewArrival { get; set; }

        public int OnMemo { get; set; }

        public int BestDeal { get; set; }
    }

    public class Criteria
    {
        public string RoundShapeCriteria { get; set; }
        public string FancyShapeCriteria { get; set; }
        public string FancyCountCriteria { get; set; }
    }

    public class StockStatus
    {
        public DateTime createdOn { get; set; }
        public string openingstock { get; set; }
        public string newStock { get; set; }
        public int memostock { get; set; }
        public string orderPending { get; set; }
        public string sold { get; set; }
        public string closingStock { get; set; }
        public int stockId { get; set; }
        public string inActiveStock { get; set; }
    }

    public class CustomerRecentSearch
    {
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime Date { get; set; }
    }

    public class DemandList
    {
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int demandID { get; set; }
        public string demandCriteria { get; set; }
        public string demandName { get; set; }
        public int TotalFound { get; set; }
        public DateTime Createdon { get; set; }
    }
}

