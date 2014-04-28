using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Evolve.Domain.Auth.Model
{
    public interface IOAuthConfigurationProvider
    {
        string ClientId { get; }
    }
}
