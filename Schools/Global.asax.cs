using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Schools.Services;

namespace Schools
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var webhostAssembly = Assembly.GetExecutingAssembly();

            var builder = new ContainerBuilder();
            builder.RegisterControllers(webhostAssembly);

            builder.RegisterAssemblyTypes(webhostAssembly)
                .Where(t => t.IsAssignableTo<ISingletonService>())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(webhostAssembly)
                .Where(t => t.IsAssignableTo<IService>())
                .AsImplementedInterfaces();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //RegisterAutofacIoc();
        }

        private void RegisterAutofacIoc()
        {
            var webhostAssembly = Assembly.GetExecutingAssembly();

            var builder = new ContainerBuilder();
            builder.RegisterControllers(webhostAssembly);

            builder.RegisterAssemblyTypes(webhostAssembly)
                .Where(t => t.IsAssignableTo<ISingletonService>())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(webhostAssembly)
                .Where(t => t.IsAssignableTo<IService>())
                .AsImplementedInterfaces();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}