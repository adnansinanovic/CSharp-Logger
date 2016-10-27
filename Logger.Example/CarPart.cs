using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Example
{
    public class CarPart
    {
        public Double Price { get; set; }
        public Guid Guid { get; set; }

        public CarPart()
        {
            Guid = Guid.NewGuid();
        }
    }
}
