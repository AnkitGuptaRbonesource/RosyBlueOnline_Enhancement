using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class _SearchFilterViewModel
    {
        public List<mstColorViewModel> VMcolor { get; set; }
        public List<mstLabViewModel> VMlab { get; set; }
        public List<mstfancyColorViewModel> VMfancyColor { get; set; }
        public List<mstClarityViewModel> VMclarity { get; set; }
        public List<mstFluorescenceViewModel> VMfluorescence { get; set; }
        public List<mstH_AViewModel> VMha { get; set; }
        public List<mstKeyToSymbolViewModel> VMkeyToSymbol { get; set; }
        public List<mstGirdleNamesViewModel> VMgirdleNames { get; set; }
        public List<mstShadeViewModel> VMshade { get; set; }
        public List<mstTableBlackInclusionViewModel> VMtableBlackInclusion { get; set; }
        public List<mstSideBlackInclusionViewModel> VMsideBlackInclusion { get; set; }
        public List<mstMilkyInclusionViewModel> VMmilkyInclusion { get; set; }
        public List<mstEyeCleanViewModel> VMeyeClean { get; set; }
        public List<savedSearchViewModel> VMsavedSearch { get; set; }
        public List<customerDemandModel> VMcustomerDemand { get; set; }
        public List<SalesLocationViewModel> VMSalesLocation { get; set; }
        public string FromID { get; set; }

        public string Query { get; set; }

        public List<MstCaratsSizeViewModel> VMCaratsSize { get; set; }

        public List<MstStoneOriginViewModel> VMStoneOrigin { get; set; }

    }
}
