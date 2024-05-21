using DynamicData;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public abstract class Plane : Entity
    {
        protected Plane(ulong iD, string? serial, string? country, string? model): base()
        {
            ID = iD;
            Serial = serial;
            Country = country;
            Model = model;
            NameToType.Add("serial", "string");
            NameToType.Add("countrycode", "string");
            NameToType.Add("model", "string");
            NameToUpdateFunc.Add("serial", UpdateSerial);
            NameToUpdateFunc.Add("countrycode", UpdateCountry);
            NameToUpdateFunc.Add("model", UpdateModel);
            NameToUpdateFunc.Add("id", UpdateId);
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("serial", this.Serial.ToString());
            NameToValue.Add("countrycode", this.Country.ToString());
            NameToValue.Add("model", this.Model.ToString());
        }

        public void UpdateSerial(string s)
        {
            this.Serial = s;
        }

        public void UpdateCountry(string s)
        {
            this.Country = s;
        }

        public void UpdateModel(string s)
        {
            this.Model = s;
        }

        public override void ChangeID(IDUpdateArgs e, ListsDatabase l)
        {
            List<Flight> fl = l.flights;
            List<Flight> affectedfl = new List<Flight>();
            affectedfl = fl.FindAll((x) => (x.PlaneID == e.ObjectID));
            this.ID = e.NewObjectID;
            foreach (Flight f in affectedfl)
            {
                f.NameToValue["plane"] = this.ToString();
                f.NameToValue["plane.id"] = this.ID.ToString();
                f.NameToValue["plane.serial"] = this.Serial.ToString();
                f.NameToValue["plane.model"] = this.Model.ToString();
                f.NameToValue["plane.countrycode"] = this.Country.ToString();
            }
        }
        public string? Serial { get; private set; }
        public string? Country { get; private set; }
        public string? Model { get; private set; }

    }

    public class CargoPlane : Plane, IReportable
    {
        public CargoPlane(ulong ID, string Serial, string Country, string Model, float MaxLoad) : base(ID, Serial, Country, Model)
        {
            this.MaxLoad = MaxLoad;
            NameToType.Add("maxload", "float");
            NameToUpdateFunc.Add("maxload", UpdateMaxLoad);
            NameToValue.Add("maxload", this.MaxLoad.ToString());
        }

        [JsonPropertyOrder(1)]
        public float MaxLoad { get; private set; }

        public void UpdateMaxLoad(string s)
        {
            this.MaxLoad = float.Parse(s);
        }

        public string Accept(IMedia visitor)
        {
            return visitor.PrintCargoPlaneInfo(this);
        }

       /* public override void CreateNameToValue()
        {
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("serial", this.Serial.ToString());
            NameToValue.Add("countrycode", this.Country.ToString());
            NameToValue.Add("model", this.Model.ToString());
            NameToValue.Add("maxload", this.MaxLoad.ToString());
        }*/

        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.cargoPlanes.Remove(this);
            ld.entities.Remove(this);
        }

    }

    public class PassengerPlane : Plane, IReportable
    {
        [JsonPropertyOrder(1)]
        public ushort FirstClassSize { get; private set; }

        [JsonPropertyOrder(1)]
        public ushort BusinessClassSize { get; private set; }

        [JsonPropertyOrder(1)]
        public ushort EconomyClassSize { get; private set; }
        public PassengerPlane(ulong ID, string Serial, string Country, string Model, ushort FirstClassSize, ushort BusinessClassSize, ushort EconomyClassSize) : base(ID, Serial, Country, Model)
        {
            this.FirstClassSize = FirstClassSize;
            this.EconomyClassSize = EconomyClassSize;
            this.BusinessClassSize = BusinessClassSize;
            NameToType.Add("firstclasssize", "ushort");
            NameToType.Add("businessclasssize", "ushort");
            NameToType.Add("economyclasssize", "ushort");
            NameToUpdateFunc.Add("firstclasssize", UpdateFirstClassSize);
            NameToUpdateFunc.Add("businessclasssize", UpdateBusinessClassSize);
            NameToUpdateFunc.Add("economyclasssize", UpdateEconomyClassSize);
            NameToValue.Add("firstclasssize", this.FirstClassSize.ToString());
            NameToValue.Add("businessclasssize", this.BusinessClassSize.ToString());
            NameToValue.Add("economyclasssize", this.EconomyClassSize.ToString());
        }

        public void UpdateBusinessClassSize(string s)
        {
            this.BusinessClassSize = ushort.Parse(s);
        }

        public void UpdateEconomyClassSize(string s)
        {
            this.EconomyClassSize = ushort.Parse(s);
        }
        public void UpdateFirstClassSize(string s)
        {
            this.FirstClassSize = ushort.Parse(s);
        }
        public string Accept(IMedia visitor)
        {
            return visitor.PrintPassengerPlaneInfo(this);
        }

        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.passengerPlanes.Remove(this);
            ld.entities.Remove(this);
        }
    }
}
