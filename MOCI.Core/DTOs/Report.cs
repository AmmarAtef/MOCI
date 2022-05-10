using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MOCI.Core.DTOs
{
    public class Report
    {
        [Display(Name = "Transaction Date From")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDateFrom { get; set; }

        [Display(Name = "Transaction Date To")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDateTo { get; set; }
    }

  
}
