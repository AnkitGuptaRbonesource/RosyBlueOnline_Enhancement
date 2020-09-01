using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;

namespace Rosyblueonline.Adapters
{
    //public class GenericAdapter
    //{
    //    public virtual M ConvertEntityToModel<E, M>(E entity) where E : class where M : class
    //    {
    //        var model = Mapper.Map<E, M>(entity);
    //        return model;
    //    }
    //}

    public class LoginDetailsAdapter : GenericAdapter
    {
        public LoginDetailsModel ConvertEntityToModel(loginDetail entity)
        {
            var model = Mapper.Map<loginDetail, LoginDetailsModel>(entity);
            return model;
        }

        //public override M ConvertEntityToModel(loginDetail model)
        //{
        //    var model = Mapper.Map<loginDetail, LoginDetailsModel> (model).ForSourceMember(src => src.Id, opts => opts.Ignore()); ;
        //    ///return  base.ConvertEntityToModel<E, M>(entity);
        //}

        //public loginDetail ConvertModelToEntity(LoginDetailsModel model)
        //{
        //    var entity = Mapper.Map<LoginDetailsModel, loginDetail>(model);
        //    return entity;
        //}

        public IEnumerable<LoginDetailsModel> ConvertEntitiesToModels(IEnumerable<loginDetail> IEntity)
        {
            var models = Mapper.Map<IEnumerable<loginDetail>, IEnumerable<LoginDetailsModel>>(IEntity);
            return models;
        }
    }
}
