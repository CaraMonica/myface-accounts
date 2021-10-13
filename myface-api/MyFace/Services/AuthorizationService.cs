
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
            var authHeaderRegex = new Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authorizationHeader))
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));

            var authBase64 =
            Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    authHeaderRegex.Replace(authorizationHeader, "$1")));

            var authSplit = authBase64.Split(Convert.ToChar(':'), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            var user = _usersRepo.GetByUsername(authUsername);

            if (user == null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid username."));

            if (!(HashedPasswordGenerator.GenerateHash(authPassword, user.Salt) == user.HashedPassword))
                return Task.FromResult(AuthenticateResult.Fail("Invalid password."));

            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "roundthecode");
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}