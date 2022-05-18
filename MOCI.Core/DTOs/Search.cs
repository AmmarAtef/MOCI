using System;
using System.ComponentModel.DataAnnotations;

namespace MOCI.Core.DTOs
{
    public class Search
    {
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime? TRANSACTION_DATE { get; set; }

        [Display(Name = "Invoice No")]
        public string INVOICE_NO { get; set; }

        [Display(Name = "Card Number")]
        public string CARD_NUMBER { get; set; }

        [Display(Name = "Approved Code")]
        public string APPROVED_CODE { get; set; }


    }
}
