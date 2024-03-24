using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public class Airport : Entity
    {
        public ulong ID { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }
        public float AMSL { get; init; }
        public string Country { get; init; }
        public Airport(ulong ID, string Name, string Code, float Longitude, float Latitude, float AMSL, string Country)
        {
            this.ID = ID;
            this.Name = Name;
            this.Code = Code;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
            this.AMSL = AMSL;
            this.Country = Country;
        }
    }
}
