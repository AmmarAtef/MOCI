using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MOCI.Core.Entities
{
    public class NLog
    {
        [Key]
        public long ID { get; set; }

        public string MachineName { get; set; }

        public DateTime Logged { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Properties { get; set; }

        public string Callsite { get; set; }

        public string Exception { get; set; }
    }
}
