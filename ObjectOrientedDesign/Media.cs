using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectOrientedDesign.Objects;

namespace ObjectOrientedDesign
{
    public interface IMedia
    {
        public string PrintAirportInfo(Airport aiport);

        public string PrintCargoPlaneInfo(CargoPlane c);

        public string PrintPassengerPlaneInfo(PassengerPlane p);
        public string Name { get; init; }
    }

    public class Radio : IMedia
    {
        public string Name { get; init; }
        public string PrintAirportInfo(Airport a)
        {
            return $"Reporting for {Name}. Ladies and gentelman, we are at {a.Name} airport";
        }

        public string PrintCargoPlaneInfo(CargoPlane c)
        {
            return $"Reporting for {Name},Ladies and gentelmen, we are seeing the {c.Serial} aircraft fly above us.";
        }

        public string PrintPassengerPlaneInfo(PassengerPlane p)
        {
            return $"Reporting for {Name},Ladies and gentelmen, we’ve just witnessed {p.Serial} take off.";
        }
        public Radio(string s) 
        {
            Name = s;
        }
    }
    public class Television :IMedia
    {
        public string Name { get; init; }
        public string PrintAirportInfo(Airport a)
        {
            return $"An image of {a.Name} airport";
        }
        public string PrintCargoPlaneInfo(CargoPlane c)
        { 
            return $"An image of {c.Model} cargo plane";
        }
        public string PrintPassengerPlaneInfo(PassengerPlane p)
        {
            return $"An image of {p.Model} passenger plane";
        }

        public Television(string s)
        {
            Name = s;
        }
    }
    public class Newspaper : IMedia
    {
        public string Name { get; init; }
        public string PrintAirportInfo(Airport a)
        {
            return $"{Name} - A report from the {a.Name} airport {a.Country}";
        }
        public string PrintCargoPlaneInfo(CargoPlane c)
        {
            return $"{Name} - An interview with the crew of {c.Serial}.";
        }
        public string PrintPassengerPlaneInfo(PassengerPlane p)
        {
            return $"{Name} -Breaking news! {p.Model} aircraft loses EASA fails certification after inspection of {p.Serial}";
        }
        public Newspaper(string s)
        {
            Name = s;
        }
    }
}
