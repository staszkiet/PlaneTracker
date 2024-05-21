using DynamicData;
using NetworkSourceSimulator;
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
        public string Name { get; set; }
        public string Code { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float AMSL { get; set; }
        public string Country { get; set; }

        public Airport(ulong ID, string Name, string Code, float Longitude, float Latitude, float AMSL, string Country): base()
        {
            this.ID = ID;
            this.Name = Name;
            this.Code = Code;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
            this.AMSL = AMSL;
            this.Country = Country;
            NameToType.Add("name", "string");
            NameToType.Add("code", "string");
            NameToType.Add("worldposition.lon", "float");
            NameToType.Add("worldposition.lan", "float");
            NameToType.Add("amsl", "float");
            NameToType.Add("countrycode", "string");
            NameToUpdateFunc.Add("name", UpdateName);
            NameToUpdateFunc.Add("code", UpdateCode);
            NameToUpdateFunc.Add("worldposition.lon", UpdateLon);
            NameToUpdateFunc.Add("worldposition.lat", UpdateLat);
            NameToUpdateFunc.Add("amsl", UpdateAMSL);
            NameToUpdateFunc.Add("country", UpdateCountry);
            NameToUpdateFunc.Add("id", UpdateId);
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("name", this.Name.ToString());
            NameToValue.Add("code", this.Code.ToString());
            NameToValue.Add("worldposition", "{" + this.Longitude.ToString() + ", " + this.Latitude.ToString() + "}");
            NameToValue.Add("amsl", this.AMSL.ToString());
            NameToValue.Add("countrycode", this.Country.ToString());
            NameToValue.Add("worldposition.lat", this.Latitude.ToString());
            NameToValue.Add("worldposition.lon", this.Longitude.ToString());
        }

        public override void UpdateId(string s)
        {
            ListsDatabase l = ListsDatabase.GetInstance();
            this.ID = ulong.Parse(s);
            List<Flight> affectedfl = l.flights.FindAll((x) => (x.Target == this.ID));
            foreach (Flight f in affectedfl)
            {
                f.NameToValue["target.id"] = s;
                f.NameToValue["target.lon"] = this.Longitude.ToString();
                f.NameToValue["target.lat"] = this.Latitude.ToString();
            }
            affectedfl = new List<Flight>();
            affectedfl = l.flights.FindAll((x) => (x.Origin == this.ID));
            foreach (Flight f in affectedfl)
            {
                f.NameToValue["origin.id"] = s;
                f.NameToValue["origin.lon"] = this.Longitude.ToString();
                f.NameToValue["origin.lat"] = this.Latitude.ToString();
            }
        }
        public void UpdateCountry(string s)
        {
            this.Country = s;
        }
        public void UpdateAMSL(string s)
        {
            this.AMSL = float.Parse(s);
        }
        public void UpdateLat(string s)
        {
            this.Latitude = float.Parse(s);
        }
        public void UpdateLon(string s)
        {
            this.Longitude = float.Parse(s);
        }
        public void UpdateName(string s)
        {
            this.Name = s;
        }
        public void UpdateCode(string s)
        {
            this.Code = s;
        }
        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.airports.Remove(this);
            ld.entities.Remove(this);
        }
        public string Accept(IMedia visitor)
        {
            return visitor.PrintAirportInfo(this);
        }

        public override void ChangeID(IDUpdateArgs e, ListsDatabase l)
        {
            this.Update("id", e.NewObjectID.ToString());
        }
    }
}
