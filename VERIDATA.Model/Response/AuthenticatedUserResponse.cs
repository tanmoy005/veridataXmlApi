namespace VERIDATA.Model.Response
{
    public class AuthenticatedUserResponse
    {
        public UserDetailsResponse? UserDetails { get; set; }
        public TokenDetailsResponse? TokenDetails { get; set; }
    }
}