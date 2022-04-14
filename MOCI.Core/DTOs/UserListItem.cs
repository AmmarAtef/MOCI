using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.DTOs
{
    public class UserListItem
    {
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public long Id { get; set; }
    }
}
