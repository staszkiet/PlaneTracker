using DynamicData;
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
        public string TakeoffTime { get; private set; }
        public string LandingTime { get; private set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float? AMSL { get; set; }
        public ulong PlaneID { get; set; }
        public ulong[] Crew { get; set; }
        public ulong[] Load { get; set; }

        public Flight(ulong iD, ulong origin, ulong target, string takeoffTime, string landingTime, float longitude, float latitude, float? aMSL, ulong planeID, ulong[] crew, ulong[] load): base()
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
            NameToType.Add("origin.lon","float");
            NameToType.Add("target.lat", "float");
            NameToType.Add("origin.lat", "float");
            NameToType.Add("target.lon", "float");
            NameToType.Add("takeofftime", "string");
            NameToType.Add("landingtime", "string");
            NameToType.Add("worldposition.lon", "float");
            NameToType.Add("worldposition.lat", "float");
            NameToType.Add("amsl", "float");
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("worldposition", "{" + this.Longitude.ToString() + ", " + this.Latitude.ToString() + "}");
            if (TakeoffTime == "0")
            {
                TakeoffTime = "0:00";
            }
            DateTime tkf = DateTime.Parse(TakeoffTime);
            NameToValue.Add("takeofftime", tkf.ToString());
            if (LandingTime == "0")
            {
                LandingTime = "0:00";
            }
            DateTime lnt = DateTime.Parse(LandingTime);
            if (tkf > lnt)
            {
                lnt = lnt.AddDays(1);
            }
            NameToValue.Add("landingtime", lnt.ToString());
            ListsDatabase db = ListsDatabase.GetInstance();
            Airport? o = db.airports.Find((x) => x.ID == this.Origin);
            Airport? t = db.airports.Find((x) => x.ID == this.Target);
            if (o != null)
            {
                NameToValue.Add("origin", "{" + o.Longitude.ToString() + ", " + o.Latitude.ToString() + "}");
            }
            else
            {
                NameToValue.Add("origin", "{" + "invalid" + ", " + "invalid" + "}");
            }
            if (t != null)
            {
                NameToValue.Add("target", "{" + t.Longitude.ToString() + ", " + t.Latitude.ToString() + "}");
            }
            else
            {
                NameToValue.Add("target", "{" + "invalid" + ", " + "invalid" + "}");
            }
            Plane? p = db.passengerPlanes.Find((x) => x.ID == this.PlaneID);
            if (p == null)
            {
                p = db.cargoPlanes.Find((x) => x.ID == this.PlaneID);
            }
            if (p == null)
            {
                NameToValue.Add("plane", "invaid");
            }
            else
            {
                NameToValue.Add("plane", p.ToString());
                NameToValue.Add("plane.id", p.ID.ToString());
                NameToValue.Add("plane.serial", p.Serial.ToString());
                NameToValue.Add("plane.model", p.Model.ToString());
                NameToValue.Add("plane.countrycode", p.Country.ToString());
            }
            NameToUpdateFunc.Add("origin.id", UpdateOrigin);
            NameToUpdateFunc.Add("target.id", UpdateTarget);
            NameToUpdateFunc.Add("takeofftime", UpdateTakeOffTime);
            NameToUpdateFunc.Add("landingtime", UpdateLandingTime);
            NameToUpdateFunc.Add("worldposition.lat", UpdateLat);
            NameToUpdateFunc.Add("worldposition.lon", UpdateLon);
            NameToUpdateFunc.Add("plane.id", UpdatePlane);
            NameToUpdateFunc.Add("amsl", UpdateAMSL);
            NameToUpdateFunc.Add("id", UpdateId);
        }

        public void UpdateOrigin(string s)
        {
            ulong id = ulong.Parse(s);
            ListsDatabase db = ListsDatabase.GetInstance();
            Airport? a = db.airports.Find((x) => x.ID == id);
            if (a != null)
            {
                this.Origin = id;
                NameToValue["origin.id"] = s;
                NameToValue["origin.lon"] = a.Longitude.ToString();
                NameToValue["origin.lat"] = a.Latitude.ToString();
            }
        }

        public void UpdateTarget(string s)
        {
            ulong id = ulong.Parse(s);
            ListsDatabase db = ListsDatabase.GetInstance();
            Airport? a = db.airports.Find((x) => x.ID == id);
            if (a != null)
            {
                this.Target = id;
                NameToValue["target.id"] = s;
                NameToValue["target.lon"] = a.Longitude.ToString();
                NameToValue["target.lat"] = a.Latitude.ToString();
            }
        }

        public void UpdateTakeOffTime(string s)
        {
            this.TakeoffTime = s;
            DateTime dt = DateTime.Parse(s);
            NameToValue["takeofftime"] = dt.ToString();
        }

        public void UpdateLandingTime(string s)
        {
            this.LandingTime = s;
            DateTime dt = DateTime.Parse(s);
            NameToValue["landingtime"] = dt.ToString();
        }
        public void UpdateLat(string s)
        {
            this.Latitude = float.Parse(s);
            NameToValue["worldposition.lat"] = s;
            NameToValue["worldposition"] = "{" + this.Longitude.ToString() + ", " + this.Latitude.ToString() + "}";
        }
        public void UpdateLon(string s)
        {
            this.Longitude = float.Parse(s);
            NameToValue["worldposition.lon"] = s;
            NameToValue["worldposition"] = "{" + this.Longitude.ToString() + ", " + this.Latitude.ToString() + "}";
        }
        public void UpdateAMSL(string s)
        {
            this.AMSL = float.Parse(s);
            NameToValue["amsl"] = s;
        }
        public void UpdatePlane(string s)
        {
            ulong id = ulong.Parse(s);
            ListsDatabase db = ListsDatabase.GetInstance();
            Plane? a = db.cargoPlanes.Find((x) => x.ID == id) != null ? db.cargoPlanes.Find((x) => x.ID == id) : db.passengerPlanes.Find((x) => x.ID == id);
            if (a == null)
            {
                return;
            }
            else
            {
                this.PlaneID = id;
                NameToValue["plane"] = a.ToString();
                NameToValue["plane.id"] = a.ID.ToString();
                NameToValue["plane.serial"] = a.Serial.ToString();
                NameToValue["plane.model"] = a.Model.ToString();
                NameToValue["plane.countrycode"] = a.Country.ToString();
            }
        }
        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.flights.Remove(this);
            ld.entities.Remove(this);
        }

        public override void Update(string what, string how)
        {
            Action<string> f = NameToUpdateFunc[what];
            f(how);
        }
    }
}
