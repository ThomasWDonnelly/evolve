using System;
using System.Reflection;
using Microsoft.Owin;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Evolve.Api.Startup))]

namespace Evolve.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            

            app
                .UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load("*.infrastructure.*.dll");
            return kernel;
        }
    }
}
