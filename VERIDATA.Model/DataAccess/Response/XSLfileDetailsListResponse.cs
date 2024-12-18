using System.Data;
using VERIDATA.Model.Base;

namespace VERIDATA.Model.DataAccess.Response
{
    public class XSLfileDetailsListResponse : ErrorResponse
    {
        public DataTable? ValidXlsData { get; set; }
        public DataTable? InValidXlsData { get; set; }
    }
}