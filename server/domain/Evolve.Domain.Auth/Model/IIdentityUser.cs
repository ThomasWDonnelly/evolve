using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Evolve.Domain.Auth.Model
{
    public interface IIdentityUser : IUser
    {
        List<string> Roles { get; }
        /// <summary>
        /// Gets the logins.
        /// </summary>
        /// <value>The logins.</value>
        List<UserLoginInfo> Logins { get; }
    }


    
}