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

namespace ObjectOrientedDesign.Objects
{
    [JsonDerivedType(typeof(Airport), "AI")]
    [JsonDerivedType(typeof(Crew), "C")]
    [JsonDerivedType(typeof(CargoPlane), "CP")]
    [JsonDerivedType(typeof(PassengerPlane), "PP")]
    [JsonDerivedType(typeof(Passenger), "P")]
    [JsonDerivedType(typeof(Flight), "FL")]
    [JsonDerivedType(typeof(Cargo), "CA")]
    public abstract class Entity
    {

    }
}
