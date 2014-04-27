using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Domain.Auth;
using Evolve.Domain.Auth.Model;
using Evolve.Infrastructure.Auth.Providers;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Evolve.Infrastructure.Auth
{
    public class MongoAuthModule : NinjectModule 
    {
        public override void Load()
        {
            Bind<IIdentityUserFactory>().ToFactory();
            Bind(typeof(UserManager<>)).ToProvider<UserManagerProvider>();
        }
    }
}
