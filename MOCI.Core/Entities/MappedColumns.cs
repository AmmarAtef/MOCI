using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MOCI.Core.Entities
{
    public class MappedColumns
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MappedFrom { get; set; }
        [Required]
        public string MappedTo { get; set; }
    }
}
