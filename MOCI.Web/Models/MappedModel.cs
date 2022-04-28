using System;

namespace MOCI.Web.Models
{
    public class MappedModel
    {

		public String Merchant_Id { get; set; }
		public String Location { get; set; }
		public String Terminal_Id { get; set; }
		public String Trxn_Type { get; set; }
		
		public String Trxn_DateValue { get; set; }
		public String Trxn_Time { get; set; }
		
		public String Posting_DateValue { get; set; }
		public String Amount { get; set; }
		public String Commission { get; set; }
		public String Net_Amount { get; set; }
		public String Card_Number { get; set; }
		public String Card_Type { get; set; }
		public String Approved_Code { get; set; }
		public String Invoice_No { get; set; }

	}
}
