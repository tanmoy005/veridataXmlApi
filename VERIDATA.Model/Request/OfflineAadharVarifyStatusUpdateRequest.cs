namespace VERIDATA.Model.Request
{
    public class OfflineAadharVarifyStatusUpdateRequest
    {
        public int AppointeeId { get; set; }
        public bool? OfflineKycStatus { get; set; }
        public int UserId { get; set; }
    }
}