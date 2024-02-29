using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VERIDATA.Model.Configuration;

namespace VERIDATA.BLL.Authentication
{
    public class TokenAuth : ITokenAuth
    {
        private readonly TokenConfiguration _tokenConfig;

        public TokenAuth(TokenConfiguration tokenConfig)
        {
            _tokenConfig = tokenConfig;
        }

        public string createToken(string? UserName, string? Role, int? userId)
        {
            //SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenConfig?.Key));
            //SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            //var Subject = new Claim(new Claim[]
            //  {
            //        new Claim(ClaimTypes.Name, "UserName"),
            //        new Claim(ClaimTypes.Role, "Role")
            //  });
            //var token = new JwtSecurityToken("your-issuer", "your-audience", null, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            //var claims = new ClaimsIdentity(new Claim[] {
            //             new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
            //             new Claim(ClaimTypes.Name, UserName),
            //             new Claim(ClaimTypes.Role, Role),
            //             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //             new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //             new Claim("UserId", userId.ToString()),
            //        }
            //);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("userId", userId.ToString()),
            };
            string Token = GenerateAccessToken(claims);
            //JwtSecurityTokenHandler tokenHandler = new();
            //SecurityTokenDescriptor tokenDescriptor =
            //    new()
            //    {
            //        Issuer = _tokenConfig?.Issuer,
            //        Audience = _tokenConfig?.Audience,
            //        Subject = claims,
            //        IssuedAt = DateTime.UtcNow,
            //        Expires = DateTime.UtcNow.AddMinutes(_tokenConfig?.Timeout ?? 0),
            //        // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //        SigningCredentials = credentials
            //    };

            //SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            //string Token = tokenHandler.WriteToken(token);
            return Token;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenConfig?.Key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityTokenHandler tokenHandler = new();

            var tokeOptions = new JwtSecurityToken(
                issuer: _tokenConfig?.Issuer,
                audience: _tokenConfig?.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenConfig?.Timeout ?? 0),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            //SecurityTokenDescriptor tokenDescriptor =
            //    new()
            //    {
            //        Issuer = _tokenConfig?.Issuer,
            //        Audience = _tokenConfig?.Audience,
            //        Subject = claims,
            //        //Subject = new ClaimsIdentity(new Claim[]
            //        //{
            //        //    new Claim(ClaimTypes.Name, UserName),
            //        //    new Claim(ClaimTypes.Role, Role),
            //        //    new Claim(ClaimTypes.Role, Role)
            //        //}),
            //        IssuedAt = DateTime.UtcNow,
            //        Expires = DateTime.UtcNow.AddMinutes(_tokenConfig?.Timeout ?? 0),
            //        // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //        SigningCredentials = credentials
            //    };

            //SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            //string Token = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfig.Key);
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_tokenConfig.Key);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out securityToken
            );
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (
                jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
