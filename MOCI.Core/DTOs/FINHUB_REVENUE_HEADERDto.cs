using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.DTOs
{

    public class FINHUB_REVENUE_HEADERDto
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

    }

}
