using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace projektowanie
{
    public interface ObjectParser
    {
        Dictionary<string, List<IEntity>> Generate(string path);
    }

    public class FTRtoObject : ObjectParser
    {
        Dictionary<string, IGenerator> generators = new Dictionary<string, IGenerator>();
        public Dictionary<string, List<IEntity>> Generate(string path)
        {
            Dictionary<string, List<IEntity>> objects = new Dictionary<string, List<IEntity>>();
            objects.Add("C", new List<IEntity>());
            objects.Add("P", new List<IEntity>());
            objects.Add("CA", new List<IEntity>());
            objects.Add("CP", new List<IEntity>());
            objects.Add("PP", new List<IEntity>());
            objects.Add("AI", new List<IEntity>());
            objects.Add("FL", new List<IEntity>());
            StreamReader sr = new StreamReader(path);
            string? s = sr.ReadLine();
            while (s != null)
            {
                string[] tab = s.Split(",");
                IEntity e = generators[tab[0]].Generate(tab);
                objects[tab[0]].Add(e);
                s = sr.ReadLine();
            }
            return objects;
        }
        public FTRtoObject()
        {
            generators = new Dictionary<string, IGenerator>();
            generators.Add("C", new CrewGenerator());
            generators.Add("P", new PassengerGenerator());
            generators.Add("CA", new CargoGenerator());
            generators.Add("CP", new CargoPlaneGenerator());
            generators.Add("PP", new PassengerPlaneGenerator());
            generators.Add("AI", new AirportGenerator());
            generators.Add("FL", new FlightGenerator());

        }


    }
}
