using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace projektowanie
{
    [JsonDerivedType(typeof(Airport), "AI")]
    [JsonDerivedType(typeof(Crew), "C")]
    [JsonDerivedType(typeof(CargoPlane), "CP")]
    [JsonDerivedType(typeof(PassengerPlane), "PP")]
    [JsonDerivedType(typeof(Passenger), "P")]
    [JsonDerivedType(typeof(Flight), "FL")]
    [JsonDerivedType(typeof(Cargo), "CA")]
    public interface IEntity
    {

    }

    public abstract class Plane : IEntity
    {
        protected Plane(ulong iD, string? serial, string? country, string? model)
        {
            ID = iD;
            Serial = serial;
            Country = country;
            Model = model;
        }

        public ulong ID { get; init; }
        public string? Serial { get; init; }
        public string? Country { get; init; }
        public string? Model { get; init; }
    }

    public class CargoPlane : Plane
    {
        public CargoPlane(ulong ID, string Serial, string Country, string Model, float MaxLoad) : base(ID, Serial, Country, Model)
        {
            this.MaxLoad = MaxLoad;
        }

         [JsonPropertyOrder(1)]
        public float MaxLoad { get; init; }
    }

    public class PassengerPlane : Plane
    {
        [JsonPropertyOrder(1)]
        public ushort FirstClassSize { get; init; }

        [JsonPropertyOrder(1)]
        public ushort BusinessClassSize { get; init; }

        [JsonPropertyOrder(1)]
        public ushort EconomyClassSize { get; init; }
        public PassengerPlane(ulong ID, string Serial, string Country, string Model, ushort FirstClassSize, ushort BusinessClassSize, ushort EconomyClassSize) : base(ID, Serial, Country, Model)
        { 
            this.FirstClassSize = FirstClassSize;
            this.EconomyClassSize = EconomyClassSize;
            this.BusinessClassSize = BusinessClassSize;
        }
    }

    public class Airport : IEntity
    {
        public ulong ID { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }
        public float AMSL { get; init; }
        public string Country { get; init; }
        public Airport(ulong ID, string Name , string Code, float Longitude, float Latitude, float AMSL, string Country)
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

    public class Flight : IEntity
    {
        public ulong ID { get; init; }
        public ulong Origin { get; init; }
        public ulong Target { get; init; }
        public string TakeoffTime { get; init; }
        public string LandingTime { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }
        public float AMSL { get; init; }
        public ulong PlaneID { get; init; }
        public ulong[] Crew { get; init; }
        public ulong[] Load { get; init; }


        public Flight(ulong iD, ulong origin, ulong target, string takeoffTime, string landingTime, float longitude, float latitude, float aMSL, ulong planeID, ulong[] crew, ulong[] load)
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

    public abstract class Person : IEntity
    {
        public ulong ID { get; init; }
        public string Name { get; init; }
        public ulong Age { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }

        protected Person(ulong iD, string name, ulong age, string phone, string email)
        {
            ID = iD;
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
        }
    }

    public class Crew : Person
    {
        [JsonPropertyOrder(1)]
        public ushort Practice { get; init; }

        [JsonPropertyOrder(1)]
        public string Role { get; init; }

        public Crew(ulong iD, string name, ulong age, string phone, string email, ushort Practice, string Role) : base(iD, name, age, phone, email)
        {
            this.Practice = Practice;
            this.Role = Role;
        }
    }

    public class Passenger : Person
    {
        [JsonPropertyOrder(1)]
        public string Class { get; init; }

        [JsonPropertyOrder(1)]
        public ulong Miles { get; init; }


        public Passenger(ulong iD, string name, ulong age, string phone, string email, string Class, ulong Miles) : base(iD, name, age, phone, email)
        {
            this.Class = Class;
            this.Miles = Miles;
        }
    }

     public class Cargo : IEntity
    {
        public ulong ID { get; init; }
        public float Weight { get; init; }
        public string Code { get; init; }
        public string Description { get; init; }

        public Cargo(ulong iD, float weight, string code, string description)
        {
            ID = iD;
            Weight = weight;
            Code = code;
            Description = description;
        }
    }
}
