using System;
using Evolve.Domain.Auth.Model;
using Evolve.Infrastructure.Auth.Factory;
using Evolve.Infrastructure.Auth.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Modules;

namespace Evolve.Infrastructure.Auth
{
    public class MongoAuthModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IOAuthConfigurationProvider>().ToConstructor<OAuthConfigurationProvider>(
                ctor => new OAuthConfigurationProvider("self"));
            Bind<IUserManager>().To<UserManager>();
            Bind<IIdentityUserFactory>().To<IdentityUserFactory>();
        }

    }
}
