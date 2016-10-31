using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Example
{
    public class Planet
    {
        private string _name;

        public int Radius { get; set; }
        public int Population { get; set; }
        public double Mass { get; set; }
        public int Sattelites { get; set; }
        public string GetName { get { return _name; } }
        public string SetName { set { _name = value; } }
    }
}
