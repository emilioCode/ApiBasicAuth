using ApiBasicAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiBasicAuth.Security
{
    public class BasicAuthHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public readonly IUserService _userService;
        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService
            ) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Access Denied");

            bool result = false;
            var email = string.Empty;
            var password = string.Empty;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                email = credentials[0];
                password = credentials[1];
                result = _userService.IsUser(email, password);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Invalid Credentials");
            }

            if (!result) return AuthenticateResult.Fail("User Not Found");

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.AuthorizationDecision, result.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
