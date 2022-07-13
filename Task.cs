using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_Server
{
    public record class TODO
    {
        public TODO()
        {

        }
        public TODO(int no, string name)
        {
            No = no;
            Name = name;
        }
        public int No { get; set; }
        public string? Name { get; set; }
    }
}
