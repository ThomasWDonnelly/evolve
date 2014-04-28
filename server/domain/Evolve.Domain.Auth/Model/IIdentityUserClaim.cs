using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolve.Domain.Auth.Model
{
    public interface IIdentityUserClaim
    {
            string ClaimType { get; set; }
            string ClaimValue { get; set; }
            string Id { get; set; }
            string UserId { get; set;}
        }
    }
