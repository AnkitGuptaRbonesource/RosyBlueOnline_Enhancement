using AutoMapper;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UpcomingShowProfile());
                cfg.AddProfile(new UserRegistrationProfile());
            });
        }

        public class UpcomingShowProfile : Profile
        {
            public UpcomingShowProfile()
            {
                CreateMap<upcomingShow, _UpcomingShow>();
                CreateMap<_UpcomingShow, upcomingShow>();
            }
        }
        public class UserRegistrationProfile : Profile
        {
            public UserRegistrationProfile()
            {
                CreateMap<loginDetail, LoginDetailsModel>();
                CreateMap<LoginDetailsModel, loginDetail>();
                CreateMap<userDetail, UserDetailsModel>();
                CreateMap<UserDetailsModel, userDetail>();
                CreateMap<mstBillingAddress, MstBillingAddressModel>();
                CreateMap<MstBillingAddressModel, mstBillingAddress>();
                CreateMap<mstShippingAddress, MstShippingAddressModel>();
                CreateMap<MstShippingAddressModel, mstShippingAddress>();
            }
        }
    }
}
