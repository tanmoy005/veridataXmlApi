namespace VERIDATA.Model.Request
{
    public class ModifyAadhaarValidatedFieldRequest
    {
        public int AppointeeId { get; set; }
        public string? Name { get; set; }
        public string? FathersName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}