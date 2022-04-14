using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.Web.Models
{
    public class Node
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public List<Node> Nodes { get; set; }
    }

    public class EmployeeNode
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
