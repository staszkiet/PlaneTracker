using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
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
}
