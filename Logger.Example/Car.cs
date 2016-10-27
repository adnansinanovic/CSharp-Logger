using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Example
{
    public class Car
    {
        private ColorEnum privateColor = ColorEnum.Red;        
        protected ColorEnum protectedColor = ColorEnum.Blue;
        internal ColorEnum internalColor = ColorEnum.Green;
        internal protected ColorEnum internalProtectedColor = ColorEnum.White;
        public ColorEnum publicColor = ColorEnum.Violet;

        public CarPart Wheel { get; set; }
        public CarPart Window { get; set; }
    }
}
