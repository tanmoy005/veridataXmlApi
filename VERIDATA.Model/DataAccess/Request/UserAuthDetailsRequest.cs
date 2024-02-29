namespace VERIDATA.Model.DataAccess.Request
{
    public class UserAuthDetailsRequest
    {
        public int UserId { get; set; }
        public string? ClientId { get; set; }
        public string? Token { get; set; }
        public string? Otp { get; set; }
        public string? IpaAdress { get; set; }
        public string? browserName { get; set; }
    }
}
