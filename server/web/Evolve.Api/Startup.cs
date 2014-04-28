using System;
using System.Reflection;
using Evolve.Core.Auth.Middleware;
using Evolve.Domain.Auth.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using System.Web.Http;
using System.Diagnostics;

[assembly: OwinStartup(typeof(Evolve.Api.Startup))]

namespace Evolve.Api
{
    public class Startup
    {
        private static OAuthAuthorizationServerOptions oauthOptions;
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

            oauthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true,
                AuthenticationMode = AuthenticationMode.Passive
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseOAuthBearerTokens(oauthOptions);
            app.UseGoogleAuthentication();

            app
                .UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
        }

        private static IKernel CreateKernel()
        {
            Debugger.Launch();

            var kernel = new StandardKernel();

            kernel.Load("Evolve.Infrastructure.*.dll");
            kernel.Load("Evolve.Core.*.dll");

            oauthOptions.Provider = kernel.Get<ApplicationOAuthProvider>();

            return kernel;
        }
    }
}
