namespace VERIDATA.Model.Response
{
    public class UnzipAadharDataResponse
    {
        public bool IsValid { get; set; }
        public string? FileContent { get; set; }
        public string? Message { get; set; }
    }
}
