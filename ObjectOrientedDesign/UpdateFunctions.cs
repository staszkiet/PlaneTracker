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
            float NewLatitude;
            float NewLongitude;
            if (takeoffticks < nowticks && landingticks > nowticks)
            {
                if (fl.Latitude == int.MaxValue)
                {
                    float l = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                    float k = (float)(nowticks - takeoffticks) / (landingticks - takeoffticks);
                    NewLatitude = Single.Lerp(origin.Latitude, dest.Latitude, l);
                    NewLongitude = Single.Lerp(origin.Longitude, dest.Longitude, k);
                }
                else
                {
                    float latdiff = dest.Latitude - fl.Latitude;
                    float londiff = dest.Longitude - fl.Longitude;
                    TimeSpan timediff = landing - DateTime.Now;
                    float secs = (float)timediff.TotalSeconds;
                    float latspeed = latdiff / secs;
                    float lonspeed = londiff / secs;
                    NewLatitude = fl.Latitude + latspeed;
                    NewLongitude = fl.Longitude + lonspeed;
                }
                double degrees;
                var pos1 = SphericalMercator.FromLonLat(fl.Longitude, fl.Latitude);
                var pos2 = SphericalMercator.FromLonLat(NewLongitude, NewLatitude);
                fl.Update("worldposition.lat", NewLatitude.ToString());
                fl.Update("worldposition.lon", NewLongitude.ToString());
                degrees = Math.Atan2(pos2.x - pos1.x, pos2.y - pos1.y);
                list.Add(new FlightAdapter(fl, NewLatitude, NewLongitude, degrees));
            }
            
          
        }
    }
}
