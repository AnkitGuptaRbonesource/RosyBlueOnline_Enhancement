using AutoMapper;
using Rosyblueonline.Entities;
using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Adapters
{
   public class UserDetailsAdapter : IAdapter<UserDetailsModel, userDetail>
    {
        public UserDetailsModel ConvertEntityToModel(userDetail entity)
        {
            var model = Mapper.Map<userDetail, UserDetailsModel>(entity);
            return model;
        }

        public userDetail ConvertModelToEntity(UserDetailsModel model)
        {
            var entity = Mapper.Map<UserDetailsModel, userDetail>(model);
            return entity;
        }

        public IEnumerable<UserDetailsModel> ConvertEntitiesToModels(IEnumerable<userDetail> IEntity)
        {
            var models = Mapper.Map<IEnumerable<userDetail>, IEnumerable<UserDetailsModel>>(IEntity);
            return models;
        }
    }
}
