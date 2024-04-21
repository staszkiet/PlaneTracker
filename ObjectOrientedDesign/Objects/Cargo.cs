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
    }
}
