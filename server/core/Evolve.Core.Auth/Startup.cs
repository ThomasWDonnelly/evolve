using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Core.Auth.Middleware;
using Evolve.Domain.Auth.Model;
using Ninject;
using Ninject.Modules;

namespace Evolve.Core.Auth
{
    public class CoreAuthModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationOAuthProvider>().ToConstructor(ctor => new ApplicationOAuthProvider(
                    Kernel.Get<IOAuthConfigurationProvider>(), Kernel.Get<IUserManager>()));
        }

    }
}
