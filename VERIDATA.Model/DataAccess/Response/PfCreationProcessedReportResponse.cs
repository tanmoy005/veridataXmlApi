using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class PfCreationProcessedReportResponse
    {
        public PfCreationProcessedReportResponse()
        {
            AppointeeData = new AppointeeDetails();
            ProcessData = new ProcessedFileData();

        }
        public AppointeeDetails? AppointeeData { get; set; }
        public ProcessedFileData? ProcessData { get; set; }
    }
}
