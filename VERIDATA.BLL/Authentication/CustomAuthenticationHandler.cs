using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VERIDATA.BLL.Extensions;
using VERIDATA.Model.Configuration;

namespace VERIDATA.BLL.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        public static TokenConfiguration config;

        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationOptions> options, TokenConfiguration Configuration, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            config = Configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Extract the token from the request
            var _token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokendata = _token?.Split("|~|");
            var token = tokendata?.LastOrDefault()?.Trim();
            var customtokenInfo = tokendata?.FirstOrDefault()?.Trim();
            if (string.IsNullOrEmpty(customtokenInfo))
            {
                return AuthenticateResult.Fail("Token validation failed.");
            }
            // Validate the token
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var user = handler.ValidateToken(token, validationParameters, out var validatedToken);

                var claims = user?.Claims?.ToList();

                if (claims.Any())
                {
                    var userIdClaim = claims?.Where(x => x.Type == "userId")?.FirstOrDefault()?.Value;
                    var userNameClaim = user.Identity.Name;
                    if (userNameClaim != customtokenInfo)
                    {
                        return AuthenticateResult.Fail("Token validation failed.");
                    }
                }

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                // Log any validation errors
                Logger.LogError(ex, "Token validation failed.");
                return AuthenticateResult.Fail("Token validation failed.");
            }
        }
    }
}