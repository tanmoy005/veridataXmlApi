namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetUanDetailsByPanRequest
    {
        public Signzy_GetUanDetailsByPanRequest()
        {
            mobileNumber = string.Empty;
            uanNumber = string.Empty;
            dateOfBirth = string.Empty;
            employeeName = string.Empty;
            employerName = string.Empty;
            nameMatchMethod = string.Empty;
        }

        public string? panNumber { get; set; }
        public string mobileNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string uanNumber { get; set; }
        public string employeeName { get; set; }
        public string employerName { get; set; }
        public string nameMatchMethod { get; set; }
    }
}