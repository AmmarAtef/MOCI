using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MOCI.Web.Models
{
    public class FINHUB_REVENUE_HEADERModel
    {
        [Required]
        [DisplayName("Transaction Date")]
        public DateTime TRANSACTION_DATE { get; set; }

        [Required]
        [DisplayName("Transaction Amount")]
        public decimal TRANSACTION_AMOUNT { get; set; }

        [DisplayName("Invoice Number")]
        public string INVOICE_NO { get; set; }
    }
}
