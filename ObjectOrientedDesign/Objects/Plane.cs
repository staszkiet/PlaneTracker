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

    public class CargoPlane : Plane, IReportable
    {
        public CargoPlane(ulong ID, string Serial, string Country, string Model, float MaxLoad) : base(ID, Serial, Country, Model)
        {
            this.MaxLoad = MaxLoad;
        }

        [JsonPropertyOrder(1)]
        public float MaxLoad { get; init; }

        public string Accept(IMedia visitor)
        {
            return visitor.PrintCargoPlaneInfo(this);
        }
    }

    public class PassengerPlane : Plane, IReportable
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

        public string Accept(IMedia visitor)
        {
            return visitor.PrintPassengerPlaneInfo(this);
        }
    }
}
