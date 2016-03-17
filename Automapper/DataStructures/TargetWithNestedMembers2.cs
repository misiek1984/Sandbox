using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automapper
{
    public class TargetWithNestedMembers2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InnerTarget2 Inner { get; set; }
    }
}
