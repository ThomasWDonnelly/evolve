using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Evolve.Core.Auth.Middleware;
using Evolve.Domain.Auth.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Modules;
using Owin;

namespace Evolve.Core.Auth
{
    public class AuthMiddleware : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationOAuthProvider>().ToConstructor(ctor => new ApplicationOAuthProvider(
                    Kernel.Get<IOAuthConfigurationProvider>(), Kernel.Get<IUserManager>()));
        }

    }
}
