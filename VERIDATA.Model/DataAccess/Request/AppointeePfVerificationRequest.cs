namespace VERIDATA.Model.DataAccess.Request
{
    public class AppointeePfVerificationRequest
    {
        public int AppointeeId { get; set; }

        public bool? IsValid { get; set; }

        public string Type { get; set; } = string.Empty;

        public bool? IsPensionApplicable { get; set; }

        public bool? IsPensionGapFind { get; set; }

        public int UserId { get; set; }

        public AppointeePfVerificationRequest() { }

        public AppointeePfVerificationRequest(int appointeeId, bool? isValid, string type, bool? isPensionApplicable, bool? isPensionGapFind, int userId)
        {
            AppointeeId = appointeeId;
            IsValid = isValid;
            Type = type;
            IsPensionApplicable = isPensionApplicable;
            IsPensionGapFind = isPensionGapFind;
            UserId = userId;
        }
    }
}
