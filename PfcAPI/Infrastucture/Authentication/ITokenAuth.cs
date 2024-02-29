namespace PfcAPI.Infrastucture.Authentication
{
    public interface ITokenAuth
    {
        public string createToken(string? UserName, string? Role);
        public int? ValidateToken(string token);
    }
}
