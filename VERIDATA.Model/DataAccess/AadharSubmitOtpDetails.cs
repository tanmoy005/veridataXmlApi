﻿namespace VERIDATA.Model.DataAccess
{
    public class AadharSubmitOtpDetails : BaseApiResponse
    {
        public string? Name { get; set; }
        public string? Dob { get; set; }
        public string? Gender { get; set; }
        public string? CareOf { get; set; }
        public string? AadharNumber { get; set; }
        public string? MobileNumberHash { get; set; }
        public string? AadharImage { get; set; }
    }
}