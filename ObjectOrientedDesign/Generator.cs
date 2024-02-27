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
    public interface IGenerator
    {
        public IEntity Generate(string[] s);
    }
    public class CrewGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            ulong Age = ulong.Parse(s[3]);
            string Phone = s[4];
            string Email = s[5];
            ushort Practice = ushort.Parse(s[6]);
            string Role = s[7];
            return new Crew(ID, Name, Age, Phone, Email, Practice, Role);
        }

    }

    public class PassengerGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            ulong Age = ulong.Parse(s[3]);
            string Phone = s[4];
            string Email = s[5];
            string Class = s[6];
            ulong Miles = ulong.Parse(s[7]);
            return new Passenger(ID, Name, Age, Phone, Email, Class, Miles);
        }

    }

    public class CargoGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = ulong.Parse(s[1]);
            float Weight = float.Parse(s[2], CultureInfo.InvariantCulture);
            string Code = s[3];
            string Description = s[4];
            return new Cargo(ID, Weight, Code, Description);
        }
    }

    public class CargoPlaneGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = uint.Parse(s[1]);
            string Serial = s[2];
            string Country = s[3];
            string Model = s[4];
            float MaxLoad = float.Parse(s[5], CultureInfo.InvariantCulture);
            return new CargoPlane(ID, Serial, Country, Model, MaxLoad);
        }
    }
    public class PassengerPlaneGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = uint.Parse(s[1]);
            string Serial = s[2];
            string Country = s[3];
            string Model = s[4];
            ushort FirstClassSize = ushort.Parse(s[5]);
            ushort BusinessClassSize = ushort.Parse(s[6]);
            ushort EconomyClassSize = ushort.Parse(s[7]);
            return new PassengerPlane(ID, Serial, Country, Model, FirstClassSize, BusinessClassSize, EconomyClassSize);

        }
    }

    public class AirportGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
        {
            ulong ID = ulong.Parse(s[1]);
            string Name = s[2];
            string Code = s[3];
            float Longitude = float.Parse(s[4], CultureInfo.InvariantCulture);
            float Latitude = float.Parse(s[5], CultureInfo.InvariantCulture);
            float AMSL = float.Parse(s[6], CultureInfo.InvariantCulture);
            string Country = s[7];
            return new Airport(ID, Name, Code, Longitude, Latitude, AMSL, Country);
        }
    }

    public class FlightGenerator : IGenerator
    {
        public IEntity Generate(string[] s)
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
            CrewTab[0] = CrewTab[0].Remove(0, 1);
            CrewTab[CrewTab.Length - 1] = CrewTab[CrewTab.Length - 1].Remove(CrewTab[CrewTab.Length - 1].Length - 1, 1);
            LoadTab[0] = LoadTab[0].Remove(0, 1);
            LoadTab[LoadTab.Length - 1] = LoadTab[LoadTab.Length - 1].Remove(LoadTab[LoadTab.Length - 1].Length - 1, 1);

            for (int i = 0; i < Crew.Length; i++)
            {
                Crew[i] = ulong.Parse(CrewTab[i]);
            }
            for (int i = 0; i < Load.Length; i++)
            {
                Load[i] = ulong.Parse(LoadTab[i]);
            }

            return new Flight(ID, Origin, Target, TakeoffTime, LandingTime, Longitude, Latitude, AMSL, PlaneID, Load, Crew);
        }
    }
}
