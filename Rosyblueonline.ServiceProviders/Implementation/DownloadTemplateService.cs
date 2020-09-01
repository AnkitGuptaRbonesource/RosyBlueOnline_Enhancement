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
    public class DownloadTemplateService : IDownloadTemplateService
    {
        private readonly UnitOfWork uow;
        public DownloadTemplateService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public List<CustomSelectViewModel> GetColumnForDisplay()
        {
            return uow.CustomSelects.GetAll().Select(x => new CustomSelectViewModel
            {
                customSelectID = x.customSelectID,
                displayName = x.displayName
            }).ToList();
        }
    }
}
