using DynamicData;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public class Cargo : Entity
    {
        public float Weight { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public override void DeleteSelf()
        {
            ListsDatabase ld = ListsDatabase.GetInstance();
            ld.cargos.Remove(this);
            ld.entities.Remove(this);
        }
        public Cargo(ulong iD, float weight, string code, string description) : base()
        {
            ID = iD;
            Weight = weight;
            Code = code;
            Description = description;
            NameToType.Add("weight", typeof(float));
            NameToType.Add("code", typeof(string));
            NameToType.Add("description", typeof(string));
            NameToUpdateFunc.Add("weight", UpdateWeight);
            NameToUpdateFunc.Add("description", UpdateDesc);
            NameToUpdateFunc.Add("code", UpdateCode);
            NameToUpdateFunc.Add("id", UpdateId);
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("weight", this.Weight.ToString());
            NameToValue.Add("code", this.Code.ToString());
            NameToValue.Add("description", this.Description.ToString());
        }

        /*public override void CreateNameToValue()
        {
            NameToValue = new Dictionary<string, string>();
            NameToValue.Add("id", this.ID.ToString());
            NameToValue.Add("weight", this.Weight.ToString());
            NameToValue.Add("code", this.Code.ToString());
            NameToValue.Add("description", this.Description.ToString());
        }*/
        public override void ChangeID(IDUpdateArgs e, ListsDatabase l)
        {
            List<Flight> fl = l.flights;
            List<Flight> affectedfl = new List<Flight>();
            affectedfl = fl.FindAll((x) => (x.Load.Contains(e.ObjectID)));
            foreach (Flight f in affectedfl)
            {
                f.Load.Replace(e.ObjectID, e.NewObjectID);
            }
            this.ID = e.NewObjectID;
        }

        public void UpdateWeight(string s)
        {
            this.Weight = float.Parse(s);
        }
        public void UpdateDesc(string s)
        {
            this.Description = s;
        }
        public void UpdateCode(string s)
        {
            this.Code = s;
        }

    }
}
