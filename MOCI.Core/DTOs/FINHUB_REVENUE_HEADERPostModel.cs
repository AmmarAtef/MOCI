using MOCI.Core.Entities;
using System;
using System.Collections.Generic;

namespace MOCI.Web.Models
{
    public class FINHUB_REVENUE_HEADERPostModel
    {
        public DateTime TRANSACTION_DATE { get; set; }
        public decimal TRANSACTION_AMOUNT { get; set; }
        public string INVOICE_NO { get; set; }
        public string TERMINAL_ID { get; set; }

        public string ACCOUNT_NUMBER { get; set; }
        public string DEPARTMENT { get; set; }
        public string LEDGER_ACCOUNT { get; set; }
        public string SERVICE_NAME { get; set; }


    }
}
