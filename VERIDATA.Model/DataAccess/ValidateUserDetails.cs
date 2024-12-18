namespace VERIDATA.Model.DataAccess
{
    public class ValidateUserDetails : BaseApiResponse
    {
        public int dbUserType { get; set; }
        public int userStatus { get; set; }
        public int userId { get; set; }
        public string? userMailId { get; set; }
        public string? userName { get; set; }
        public string? clientId { get; set; }
        public string? otp { get; set; }
    }
}