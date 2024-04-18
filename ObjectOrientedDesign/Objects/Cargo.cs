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
    }
}
