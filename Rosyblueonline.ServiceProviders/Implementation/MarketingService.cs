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

        public BlueNileDiscountModel EditBlueNile(int id)
        {
            return uow.BlueNileDiscount.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

        }
        public int UpdateBlueNileDiscount(BlueNileDiscountModel obj)
        {
            BlueNileDiscountModel obju = new BlueNileDiscountModel();
            obju = this.uow.BlueNileDiscount.Queryable().Where(x => x.SrNo == obj.SrNo).FirstOrDefault();

            obju.caratRange1Disc = obj.caratRange1Disc;
            obju.caratRange2Disc = obj.caratRange2Disc;
            obju.caratRange3Disc = obj.caratRange3Disc;
            obju.caratRange4Disc = obj.caratRange4Disc;
            obju.caratRange5Disc = obj.caratRange5Disc;
            obju.caratRange6Disc = obj.caratRange6Disc;
            obju.caratRange7Disc = obj.caratRange7Disc;
            obju.caratRange8Disc = obj.caratRange8Disc;
            obju.caratRange9Disc = obj.caratRange9Disc;
            obju.caratRange10Disc = obj.caratRange10Disc;
            obju.caratRange11Disc = obj.caratRange11Disc;
            obju.caratRange12Disc = obj.caratRange12Disc;
            obju.caratRange13Disc = obj.caratRange13Disc;
            obju.caratRange14Disc = obj.caratRange14Disc;
            obju.caratRange15Disc = obj.caratRange15Disc;
            obju.caratRange16Disc = obj.caratRange16Disc;
            obju.caratRange17Disc = obj.caratRange17Disc;
            obju.caratRange18Disc = obj.caratRange18Disc;
            obju.caratRange19Disc = obj.caratRange19Disc;
            obju.caratRange20Disc = obj.caratRange20Disc;
            obju.caratRange21Disc = obj.caratRange21Disc;
            obju.caratRange22Disc = obj.caratRange22Disc;
            obju.caratRange23Disc = obj.caratRange23Disc;
            obju.caratRange24Disc = obj.caratRange24Disc;
            obju.caratRange25Disc = obj.caratRange25Disc;
            obju.haExDisc = obj.haExDisc;
            obju.haVgDisc = obj.haVgDisc;
            obju.UpdatedOn = DateTime.Now;
            obju.UpdatedBy = obj.UpdatedBy;
            obju.Isactive = obj.Isactive;

            this.uow.BlueNileDiscount.Edit(obju);
            return this.uow.Save();
        }
        public int DeleteBlueNileDiscount(int id)
        {
            BlueNileDiscountModel obju = new BlueNileDiscountModel();
            obju = this.uow.BlueNileDiscount.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

            if (obju != null)
            {
                this.uow.BlueNileDiscount.Delete(obju);
                return this.uow.Save();
            }
            return 0;
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

        public JamesAllenDiscountModel EditJamesAllen(int id)
        {
            return uow.JamesAllenDiscount.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

        }
        public int UpdateJamesAllenDiscount(JamesAllenDiscountModel obj)
        {
            JamesAllenDiscountModel obju = new JamesAllenDiscountModel();
            obju = this.uow.JamesAllenDiscount.Queryable().Where(x => x.SrNo == obj.SrNo).FirstOrDefault();

            obju.caratRange1Disc = obj.caratRange1Disc;
            obju.caratRange2Disc = obj.caratRange2Disc;
            obju.caratRange3Disc = obj.caratRange3Disc;
            obju.caratRange4Disc = obj.caratRange4Disc;
            obju.caratRange5Disc = obj.caratRange5Disc;
            obju.caratRange6Disc = obj.caratRange6Disc;
            obju.caratRange7Disc = obj.caratRange7Disc;
            obju.caratRange8Disc = obj.caratRange8Disc;
            obju.caratRange9Disc = obj.caratRange9Disc;
            obju.caratRange10Disc = obj.caratRange10Disc;
            obju.caratRange11Disc = obj.caratRange11Disc;
            obju.caratRange12Disc = obj.caratRange12Disc;
            obju.caratRange13Disc = obj.caratRange13Disc;
            obju.caratRange14Disc = obj.caratRange14Disc;
            obju.caratRange15Disc = obj.caratRange15Disc;
            obju.caratRange16Disc = obj.caratRange16Disc;
            obju.caratRange17Disc = obj.caratRange17Disc;
            obju.caratRange18Disc = obj.caratRange18Disc;
            obju.caratRange19Disc = obj.caratRange19Disc;
            obju.caratRange20Disc = obj.caratRange20Disc;
            obju.caratRange21Disc = obj.caratRange21Disc;
            obju.caratRange22Disc = obj.caratRange22Disc;
            obju.caratRange23Disc = obj.caratRange23Disc;
            obju.caratRange24Disc = obj.caratRange24Disc;
            obju.caratRange25Disc = obj.caratRange25Disc;
            obju.CNRDisc = obj.CNRDisc;
            obju.haExDisc = obj.haExDisc;
            obju.haVgDisc = obj.haVgDisc;
            obju.cnrDiscHA = obj.cnrDiscHA;
            obju.UpdatedOn = DateTime.Now;
            obju.UpdatedBy = obj.UpdatedBy;
            obju.Isactive = obj.Isactive;

            this.uow.JamesAllenDiscount.Edit(obju);
            return this.uow.Save();
        }
        public int DeleteJamesAllenDiscount(int id)
        {
            JamesAllenDiscountModel obju = new JamesAllenDiscountModel();
            obju = this.uow.JamesAllenDiscount.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

            if (obju != null)
            {
                this.uow.JamesAllenDiscount.Delete(obju);
                return this.uow.Save();
            }
            return 0;
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

        public JamesAllenDiscountHKModel EditJamesAllenHK(int id)
        {
            return uow.JamesAllenDiscountHK.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

        }
        public int UpdateJamesAllenHKDiscount(JamesAllenDiscountHKModel obj)
        {
            JamesAllenDiscountHKModel obju = new JamesAllenDiscountHKModel();
            obju = this.uow.JamesAllenDiscountHK.Queryable().Where(x => x.SrNo == obj.SrNo).FirstOrDefault();

            obju.caratRange1Disc = obj.caratRange1Disc;
            obju.caratRange2Disc = obj.caratRange2Disc;
            obju.caratRange3Disc = obj.caratRange3Disc;
            obju.caratRange4Disc = obj.caratRange4Disc;
            obju.caratRange5Disc = obj.caratRange5Disc;
            obju.caratRange6Disc = obj.caratRange6Disc;
            obju.caratRange7Disc = obj.caratRange7Disc;
            obju.caratRange8Disc = obj.caratRange8Disc;
            obju.caratRange9Disc = obj.caratRange9Disc;
            obju.caratRange10Disc = obj.caratRange10Disc;
            obju.caratRange11Disc = obj.caratRange11Disc;
            obju.caratRange12Disc = obj.caratRange12Disc;
            obju.caratRange13Disc = obj.caratRange13Disc;
            obju.caratRange14Disc = obj.caratRange14Disc;
            obju.caratRange15Disc = obj.caratRange15Disc;
            obju.caratRange16Disc = obj.caratRange16Disc;
            obju.caratRange17Disc = obj.caratRange17Disc;
            obju.caratRange18Disc = obj.caratRange18Disc;
            obju.caratRange19Disc = obj.caratRange19Disc;
            obju.caratRange20Disc = obj.caratRange20Disc;
            obju.caratRange21Disc = obj.caratRange21Disc;
            obju.caratRange22Disc = obj.caratRange22Disc;
            obju.caratRange23Disc = obj.caratRange23Disc;
            obju.caratRange24Disc = obj.caratRange24Disc;
            obju.caratRange25Disc = obj.caratRange25Disc;
            obju.CNRDisc = obj.CNRDisc;
            obju.haExDisc = obj.haExDisc;
            obju.haVgDisc = obj.haVgDisc;
            obju.cnrDiscHA = obj.cnrDiscHA;
            obju.UpdatedOn = DateTime.Now;
            obju.UpdatedBy = obj.UpdatedBy;
            obju.Isactive = obj.Isactive;

            this.uow.JamesAllenDiscountHK.Edit(obju);
            return this.uow.Save();
        }
        public int DeleteJamesAllenHKDiscount(int id)
        {
            JamesAllenDiscountHKModel obju = new JamesAllenDiscountHKModel();
            obju = this.uow.JamesAllenDiscountHK.Queryable().Where(x => x.SrNo == id).FirstOrDefault();

            if (obju != null)
            {
                this.uow.JamesAllenDiscountHK.Delete(obju);
                return this.uow.Save();
            }
            return 0;
        }
    }
}
