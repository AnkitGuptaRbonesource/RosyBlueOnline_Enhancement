using Autofac;
using Autofac.Integration.Mvc;
using Rosyblueonline.Framework;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            //builder.RegisterModelBinderProvider();
            //builder.RegisterFilterProvider();
            //builder.RegisterModule<AutofacWebTypesModule>();

            // Register dependencies in filter attributes
            //builder.RegisterFilterProvider();

            //autofac Mapping
            builder.RegisterType(typeof(DataContext)).AsImplementedInterfaces();
            builder.RegisterType(typeof(DBSQLServer)).AsImplementedInterfaces();
            builder.RegisterType(typeof(UnitOfWork)).AsImplementedInterfaces();
            builder.RegisterType(typeof(HomeServiceProvider)).AsImplementedInterfaces();
            builder.RegisterType(typeof(UserDetailService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(StockDetailsService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(DownloadTemplateService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(OrderService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(MemoService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(RecentSearchService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(MarketingService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(ChargeService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(MailUtility)).AsImplementedInterfaces();
            builder.RegisterType(typeof(RFIDService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(DownloadScriptService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(WS_SchedulerService)).AsImplementedInterfaces();
            //builder.RegisterType<DataContext>().As<IDataContext>();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //builder.RegisterType<HomeServiceProvider>().As<IHomeServiceProvider>();

            var container = builder.Build();
            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}