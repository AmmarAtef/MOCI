using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MOCI.Core.DTOs
{
    public class UserDto : BaseDto<long>
    {
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        
        public bool Enabled { get; set; }
    }
}
