using Mapsui.Providers.Wfs.Utilities;
using NetworkSourceSimulator;
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
        public ulong ID;
        public virtual void ChangeID(IDUpdateArgs e, ListsDatabase l)
        {
            this.ID = e.NewObjectID;
        }

        public abstract void DeleteSelf();

        public override string ToString()
        {
            string ret = "{";
            foreach (string Key in NameToValue.Keys)
            {
                ret += NameToValue[Key];
                ret += ", ";
            }
            ret = ret[0..^2];
            ret += "}";
            return ret;
        }
        public Dictionary<string, string> NameToType;
        public Dictionary<string, string> NameToValue;
        public Dictionary<string, Func<string, IComparable>> TypeToParse;
        public Dictionary<string, Func<IComparable, IComparable, bool>> SignToFunc;
        public Dictionary<string, object> NameToObject;
        public Dictionary<string, Action<string>> NameToUpdateFunc;

        public virtual void Update(string what, string how)
        {
            Action<string> f = NameToUpdateFunc[what];
            f(how);
            NameToValue[what] = how;
        }
        public bool Less(IComparable a, IComparable b)
        {
            int ret = a.CompareTo(b);
            if (ret < 0)
            {
                return true;
            }
            return false;
        }
        public bool Equal(IComparable a, IComparable b)
        {
            int ret = a.CompareTo(b);
            if (ret == 0)
            {
                return true;
            }
            return false;
        }
        public bool LessOrEqual(IComparable a, IComparable b)
        {
            int ret = a.CompareTo(b);
            if (ret <= 0)
            {
                return true;
            }
            return false;
        }
        public bool More(IComparable a, IComparable b)
        {
            int ret = a.CompareTo(b);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
        public bool MoreOrEqual(IComparable a, IComparable b)
        {
            int ret = a.CompareTo(b);
            if (ret >= 0)
            {
                return true;
            }
            return false;
        }

        public IComparable UlongParse(string a)
        {
            IComparable aparsed = ulong.Parse(a);
            return aparsed;
        }

        public IComparable UshortParse(string a)
        { 
            IComparable aparsed = ushort.Parse(a);
            return aparsed;
        }

        public IComparable FloatParse(string a)
        {
            IComparable aparsed = float.Parse(a);
            return aparsed;
        }

        public IComparable StringParse(string a)
        {
            return a;
        }

        public virtual void UpdateId(string s)
        {
            this.ID = ulong.Parse(s);
        }
        public Entity()
        {
            SignToFunc = new Dictionary<string, Func<IComparable, IComparable, bool>>();
            SignToFunc.Add("<", Less);
            SignToFunc.Add(">", More);
            SignToFunc.Add("=", Equal);
            SignToFunc.Add(">=", MoreOrEqual);
            SignToFunc.Add("<=", LessOrEqual);
            NameToType = new Dictionary<string, string>();
            NameToType.Add("id", "ulong");
            TypeToParse = new Dictionary<string, Func<string, IComparable>>();
            TypeToParse.Add("ulong", UlongParse);
            TypeToParse.Add("ushort", UshortParse);
            TypeToParse.Add("float", FloatParse);
            TypeToParse.Add("string", StringParse);
            NameToUpdateFunc = new Dictionary<string, Action<string>>();
        }
    }

    public interface IReportable
    {
        public string? Accept(IMedia v);
    }
}
