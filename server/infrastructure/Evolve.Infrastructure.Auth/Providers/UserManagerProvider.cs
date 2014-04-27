using Evolve.Domain.Auth;
using Evolve.Domain.Auth.Model;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using Ninject.Activation;

namespace Evolve.Infrastructure.Auth.Providers
{
    public class UserManagerProvider : Provider<MongoUserManager<MongoIdentityUser>>
    {
        protected override MongoUserManager<MongoIdentityUser> CreateInstance(IContext context)
        {
            var userStore = new UserStore<MongoIdentityUser>("Mongo");
            var userManager = new MongoUserManager<MongoIdentityUser>(userStore);
            return userManager;
        }
    }

    public class MongoUserManager<T> : UserManager<MongoIdentityUser>
    {
        public MongoUserManager(UserStore<MongoIdentityUser> userStore)
            : base(userStore)
        {
            
        }
    }
}
