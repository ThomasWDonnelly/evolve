using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Evolve.Domain.Auth.Model
{
    public interface IIdentityUser : IUser<string>
    {
        List<UserLoginInfo> Logins { get; }

        List<string> Roles { get; }
    }
}
