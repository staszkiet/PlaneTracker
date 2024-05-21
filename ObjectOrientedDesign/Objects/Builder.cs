using BruTile.Wmts.Generated;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign.Objects
{
    public abstract class Builder
    {
        public abstract List<Entity> GetRightList(List<string> cmd);
        public abstract List<Entity> ImposeRestrictions(List<string> cmd, List<Entity> list);
        public abstract void Ultimate(List<string> cmd, List<Entity> list);

        public Dictionary<string, List<Entity>> lists;

        public Dictionary<string, Generator> generators;

        public ListsDatabase db;
        public Builder()
        {
            lists = new Dictionary<string, List<Entity>>();
            db = ListsDatabase.GetInstance();
            List<Entity> pom = new List<Entity>();
            pom.AddRange(db.airports);
            lists.Add("airports", pom);
            pom = new List<Entity>();
            pom.AddRange(db.cargos);
            lists.Add("cargos", pom);
            pom = new List<Entity>();
            pom.AddRange(db.crews);
            lists.Add("crews", pom);
            pom = new List<Entity>();
            pom.AddRange(db.flights);
            lists.Add("flights", pom);
            pom = new List<Entity>();
            pom.AddRange(db.cargoPlanes);
            lists.Add("cargoplanes", pom);
            pom = new List<Entity>();
            pom.AddRange(db.passengerPlanes);
            lists.Add("passengerplanes", pom);
            pom = new List<Entity>();
            pom.AddRange(db.passengers);
            lists.Add("passengers", pom);
            generators = new Dictionary<string, Generator>();
            generators.Add("crews", new CrewGenerator());
            generators.Add("passengers", new PassengerGenerator());
            generators.Add("cargos", new CargoGenerator());
            generators.Add("cargoplanes", new CargoPlaneGenerator());
            generators.Add("passengerplanes", new PassengerPlaneGenerator());
            generators.Add("airports", new AirportGenerator());
            generators.Add("flights", new FlightGenerator());
        }
    }

    public class DisplayBuilder : Builder
    {
        public override List<Objects.Entity> GetRightList(List<string> CommandList)
        {
            while (CommandList[0] != "from")
            {
                CommandList.Add(CommandList[0]);
                CommandList.RemoveAt(0);
            }
            List<Entity> ret = lists[CommandList[1]];
            CommandList.RemoveAt(0);
            CommandList.RemoveAt(0);
            return ret;
        }

        public override List<Entity> ImposeRestrictions(List<string> cmd, List<Entity> list)
        {
            if (cmd[0] == "display") 
            {
                return list;
            }
            bool[] if_satisfy_ands = new bool[list.Count];
            bool[] if_satisfy = new bool[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                if_satisfy_ands[i] = true;
                if_satisfy[i] = false;
            }
            while (true)
            {
                cmd.RemoveAt(0);
                do
                {
                    if (cmd[0] == "and")
                    {
                        cmd.RemoveAt(0);
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        string val = list[i].NameToValue[cmd[0]];
                        string t = list[i].NameToType[cmd[0]];
                        Func<string, IComparable> f = list[i].TypeToParse[t];
                        IComparable v1 = f(val);
                        IComparable v2 = f(cmd[2]);
                        Func<IComparable, IComparable, bool> compare = list[i].SignToFunc[cmd[1]];
                        if_satisfy_ands[i] &= compare(v1, v2);
                    }
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                }
                while (!(cmd[0] == "or") && !(cmd[0] == "display"));
                for (int i = 0; i < list.Count; i++)
                {
                    if_satisfy[i] |= if_satisfy_ands[i];
                    if_satisfy_ands[i] = true;
                }
                if (cmd[0] == "display")
                {
                    break;
                }
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (!if_satisfy[i])
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
        public override void Ultimate(List<string> cmd, List<Entity> list)
        {
            cmd.RemoveAt(0);
            Dictionary<string, List<string>> table = new Dictionary<string, List<string>>();
            Dictionary<string, int> lengths = new Dictionary<string, int>();
            if (cmd[0] != "*")
            {
                for (int i = 0; i < cmd.Count; i++)
                {
                    table.Add(cmd[i], new List<string>());
                }
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < cmd.Count; j++)
                    {
                        table[cmd[j]].Add(list[i].NameToValue[cmd[j]]);
                    }
                }
            }
            else if(list.Count > 0)
            {
                foreach (string Key in list[0].NameToValue.Keys)
                {
                    if (!Key.Contains('.'))
                    {
                        table.Add(Key, new List<string>());
                    } 
                }
                for (int i = 0; i < list.Count; i++)
                {
                    foreach (string Key in list[0].NameToValue.Keys)
                    {
                        if (!Key.Contains("."))
                        {
                            table[Key].Add(list[i].NameToValue[Key]);
                        }  
                    }
                }
            }

            foreach (string key in table.Keys)
            {
                int max = 0;
                for (int i = 0; i < table[key].Count; i++)
                {
                    if (table[key][i].Length > max)
                    {
                        max = table[key][i].Length;
                    }
                }
                if (key.Length > max)
                {
                    max = key.Length;
                }
                lengths.Add(key, max);
            }
            string s = " ";
            foreach (string key in table.Keys)
            {
                s += String.Format(" {0, -" + lengths[key].ToString() + "} |", key);
            }
            Console.WriteLine(s);
            s = " ";
            foreach (string key in table.Keys)
            {
                s += "-";
                for (int i = 0; i < lengths[key]; i++)
                {
                    s += "-";
                }
                s += "-";
                s += "+";
            }
            Console.WriteLine(s);
            for (int i = 0; i < list.Count; i++)
            {
                s = " ";
                foreach (string key in table.Keys)
                {
                    s += String.Format(" {0, " + lengths[key].ToString() + "} |", table[key][i]);
                }
                Console.WriteLine(s);
            }
        }
    }

    public class UpdateBuilder : Builder
    {
        public override List<Entity> GetRightList(List<string> CommandList)
        {
            CommandList.RemoveAt(0);
            List<Entity> ret = lists[CommandList[0]];
            CommandList.RemoveAt(0);
            if (CommandList.Contains("where"))
            {
                while (CommandList[0] != "where")
                {
                    CommandList.Add(CommandList[0]);
                    CommandList.RemoveAt(0);
                }
            }
            return ret;
        }

        public override List<Objects.Entity> ImposeRestrictions(List<string> cmd, List<Entity> list)
        {
            bool[] if_satisfy_ands = new bool[list.Count];
            bool[] if_satisfy = new bool[list.Count];
            if (cmd[0] != "where")
            {
                return list;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if_satisfy_ands[i] = true;
                if_satisfy[i] = false;
            }
            while (true)
            {
                cmd.RemoveAt(0);
                do
                {
                    if (cmd[0] == "and")
                    {
                        cmd.RemoveAt(0);
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        string val = list[i].NameToValue[cmd[0]];
                        string t = list[i].NameToType[cmd[0]];
                        Func<string, IComparable> f = list[i].TypeToParse[t];
                        IComparable v1 = f(val);
                        IComparable v2 = f(cmd[2]);
                        Func<IComparable, IComparable, bool> compare = list[i].SignToFunc[cmd[1]];
                        if_satisfy_ands[i] &= compare(v1, v2);
                    }
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                }
                while (!(cmd[0] == "or") && !(cmd[0] == "set"));
                for (int i = 0; i < list.Count; i++)
                {
                    if_satisfy[i] |= if_satisfy_ands[i];
                    if_satisfy_ands[i] = true;
                }
                if (cmd[0] == "set")
                {
                    break;
                }
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (!if_satisfy[i])
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
        public override void Ultimate(List<string> cmd, List<Entity> list) // w kazdej klasie metody pozwalające na dostęp do jakiegoś elementu i zmiane go
        {
            cmd.RemoveAt(0);
            while (cmd.Count > 0)
            {
                cmd[0] = cmd[0].Replace("(", "");
                cmd[0] = cmd[0].Replace(")", "");
                int eq = cmd[0].IndexOf('=');
                cmd[0] = cmd[0].Replace(",", "");
                string first = cmd[0].Substring(0, eq);
                string last = cmd[0].Substring(eq + 1);
                last = last.Replace(".", ",");
                cmd.RemoveAt(0);
                while (cmd.Count > 0 && !cmd[0].Contains('='))
                {
                    cmd[0] = cmd[0].Replace(")", "");
                    last += (" " + cmd[0]);
                    cmd.RemoveAt(0);
                }

                foreach (Entity ent in list)
                {
                    ent.Update(first, last);
                }
               
            }
        }
    }

    public class DeleteBuilder : Builder
    {
        public override List<Entity> GetRightList(List<string> CommandList)
        {
            CommandList.RemoveAt(0);
            List<Entity> ret = lists[CommandList[0]];
            //CommandList.Add(CommandList[0]);
            CommandList.RemoveAt(0);
            return ret;
        }

        public override List<Objects.Entity> ImposeRestrictions(List<string> cmd, List<Entity> list)
        {
            bool[] if_satisfy_ands = new bool[list.Count];
            bool[] if_satisfy = new bool[list.Count];
            if (cmd.Count == 0)
            {
                return list;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if_satisfy_ands[i] = true;
                if_satisfy[i] = false;
            }
            while (true)
            {
                cmd.RemoveAt(0);
                do
                {
                    if (cmd[0] == "and")
                    {
                        cmd.RemoveAt(0);
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        string val = list[i].NameToValue[cmd[0]];
                        string t = list[i].NameToType[cmd[0]];
                        Func<string, IComparable> f = list[i].TypeToParse[t];
                        IComparable v1 = f(val);
                        IComparable v2 = f(cmd[2]);
                        Func<IComparable, IComparable, bool> compare = list[i].SignToFunc[cmd[1]];
                        if_satisfy_ands[i] &= compare(v1, v2);
                    }
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                    cmd.RemoveAt(0);
                }
                while (cmd.Count > 1 && !(cmd[0] == "or"));
                for (int i = 0; i < list.Count; i++)
                {
                    if_satisfy[i] |= if_satisfy_ands[i];
                    if_satisfy_ands[i] = true;
                }
                if (cmd.Count == 1)
                {
                    break;
                }
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (!if_satisfy[i])
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
        public override void Ultimate(List<string> cmd, List<Entity> list)
        {
            foreach (Entity e in list)
            {
                e.DeleteSelf();
            }
        }
    }

    public class AddBuilder : Builder
    {
        public override List<Entity> GetRightList(List<string> CommandList)
        {
            CommandList.RemoveAt(0);
            List<Entity> ret = lists[CommandList[0]];
            return ret;
        }

        public override List<Entity> ImposeRestrictions(List<string> cmd, List<Entity> list)
        {
            return list;
        }
        public override void Ultimate(List<string> cmd, List<Entity> list)
        {
            Generator g = generators[cmd[0]];
            string[] tab = new string[100];
            for (int i = 0; i < 100; i++)
            {
                tab[i] = "0";
            }
            cmd.RemoveAt(0);
            cmd.RemoveAt(0);
            while (cmd.Count > 0)
            {
                cmd[0] = cmd[0].Replace("(", "");
                cmd[0] = cmd[0].Replace(")", "");
                int eq = cmd[0].IndexOf('=');
                string first = cmd[0].Substring(0, eq);
                string last = cmd[0].Substring(eq + 1);
                cmd.RemoveAt(0);
                while (cmd.Count > 0 && !cmd[0].Contains('='))
                {
                    cmd[0] = cmd[0].Replace(")", "");
                    last += (" " + cmd[0]);
                    cmd.RemoveAt(0);
                }
                tab[g.NameToIndex[first]] = last;
            }
            g.Generate(tab, ref db);
        }
    }
}
