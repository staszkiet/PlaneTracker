using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public class Flight : Entity
    {
        public ulong Origin { get; set; }
        public ulong Target { get; set; }
        public string TakeoffTime { get; init; }
        public string LandingTime { get; init; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float? AMSL { get; set; }
        public ulong PlaneID { get; set; }
        public ulong[] Crew { get; set; }
        public ulong[] Load { get; set; }

        public Flight(ulong iD, ulong origin, ulong target, string takeoffTime, string landingTime, float longitude, float latitude, float? aMSL, ulong planeID, ulong[] crew, ulong[] load)
        {
            ID = iD;
            Origin = origin;
            Target = target;
            TakeoffTime = takeoffTime;
            LandingTime = landingTime;
            Longitude = longitude;
            Latitude = latitude;
            AMSL = aMSL;
            PlaneID = planeID;
            Crew = crew;
            Load = load;
        }
    }
}
