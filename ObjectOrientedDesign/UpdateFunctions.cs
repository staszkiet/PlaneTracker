using Mapsui.Projections;
using ObjectOrientedDesign.Objects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    internal static class UpdateFunctions
    {
        public static void Update(Airport origin, Airport dest, Flight fl, ref List<FlightGUI> list)
        {
            long nowticks = DateTime.Now.Ticks;
            DateTime takeoff = DateTime.Parse(fl.TakeoffTime);
            DateTime landing = DateTime.Parse(fl.LandingTime);
            long takeoffticks = takeoff.Ticks;
            long landingticks = landing.Ticks;

            if (takeoffticks < nowticks && landingticks > nowticks)
            {
                float l = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                float k = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                float NewLatitude = origin.Latitude + l * (dest.Latitude - origin.Latitude);
                float NewLongitude = origin.Longitude + k * (dest.Longitude - origin.Longitude);
                double degrees = Math.Atan2(dest.Longitude - origin.Longitude, dest.Latitude - origin.Latitude);

                list.Add(new FlightAdapter(fl, NewLatitude, NewLongitude, degrees));
            }
        }
    }
}
