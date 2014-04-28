using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Evolve.Core.Auth.Middleware;
using Evolve.Core.Auth.Providers;
using Evolve.Domain.Auth;
using Evolve.Domain.Auth.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Security.Cryptography;

namespace Evolve.Core.Auth.Controllers
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private readonly IUserManager _userManager;
        private readonly IIdentityUserFactory _userFactory;
        private readonly IOAuthConfigurationProvider _oauthConfigurationProvider;

        public AccountController(IUserManager userManager, IIdentityUserFactory userFactory, 
            IOAuthConfigurationProvider oauthConfigurationProvider)
        {
            _userManager = userManager;
            _userFactory = userFactory;
            _oauthConfigurationProvider = oauthConfigurationProvider;
        }

        [AllowAnonymous]
        [Route("ExternalLogins")]
        public dynamic GetExternalLogins(string returnUrl = "/", bool generateState = false)
        {
            var descriptions = Authentication.GetExternalAuthenticationTypes().ToArray();

            var state = string.Empty;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateProvider.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            var logins = descriptions.Select(description => new
            {
                Name = description.Caption,
                Url = Url.Route("ExternalLogin", new
                {
                    provider = description.AuthenticationType,
                    response_type = "token",
                    client_id = _oauthConfigurationProvider.ClientId,
                    redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                    state = state
                })
            });

            return new { descriptions = descriptions, logins = logins };
        }

        [Route("UserInfo")]
        public dynamic GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            return new
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<dynamic> GetExternalLogin(string provider = null, string error = null)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var userLoginInfo = new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey);
            var user = await _userManager.FindAsync(userLoginInfo);

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var oAuthIdentity = await _userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
                var cookieIdentity = await _userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

                var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                var claims = externalLogin.GetClaims();
                var identityUser = _userFactory.CreateIIdentityUser(externalLogin.UserName);
                {
                    // Id = ObjectId.GenerateNewId().ToString(),  
                };
                identityUser.Logins.Add(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));
                identityUser.Roles.Add("admin");
                
                var result = await _userManager.CreateAsync(identityUser);

                if (!result.Succeeded)
                {
                    return result;
                }

                var oAuthIdentity = await _userManager.CreateIdentityAsync(identityUser, OAuthDefaults.AuthenticationType);
                var cookieIdentity = await _userManager.CreateIdentityAsync(identityUser, CookieAuthenticationDefaults.AuthenticationType);

                var properties = ApplicationOAuthProvider.CreateProperties(externalLogin.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }

            return Ok();
        }



        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IEnumerable<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }



                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }


        private static class RandomOAuthStateGenerator
        {
        }


    }
}
