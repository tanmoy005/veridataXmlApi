using System.Security.Claims;

namespace VERIDATA.BLL.Authentication
{
    public interface ITokenAuth
    {
        public string createToken(string? UserName, string? Role, int? userId);

        public string GenerateAccessToken(IEnumerable<Claim> claims);

        public string GenerateRefreshToken();

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}