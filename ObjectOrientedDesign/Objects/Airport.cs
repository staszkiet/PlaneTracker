using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public class Airport : Entity, IReportable
    {
        public string Name { get; init; }
        public string Code { get; init; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float AMSL { get; set; }
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

        public string Accept(IMedia visitor)
        {
            return visitor.PrintAirportInfo(this);
        }
    }
}
