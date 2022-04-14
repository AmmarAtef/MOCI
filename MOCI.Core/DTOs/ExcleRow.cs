using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace MOCI.Core.DTOs
{

	public class ImportedData : ExcleRowDtp

    {
        public long Id{ get; set; }

		[ForeignKey("User")]
		[Required(ErrorMessage = "The User is required")]
		public long UserId { get; set; }
		public User User { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public string Guid { get; set; }
	  public string ACCOUNT_NUMBER { get; set; }
     
    }

 
	public class ExcleRowDtp
	{


		public String Merchant_Id { get; set; }
		public  String Location { get; set; }
		public  String Terminal_Id { get; set; }
		public  String Trxn_Type { get; set; }
		[NotMapped]
		public string Trxn_Date { get; set; }
       
		public DateTime? Trxn_DateValue
		{

			get
			{
				if (Trxn_Date != null && Trxn_Date != "")
				{
					CultureInfo culture2 = new CultureInfo("en-US");
					return DateTime.ParseExact(Trxn_Date, "dd-MMM-yyyy", culture2, DateTimeStyles.None);
				}
				return null;
			
			}
			set
            {
				Trxn_Date = value.Value.ToString("dd-MMM-yyyy");

			}
		}
		public  TimeSpan Trxn_Time { get; set; }
		[NotMapped]
		public  String Posting_Date { get; set; }
		public DateTime? Posting_DateValue
		{
			get
			{
				if (Posting_Date != null && Posting_Date != "")
				{
					CultureInfo culture2 = new CultureInfo("en-US");
					return DateTime.ParseExact(Posting_Date, "dd-MMM-yyyy", culture2, DateTimeStyles.None);
				}
				return null;

			}
			set
			{
				Posting_Date = value.Value.ToString("dd-MMM-yyyy");

			}
		}
		public decimal Amount { get; set; }
		public decimal Commission { get; set; }
		public decimal Net_Amount { get; set; }
		public  String Card_Number { get; set; }
		public  String Card_Type { get; set; }
		public String Approved_Code { get; set; }
		public String Invoice_No { get; set; }



	}
	[Serializable]
	public class CombineItem
    {
		//public decimal SumOfDetails { get; set; }
		public ExcleRowDtp ExcleRow { get; set; }
	 
		public FINHUB_REVENUE_HEADER MOCI { get; set; }
    }

	//public class ReportRespons
 //   {
		 
	//	public List<CombineItem> Data;
 //       public int MatchedCount { get; set; }
	//	public decimal MatchedAmount { get; set; }
	//	public int UnmtachedCount { get; set; }
	//	public decimal UnmtachedAmount { get; set; }
	//}
	public class ReportRespons
	{

        public string Key { get; set; }
		public decimal Value { get; set; }
	}
	public class CustomeList
	{
		public string UploadDate { get; set; }
		public string Guid { get; set; }
		public string File { get; set; }
		public string User { get; set; }
		public string Date { get; set; }
		public string Account { get; set; }
	}

	public class SaveEntity
    {
		public dynamic Data { get; set; }
		public FileDto File { get; set; }
        public string Account { get; set; }
    }
}
