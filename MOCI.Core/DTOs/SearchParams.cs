using System;
using System.ComponentModel.DataAnnotations;

namespace MOCI.Core.DTOs
{
    public class SearchParams
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

        [Display(Name = "Company Name")]
        public string COMPANY_NAME { get; set; }

        [Display(Name = "Commercial No")]
        public string COMMERCIAL_NO { get; set; }

        [Display(Name = "Applicant Name")]
        public string APPLICANT_NAME { get; set; }

    }
}
