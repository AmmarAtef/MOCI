using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.Entities
{
    public class FINHUB_REVENUE_HEADER
    {
         
        public string SOURCE { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public DateTime TRANSACTION_DATE { get; set; }
        public DateTimeOffset TRANSACTION_TIME { get; set; }
        public decimal TRANSACTION_AMOUNT { get; set; }
        public string APPROVED_CODE { get; set; }

        public string INVOICE_NO { get; set; }
        public string RESPONSE_CODE { get; set; }
        public string TERMINAL_ID { get; set; }
        public string CARD_NUMBER { get; set; }

        public string ACCOUNT_NUMBER { get; set; }
        public DateTime LOAD_DATE { get; set; }
        public List<FINHUB_REVENUE_DETAIL> Details { get; set; }
        public bool? Ismatched { get; set; }
        public DateTime? MatchedTime { get; set; }
        public long? UserId{ get; set; }
        public string? UserName { get; set; }
        public long? ImportedRowId { get; set; }
        public bool? IsManual { get; set; }

    }
    public class FINHUB_REVENUE_DETAIL
    {
        public string SOURCE { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string SERVICE_NAME { get; set; }
        public string ENTITY { get; set; }
        public string LEDGER_ACCOUNT { get; set; }
        public string DEPARTMENT { get; set; }
        public decimal FEES_AMOUNT { get; set; }
        public DateTime LOAD_DATE { get; set; }
    }
}
