using ObjectOrientedDesign.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ObjectOrientedDesign
{
    
    public class FlightAdapter : FlightGUI
    {
        private Flight f;

        public FlightAdapter(Flight fl, double lattitude, double longitude, double degree)
        {
            f = fl;
            this.ID = fl.ID;
            this.WorldPosition = new WorldPosition(lattitude, longitude);
            this.MapCoordRotation = degree;
        }

    }
}
