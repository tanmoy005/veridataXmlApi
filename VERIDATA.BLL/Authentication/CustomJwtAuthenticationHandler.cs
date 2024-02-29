//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using PfcAPI.Model.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net.Http.Headers;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using System.Web.Http.Filters;

//namespace PfcAPI.Infrastucture.Authentication
//{
//    //public class CustomJwtAuthenticationHandler : AuthenticationHandler<CustomJwtAuthenticationOptions>
//    //{
//    //    public readonly TokenConfiguration _tokenConfig;
//    //    public CustomJwtAuthenticationHandler(
//    //        IOptionsMonitor<CustomJwtAuthenticationOptions> options,
//    //        ILoggerFactory logger,
//    //        UrlEncoder encoder,
//    //        ISystemClock clock)
//    //        : base(options, logger, encoder, clock)
//    //    {
//    //    }

//    //    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//    //    {
//    //        if (!Request.Headers.ContainsKey("Authorization"))
//    //        {
//    //            return AuthenticateResult.Fail("Missing Authorization header");
//    //        }

//    //        string authorizationHeader = Request.Headers["Authorization"];
//    //        if (string.IsNullOrEmpty(authorizationHeader))
//    //        {
//    //            return AuthenticateResult.Fail("Invalid Authorization header");
//    //        }

//    //        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//    //        {
//    //            return AuthenticateResult.Fail("Invalid Authorization scheme");
//    //        }

//    //        string token = authorizationHeader.Substring("Bearer ".Length).Trim();

//    //        // You can now validate the token and its claims here.
//    //        // Implement your own custom logic to validate and decode the JWT token.

//    //        if (IsValidToken(token))
//    //        {
//    //            var claims = new List<Claim>
//    //        {
//    //            new Claim(ClaimTypes.Name, "username") // Add any claims you want to include
//    //        };

//    //            var identity = new ClaimsIdentity(claims, Scheme.Name);
//    //            var principal = new ClaimsPrincipal(identity);
//    //            var ticket = new AuthenticationTicket(principal, Scheme.Name);

//    //            return AuthenticateResult.Success(ticket);
//    //        }
//    //        else
//    //        {
//    //            return AuthenticateResult.Fail("Invalid token");
//    //        }
//    //    }

//    //    private bool IsValidToken(string token)
//    //    {
//    //        // Implement your custom logic to validate the JWT token
//    //        // You can use libraries like System.IdentityModel.Tokens.Jwt or IdentityServer4
//    //        // to validate the token and its claims against a specific signing key or certificate.

//    //        // Example using System.IdentityModel.Tokens.Jwt:
//    //        var tokenHandler = new JwtSecurityTokenHandler();

//    //        var validationParameters = new TokenValidationParameters
//    //        {
//    //            ValidateIssuer = true,
//    //            ValidIssuer = _tokenConfig.Issuer,
//    //            ValidateAudience = true,
//    //            ValidAudience = _tokenConfig.Audience,
//    //            ValidateIssuerSigningKey = true,
//    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Key))
//    //        };
//    //        try
//    //        {
//    //            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//    //            var jwtToken = (JwtSecurityToken)validatedToken;
//    //            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value);
//    //            return true;
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            return false;
//    //        }

//    //        return false; // Replace with your own validation logic
//    //    }
//    //}
//    public class CustomAuthenticationFilter : AuthorizeAttribute,   IAuthenticationFilter
//    {
//        public bool AllowMultiple
//        {
//            get { return false; }
//        }
//        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
//        {
//            string authParameter = string.Empty;
//            HttpRequestMessage request = context.Request;
//            AuthenticationHeaderValue authorization = request.Headers.Authorization;
//            string[] Token_User_Role = null;

//            if (authorization == null)
//            {
//                context.ErrorResult = new AuthenticationFailureResult(reasonPhase: "Missing Authorization Header", request: request);
//                return;
//            }
//            if (authorization.Scheme != "Bearer")
//            {
//                context.ErrorResult = new AuthenticationFailureResult(reasonPhase: "Missing Authorization Scheme", request: request);
//                return;
//            }

//            Token_User_Role = authorization.Parameter.Split(':');
//            string token = Token_User_Role[0];
//            //string username = Token_User_Role[1];
//            string userrole = Token_User_Role[2];

//            if (string.IsNullOrEmpty(token))
//            {
//                context.ErrorResult = new AuthenticationFailureResult(reasonPhase: "Missing Token", request: request);
//                return;
//            }

//            ValidTokenModel validtokenCheck = TokenManager.ValidateToken(token);
//            if (validtokenCheck.UserRole != userrole)
//            {
//                context.ErrorResult = new AuthenticationFailureResult(reasonPhase: "Invalid Token for UserName and UserRole", request: request);
//                return;
//            }
//            context.Principal = TokenManager.Getprincipal(token);
//        }

//        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
//        {
//            var result = await context.Result.ExecuteAsync(cancellationToken);
//            if (result.StatusCode == HttpStatusCode.Unauthorized)
//            {
//                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(scheme: "Basic", parameter: "realm=localhost"));
//            }
//            context.Result = new ResponseMessageResult(result);
//        }
//    }

//    public class AuthenticationFailureResult : IHttpActionResult
//    {
//        public string ReasonPhase;
//        public HttpRequestMessage Request { get; set; }

//        public AuthenticationFailureResult(string reasonPhase, HttpRequestMessage request)
//        {
//            ReasonPhase = reasonPhase;
//            Request = request;
//        }
//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            return Task.FromResult(Execute());
//        }

//        public HttpResponseMessage Execute()
//        {
//            HttpResponseMessage responceMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
//            responceMessage.RequestMessage = Request;
//            responceMessage.ReasonPhrase = ReasonPhase;
//            return responceMessage;
//        }
//    }
//}