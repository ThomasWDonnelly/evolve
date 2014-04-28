using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolve.Domain.Auth.Model;
using Microsoft.Owin.Security.OAuth;

namespace Evolve.Infrastructure.Auth.Provider
{
    public class OAuthConfigurationProvider : IOAuthConfigurationProvider
    {
        public string ClientId { get; private set; }

        public OAuthConfigurationProvider(string clientId)
        {
            ClientId = clientId;
        }

    }
}
