using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.Entities
{
  public  class Terminal
    {
		public int Id { get; set; }
		public string TerminalId { get; set; }
		public string MerchantId { get; set; }
		public string Location { get; set; }
		public string Department { get; set; }
		public string Account { get; set; }

	}
}
