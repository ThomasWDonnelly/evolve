using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.Domain.Auth.Model
{
    public interface IUserManager
    {

        Task<IUser> FindAsync(UserLoginInfo userLoginInfo);

        Task<ClaimsIdentity> CreateIdentityAsync(IUser identityUser, string authenticationType);

        Task<IdentityResult> CreateAsync(IIdentityUser identityUser);

        Task<IUser> FindAsync(string userName);
    }
}
