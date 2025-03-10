﻿namespace VERIDATA.Model.Response
{
    public class PfUserListResponse
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public int? appointeeId { get; set; }
        public string? candidateId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; } //number that varified with aadhar
        public string? adhaarNo { get; set; }
        public string? panNo { get; set; }
        public string? uanNo { get; set; }
        public string? status { get; set; }
        public string? isPensionApplicable { get; set; }
        public bool? isTrustPFApplicable { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public decimal? epfWages { get; set; }
        public string? uanAadharLink { get; set; }
    }
}