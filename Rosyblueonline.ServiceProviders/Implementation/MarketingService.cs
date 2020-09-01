using Rosyblueonline.Models;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class MarketingService: IMarketingService
    {
        readonly UnitOfWork uow = null;
        public MarketingService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public IQueryable<BlueNileDiscountModel> GetBlueNileDiscountQueryable()
        {
            return this.uow.BlueNileDiscount.Queryable();
        }

        public int AddBlueNileDiscount(BlueNileDiscountModel obj)
        {
            obj.createdOn = DateTime.Now;
            this.uow.BlueNileDiscount.Add(obj);
            return this.uow.Save();
        }

        public IQueryable<JamesAllenDiscountModel> GetJamesAllenDiscountQueryable()
        {
            return this.uow.JamesAllenDiscount.Queryable();
        }

        public int AddJamesAllenDiscount(JamesAllenDiscountModel obj)
        {
            obj.createdOn = DateTime.Now;
            this.uow.JamesAllenDiscount.Add(obj);
            return this.uow.Save();
        }

        public IQueryable<JamesAllenDiscountHKModel> GetJamesAllenDiscountHKQueryable()
        {
            return this.uow.JamesAllenDiscountHK.Queryable();
        }

        public int AddJamesAllenDiscountHK(JamesAllenDiscountHKModel obj)
        {
            obj.createdOn = DateTime.Now;
            this.uow.JamesAllenDiscountHK.Add(obj);
            return this.uow.Save();
        }

    }
}
