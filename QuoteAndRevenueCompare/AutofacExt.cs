using Autofac;
using Autofac.Integration.Mvc;
using RApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace QuoteAndRevenueCompare
{
    public static class AutofacExt
    {
        private static IContainer _container;

        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            //注册app层
            var appAssembly = typeof(ImportApp).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(appAssembly).InstancePerHttpRequest();

            //注册领域服务
       
            //注册所有Repository
            var repository = System.Reflection.Assembly.Load("RRepository");
            builder.RegisterAssemblyTypes(repository)
  .Where(t => t.Name.EndsWith("Repository"))
  .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(repository)
      .Where(t => t.Name.EndsWith("DatabaseFactory"))
      .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(repository)
    .Where(t => t.Name.EndsWith("UnitWork"))
    .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterControllers(assembly).PropertiesAutowired();

            //容器
            _container = builder.Build();
            //注入改为Autofac注入 for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
            //注入for web api
            var configuration = GlobalConfiguration.Configuration;
        }

        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }
}