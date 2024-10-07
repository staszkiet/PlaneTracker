using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ObjectOrientedDesign.Objects;

namespace ObjectOrientedDesign
{
    public abstract class Generator
    {
        public abstract void Generate(string[] s, ref ListsDatabase l);

        public Dictionary<string, int> NameToIndex = new Dictionary<string, int>();
    }
    public class CrewGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            ulong Age = ulong.Parse(s[3]);
            string Phone = s[4];
            string Email = s[5];
            ushort Practice = ushort.Parse(s[6]);
            string Role = s[7];
            Crew c = new Crew(ID, Name, Age, Phone, Email, Practice, Role);
            l.crews.Add(c);
            l.entities.Add(c);
        }
        public CrewGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("name", 2);
            NameToIndex.Add("age", 3);
            NameToIndex.Add("phone", 4);
            NameToIndex.Add("email", 5);
            NameToIndex.Add("practise", 6);
            NameToIndex.Add("role", 7);
        }
    }

    public class PassengerGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            ulong Age = ulong.Parse(s[3]);
            string Phone = s[4];
            string Email = s[5];
            string Class = s[6];
            ulong Miles = ulong.Parse(s[7]);
            Passenger p = new Passenger(ID, Name, Age, Phone, Email, Class, Miles);
            l.passengers.Add(p);
            l.entities.Add(p);
        }
        public PassengerGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("name", 2);
            NameToIndex.Add("age", 3);
            NameToIndex.Add("phone", 4);
            NameToIndex.Add("email", 5);
            NameToIndex.Add("class", 6);
            NameToIndex.Add("miles", 7);
        }
    }

    public class CargoGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            float Weight = float.Parse(s[2], CultureInfo.InvariantCulture);
            string Code = s[3];
            string Description = s[4];
            Cargo c = new Cargo(ID, Weight, Code, Description);
            l.cargos.Add(c);
            l.entities.Add(c);
        }

        public CargoGenerator()
        {
            NameToIndex = new Dictionary<string, int>();
            NameToIndex.Add("id", 1);
            NameToIndex.Add("weight", 2);
            NameToIndex.Add("code", 3);
            NameToIndex.Add("description", 4);
        }
    }

    public class CargoPlaneGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            string Serial = s[2];
            string Country = s[3];
            string Model = s[4];
            float MaxLoad = float.Parse(s[5], CultureInfo.InvariantCulture);
            CargoPlane cp = new CargoPlane(ID, Serial, Country, Model, MaxLoad);
            l.cargoPlanes.Add(cp);
            l.entities.Add(cp);
        }

        public CargoPlaneGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("serial", 2);
            NameToIndex.Add("country", 3);
            NameToIndex.Add("model", 4);
            NameToIndex.Add("maxload", 5);
        }
    }
    public class PassengerPlaneGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            string Serial = s[2];
            string Country = s[3];
            string Model = s[4];
            ushort FirstClassSize = ushort.Parse(s[5]);
            ushort BusinessClassSize = ushort.Parse(s[6]);
            ushort EconomyClassSize = ushort.Parse(s[7]);
            PassengerPlane pp = new PassengerPlane(ID, Serial, Country, Model, FirstClassSize, BusinessClassSize, EconomyClassSize);
            l.passengerPlanes.Add(pp);
            l.entities.Add(pp);

        }

        public PassengerPlaneGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("serial", 2);
            NameToIndex.Add("country", 3);
            NameToIndex.Add("model", 4);
            NameToIndex.Add("firstclasssize", 5);
            NameToIndex.Add("businessclasssize", 6);
            NameToIndex.Add("economyclasssize", 7);
        }
    }

    public class AirportGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            string Code = s[3];
            float Longitude = float.Parse(s[4], CultureInfo.InvariantCulture);
            float Latitude = float.Parse(s[5], CultureInfo.InvariantCulture);
            float AMSL = float.Parse(s[6], CultureInfo.InvariantCulture);
            string Country = s[7];
            Airport a = new Airport(ID, Name, Code, Longitude, Latitude, AMSL, Country);
            l.airports.Add(a);
            l.entities.Add(a);
        }

        public AirportGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("name", 2);
            NameToIndex.Add("code", 3);
            NameToIndex.Add("worldposition.lon", 4);
            NameToIndex.Add("worldposition.lat", 5);
            NameToIndex.Add("amsl", 6);
            NameToIndex.Add("countrycode", 7);
        }
    }

    public class FlightGenerator : Generator
    {
        public override void Generate(string[] s, ref ListsDatabase l)
        {
            ulong ID = ulong.Parse(s[1]);
            ulong Origin = ulong.Parse(s[2]);
            ulong Target = ulong.Parse(s[3]);
            string TakeoffTime = s[4];
            string LandingTime = s[5];
            float Longitude = float.Parse(s[6], CultureInfo.InvariantCulture);
            float Latitude = float.Parse(s[7], CultureInfo.InvariantCulture);
            float AMSL = float.Parse(s[8], CultureInfo.InvariantCulture);
            ulong PlaneID = ulong.Parse(s[9]);
            string[] CrewTab = s[10].Split(";");
            string[] LoadTab = s[11].Split(";");
            ulong[] Crew = new ulong[CrewTab.Length];
            ulong[] Load = new ulong[LoadTab.Length];
            if (CrewTab[0] != "0")
            {
                CrewTab[0] = CrewTab[0].Remove(0, 1);
                CrewTab[CrewTab.Length - 1] = CrewTab[CrewTab.Length - 1].Remove(CrewTab[CrewTab.Length - 1].Length - 1, 1);
                for (int i = 0; i < Crew.Length; i++)
                {
                    Crew[i] = ulong.Parse(CrewTab[i]);
                }
            }
            if (LoadTab[0] != "0")
            {
                LoadTab[0] = LoadTab[0].Remove(0, 1);
                LoadTab[LoadTab.Length - 1] = LoadTab[LoadTab.Length - 1].Remove(LoadTab[LoadTab.Length - 1].Length - 1, 1);
                for (int i = 0; i < Load.Length; i++)
                {
                    Load[i] = ulong.Parse(LoadTab[i]);
                }
            }
            
            Flight f = new Flight(ID, Origin, Target, TakeoffTime, LandingTime, Longitude, Latitude, AMSL, PlaneID, Load, Crew);
            l.flights.Add(f);
            l.entities.Add(f);
        }
        public FlightGenerator()
        {
            NameToIndex.Add("id", 1);
            NameToIndex.Add("origin.id", 2);
            NameToIndex.Add("target.id", 3);
            NameToIndex.Add("takeofftime", 4);
            NameToIndex.Add("landingtime", 5);
            NameToIndex.Add("worldposition.lon", 6);
            NameToIndex.Add("worldposition.lat", 7);
            NameToIndex.Add("amsl", 8);
            NameToIndex.Add("plane.id", 9);
        }

    }
}
