using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automapper
{
    public class Source1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InnerSource1 Inner { get; set; }

        private int _total = 0;
        public int GetTotal()
        {
            return _total;
        }

        public void Add(int i)
        {
            _total += i;
        }
    }
}
