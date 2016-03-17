using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automapper
{
    public class SourceWithNestedMembers1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InnerSource1 Inner { get; set; }
    }
}
