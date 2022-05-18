using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.Entities
{
    public class CustomerData
    {
        public string COMPANY_NAME { get; set; }
        public string COMMERCIAL_NO { get; set; }
        public string COMMERCIAL_LICENSE_NO { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string APPLICANT_ID { get; set; }
        public string APPLICANT_NAME { get; set; }
        public DateTime TRANSACTION_DATE { get; set; }
        public DateTimeOffset TRANSACTION_TIME { get; set; }
        public decimal TRANSACTION_AMOUNT { get; set; }
    }
}
