namespace VERIDATA.Model.DataAccess.Response
{
    public class ApiConfigResponse
    {
        public string? apiName { get; set; }
        public string? apiBaseUrl { get; set; }
        public string? apiUrl { get; set; }
        public string? apiProvider { get; set; }
        public string? typeCode { get; set; }
        public bool? activeStatus { get; set; }
        public int? apiPrioirty { get; set; }
    }
}