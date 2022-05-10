using System;
using System.ComponentModel.DataAnnotations;

namespace MOCI.Core.DTOs
{
    public class SearchParams
    {
        [Display(Name = "TransactionDate")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDate { get; set; }


        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }


        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Display(Name = "Approved Code")]
        public string ApprovedCode { get; set; }

    }
}
