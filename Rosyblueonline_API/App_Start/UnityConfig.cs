 
using Rosyblueonline.Framework;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.ServiceProviders.Implementation;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;

namespace Rosyblueonline_API
{
    public static class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            // var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>(); 


            // container.RegisterType<IDataContext, DataContext>(); 
            container.RegisterType<IDataContext, DataContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IDBSQLServer, DBSQLServer>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IUserDetailService, UserDetailService>();
            container.RegisterType<IStockDetailsService, StockDetailsService>();
            container.RegisterType<IMemoService, MemoService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
        private static Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
    }
}