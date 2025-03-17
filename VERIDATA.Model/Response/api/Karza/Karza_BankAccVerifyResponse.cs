using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_BankAccVerifyResponse : Karza_BaseResponse
    {
        public BankTransactionResult? result { get; set; }
    }

    public class BankTransactionResult
    {
        public bool BankTxnStatus { get; set; }
        public string? AccountNumber { get; set; }
        public string? Ifsc { get; set; }
        public string? AccountName { get; set; }
        public string? BankResponse { get; set; }
    }
}