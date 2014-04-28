using System.Security.Claims;
using System.Threading.Tasks;
using Evolve.Domain.Auth.Model;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;

namespace Evolve.Infrastructure.Auth
{
    public class UserManager : IUserManager
    {
        private readonly UserStore<IdentityUser> _userStore;
        private readonly UserManager<IdentityUser> _userManager;
        public UserManager()
        {
            _userStore = new UserStore<IdentityUser>("Mongo");
            _userManager = new UserManager<IdentityUser>(_userStore);
        }

        public async Task<IUser> FindAsync(UserLoginInfo userLoginInfo)
        {
            var user = await _userManager.FindAsync(userLoginInfo);
            return user;
        }
        public async Task<IUser> FindAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(IUser identityUser, string authenticationType)
        {
            var user = await _userManager.FindByIdAsync(identityUser.Id);
            return await _userManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<IdentityResult> CreateAsync(IIdentityUser identityUser)
        {
            var user = new IdentityUser(identityUser.UserName);
            return await _userManager.CreateAsync(user);
        }



    }
}
