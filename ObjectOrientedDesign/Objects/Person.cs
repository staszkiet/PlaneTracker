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
    public abstract class Person : Entity
    {
        public string Name { get; private set; }
        public ulong Age { get; private set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public void UpdateName(string s)
        {
            this.Name = s;
        }
        public void UpdateAge(string s)
        {
            this.Age = ulong.Parse(s);
        }
        public void UpdatePhone(string s)
        {
            this.Phone = s;
        }
        public void UpdateEmail(string s)
        {
            this.Email = s;
        }

        protected Person(ulong iD, string name, ulong age, string phone, string email): base()
        {
            ID = iD;
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
            NameToType.Add("name", "string");
            NameToType.Add("age", "ulong");
            NameToType.Add("phone", "string");
            NameToType.Add("email", "string");
            NameToUpdateFunc.Add("name", UpdateName);
            NameToUpdateFunc.Add("age", UpdateAge);
            NameToUpdateFunc.Add("phone", UpdatePhone);
            NameToUpdateFunc.Add("email", UpdateEmail);
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("name", this.Name.ToString());
            NameToValue.Add("age", this.Age.ToString());
            NameToValue.Add("phone", this.Phone.ToString());
            NameToValue.Add("email", this.Email.ToString());
        }
    }

    public class Crew : Person
    {
        [JsonPropertyOrder(1)]
        public ushort Practice { get; private set; }

        [JsonPropertyOrder(1)]
        public string Role { get; private set; }

        public void UpdateRole(string s)
        {
            this.Role = s;
        }
        public void UpdatePracitce(string s)
        {
            this.Practice = ushort.Parse(s);
        }


        public Crew(ulong iD, string name, ulong age, string phone, string email, ushort Practice, string Role) : base(iD, name, age, phone, email)
        {
            this.Practice = Practice;
            this.Role = Role;
            NameToType.Add("practise", "ushort");
            NameToType.Add("role", "string");
            NameToUpdateFunc.Add("practise", UpdatePracitce);
            NameToUpdateFunc.Add("role", UpdateRole);
            NameToUpdateFunc.Add("id", UpdateId);
            NameToValue.Add("practise", this.Practice.ToString());
            NameToValue.Add("role", this.Role.ToString());
        }
        public override void ChangeID(IDUpdateArgs e, ListsDatabase l)
        {
            List<Flight> fl = l.flights;
            List<Flight> affectedfl = new List<Flight>();
            affectedfl = fl.FindAll((x) => (x.Crew.Contains(e.ObjectID)));
            foreach (Flight f in affectedfl)
            {
                f.Crew.Replace(e.ObjectID, e.NewObjectID);
            }
            this.ID = e.NewObjectID;
        }

        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.crews.Remove(this);
            ld.entities.Remove(this);
        }
        /*public override void CreateNameToValue()
        {
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("name", this.Name.ToString());
            NameToValue.Add("age", this.Age.ToString());
            NameToValue.Add("phone", this.Phone.ToString());
            NameToValue.Add("email", this.Email.ToString());
            NameToValue.Add("practise", this.Practice.ToString());
            NameToValue.Add("role", this.Role.ToString());
        }*/
    }

    public class Passenger : Person
    {
        [JsonPropertyOrder(1)]
        public string Class { get; private set; }

        [JsonPropertyOrder(1)]
        public ulong Miles { get; private set; }

        public Passenger(ulong iD, string name, ulong age, string phone, string email, string Class, ulong Miles) : base(iD, name, age, phone, email)
        {
            this.Class = Class;
            this.Miles = Miles;
            NameToType.Add("class", "string");
            NameToType.Add("miles", "ulong");
            NameToUpdateFunc.Add("class", UpdateClass);
            NameToUpdateFunc.Add("miles", UpdateMiles);
            NameToUpdateFunc.Add("id", UpdateId);
            NameToValue.Add("class", this.Class.ToString());
            NameToValue.Add("Miles", this.Miles.ToString());
        }

        public void UpdateClass(string s)
        {
            this.Class = s;
        }
        public void UpdateMiles(string s)
        {
            this.Miles = ulong.Parse(s);
        }
        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.passengers.Remove(this);
            ld.entities.Remove(this);
        }
       /* public override void CreateNameToValue()
        {
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("name", this.Name.ToString());
            NameToValue.Add("age", this.Age.ToString());
            NameToValue.Add("phone", this.Phone.ToString());
            NameToValue.Add("email", this.Email.ToString());
            NameToValue.Add("class", this.Class.ToString());
            NameToValue.Add("Miles", this.Miles.ToString());
        }*/
    }
}
