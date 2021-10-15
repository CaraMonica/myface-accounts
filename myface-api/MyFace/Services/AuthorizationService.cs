
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFace.Helpers;
using MyFace.Models.Database;
using MyFace.Models.Request;
using MyFace.Repositories;

namespace MyFace.Services
{
    public class BasicAuthorisationAttribute : AuthorizeAttribute
    {
        public BasicAuthorisationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }

    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        }

        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }
        public string Name { get; }

    }

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUsersRepo usersRepo
        ) : base(options, logger, encoder, clock)
        {
            _usersRepo = usersRepo;
        }

        private IUsersRepo _usersRepo;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            User user;
            try
            {
                user = AuthorizationHelper.GetUserFromHeader(authorizationHeader, _usersRepo);
            }
            catch (ArgumentException e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e.Message));

            }

            AuthenticatedUser authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, user.Username);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
