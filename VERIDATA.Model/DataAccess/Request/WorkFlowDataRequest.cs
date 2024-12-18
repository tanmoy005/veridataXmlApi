namespace VERIDATA.Model.DataAccess.Request
{
    public class WorkFlowDataRequest
    {
        public int appointeeId { get; set; }
        public int? workflowState { get; set; }
        public string? approvalStatus { get; set; }
        public string? remarks { get; set; }
        public int userId { get; set; }
    }
}