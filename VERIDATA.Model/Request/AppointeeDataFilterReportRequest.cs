﻿namespace VERIDATA.Model.Request
{
    public class AppointeeDataFilterReportRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? AppointeeName { get; set; }
        public string? CandidateId { get; set; }
        public string? StatusCode { get; set; }
    }
}