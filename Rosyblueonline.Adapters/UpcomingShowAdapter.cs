using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Rosyblueonline.Entities;
using Rosyblueonline.Models.ViewModel;

namespace Rosyblueonline.Adapters
{
    public class UpcomingShowAdapter : IAdapter<_UpcomingShow, upcomingShow>
    {
        public _UpcomingShow ConvertEntityToModel(upcomingShow entity)
        {
            var model = Mapper.Map<upcomingShow, _UpcomingShow>(entity);
            return model;
        }

        public upcomingShow ConvertModelToEntity(_UpcomingShow model)
        {
            var entity = Mapper.Map<_UpcomingShow, upcomingShow>(model);
            return entity;
        }

        public IEnumerable<_UpcomingShow> ConvertEntitiesToModels(IEnumerable<upcomingShow> IEntity)
        {
            var models = Mapper.Map<IEnumerable<upcomingShow>, IEnumerable<_UpcomingShow>>(IEntity);
            return models;
        }

        public _UpcomingShow ConvertEntityToModel(_UpcomingShow entity)
        {
            var model = Mapper.Map<_UpcomingShow>(entity);
            //model.AnnualPortCharge = Math.Round(model.PortCharge * 12, 2);
            //model.ValidFrom = entity.ValidFrom;//String.Format("{0:dd/MM/yyyy}", entity.ValidFrom);
            //model.ValidTo = entity.ValidTo;//String.Format("{0:dd/MM/yyyy}", entity.ValidTo);
            //model.PortCharge = Math.Round(model.PortCharge, 2);
            return model;
        }


    }
}
