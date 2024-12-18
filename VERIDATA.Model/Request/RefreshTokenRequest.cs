namespace VERIDATA.Model.Request
{
    public class RefreshTokenRequest
    {
        //public int UserId { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}