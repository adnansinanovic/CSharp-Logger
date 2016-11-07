using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Example
{
    class PlanetFormatter : IDumpFormatter
    {
        public Type FormatterType
        {
            get
            {
                return typeof(Planet);
            }
        }

        public string Format(object value, int intendation)
        {
            if (value == null)
                return "null";

            Planet planet = value as Planet;
            if (planet == null)
                return $"{string.Empty.PadRight(intendation, '\t')}{value.ToString()}";

            return $"{string.Empty.PadRight(intendation, '\t')}There was one planet, named {planet.GetName}. Mass of planet was {planet.Mass}, and radius={planet.Radius}";

        }
    }
}
