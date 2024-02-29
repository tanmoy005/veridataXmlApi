using Microsoft.IdentityModel.Tokens;
using PfcAPI.Model.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PfcAPI.Infrastucture.Authentication
{
    public class TokenAuth : ITokenAuth
    {
        private readonly TokenConfiguration _tokenConfig;
        public TokenAuth(TokenConfiguration tokenConfig)
        {
            _tokenConfig = tokenConfig;
        }
        public string createToken(string? UserName, string? Role)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig?.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //var Subject = new Claim(new Claim[]
            //  {
            //        new Claim(ClaimTypes.Name, "UserName"),
            //        new Claim(ClaimTypes.Role, "Role")
            //  });
            //var token = new JwtSecurityToken("your-issuer", "your-audience", null, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenConfig?.Issuer,
                Audience = _tokenConfig?.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Role, Role)
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_tokenConfig?.Timeout ?? 0),
                // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var Token = tokenHandler.WriteToken(token);
            return Token;
        }


        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfig.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

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

    }
}
