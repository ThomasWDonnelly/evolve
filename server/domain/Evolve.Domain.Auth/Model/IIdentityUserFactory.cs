using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.Domain.Auth.Model
{
    public interface IIdentityUserFactory
    {
        IIdentityUser CreateIIdentityUser(string userName);
    }
}
