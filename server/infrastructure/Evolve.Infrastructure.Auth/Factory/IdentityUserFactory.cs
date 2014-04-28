using Evolve.Domain.Auth.Model;

namespace Evolve.Infrastructure.Auth.Factory
{
    public class IdentityUserFactory : IIdentityUserFactory
    {
        public IIdentityUser CreateIIdentityUser(string userName)
        {
            return new MongoIdentityUser(userName);
        }
    }
}
