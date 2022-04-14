using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MOCI.Core.DTOs
{
  public  class TerminalDto
	{
		 
		public int Id { get; set; }
		[Required]
		public string TerminalId { get; set; }
		[Required]
		public string MerchantId { get; set; }
		public string Location { get; set; }
		[Required]
		public string Department { get; set; }
		[Required]
		public string Account { get; set; }

	}
}
