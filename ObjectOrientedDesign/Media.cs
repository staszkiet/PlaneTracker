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
        public string Name { get; init; }
    }

    public class Radio : IMedia
    {
        public string Name { get; init; }
        public string PrintAirportInfo(Airport a)
        {
            return $"Reporting for {Name}. Ladies and gentelman, we are at {a.Name} airport";
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
        public Newspaper(string s)
        {
            Name = s;
        }
    }
}
