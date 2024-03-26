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
            if (takeoffticks > landingticks)
            {
                landing = landing.AddDays(1);
                landingticks = landing.Ticks;
            }
            if (takeoffticks < nowticks && landingticks > nowticks)
            {
                float l = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                float k = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                //    float NewLatitude = origin.Latitude + l * (dest.Latitude - origin.Latitude);
                //    float NewLongitude = origin.Longitude + k * (dest.Longitude - origin.Longitude);
                float NewLatitude = Single.Lerp(origin.Latitude, dest.Latitude, l);
                float NewLongitude = Single.Lerp(origin.Longitude, dest.Longitude, k);
                double degrees;
                if (fl.Latitude == int.MaxValue)
                { 
                    degrees = Math.Atan2(dest.Longitude - origin.Longitude, dest.Latitude - origin.Latitude);
                }
                else
                {
                    var pos1 = SphericalMercator.FromLonLat(fl.Longitude, fl.Latitude);
                    var pos2 = SphericalMercator.FromLonLat(NewLongitude, NewLatitude);
                    fl.Latitude = NewLatitude;
                    fl.Longitude = NewLongitude;
                    degrees = Math.Atan2(pos2.x - pos1.x, pos2.y - pos1.y);
                }
                
                list.Add(new FlightAdapter(fl, NewLatitude, NewLongitude, degrees));
            }
        }
    }
}
