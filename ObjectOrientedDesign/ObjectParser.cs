using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using NetworkSourceSimulator;
using ObjectOrientedDesign.Objects;
using System.Numerics;
using HarfBuzzSharp;
using System.Xml;
using System.Reflection.Metadata;

namespace ObjectOrientedDesign
{
    public delegate void UpdateDel();
    public interface ObjectParser
    {
        List<Entity> Generate();
        List<Flight> GenerateFlights();
        List<Airport> GenerateAirports();
        List<IReportable> GenerateReportables();
        string outpath
        {
            get;
        }

        public event UpdateDel? OnUpdate;
    }

    public class FTRtoObject : ObjectParser
    {
        Dictionary<string, Generator> generators = new Dictionary<string, Generator>();
        public Dictionary<string, List<string[]>> strings;
        string path;
        public string outpath
        {
            get;
            init;
        }

        public event UpdateDel? OnUpdate;
        public List<Entity> Generate()
        {
            List<Entity> l = new List<Entity>();
            foreach (string p in strings.Keys)
            {
                foreach (string[] obj in strings[p])
                {
                    Entity e = generators[p].Generate(obj);
                    l.Add(e);
                }
            }
            return l;
        }

        public List<Flight> GenerateFlights()
        {
            List<Flight> l = new List<Flight>();
            FlightGenerator f = new FlightGenerator();
            foreach (string[] obj in strings["FL"])
            {
                Flight temp = f.Generate(obj);
                l.Add(temp);
            }
            return l;
        }
        public List<Airport> GenerateAirports()
        {
            List<Airport> l = new List<Airport>();
            AirportGenerator f = new AirportGenerator();
            foreach (string[] obj in strings["AI"])
            {
                Airport temp = f.Generate(obj);
                l.Add(temp);
            }
            return l;
        }

        public List<IReportable> GenerateReportables()
        {
            List<IReportable> l = new List<IReportable>();
            AirportGenerator f = new AirportGenerator();
            foreach (string[] obj in strings["AI"])
            {
                Airport temp = f.Generate(obj);
                l.Add(temp);
            }
            return l;
        }
        public FTRtoObject(string path, string outpath)
        {
            this.path = path;
            this.outpath = outpath;
            generators = new Dictionary<string, Generator>();
            generators.Add("C", new CrewGenerator());
            generators.Add("P", new PassengerGenerator());
            generators.Add("CA", new CargoGenerator());
            generators.Add("CP", new CargoPlaneGenerator());
            generators.Add("PP", new PassengerPlaneGenerator());
            generators.Add("AI", new AirportGenerator());
            generators.Add("FL", new FlightGenerator());
            strings = new Dictionary<string, List<string[]>>();
            strings.Add("C", new List<string[]>());
            strings.Add("P", new List<string[]>());
            strings.Add("CA", new List<string[]>());
            strings.Add("CP", new List<string[]>());
            strings.Add("PP", new List<string[]>());
            strings.Add("AI", new List<string[]>());
            strings.Add("FL", new List<string[]>());
            StreamReader sr = new StreamReader(path);
            string? s = sr.ReadLine();
            while (s != null)
            {
                string[] tab = s.Split(",");
                strings[tab[0]].Add(tab);
                s = sr.ReadLine();
            }
            sr.Close();

        }


    }
    public class TCPtoObject : ObjectParser
    {
        Dictionary<string, TCPGenerator> generators;
        public NetworkSourceSimulator.NetworkSourceSimulator nss;
        public Dictionary<string, List<byte[]>> bytes;
        private static Mutex mut = new Mutex();
        public event UpdateDel? OnUpdate;
        public string outpath
        {
            get
            {
                return $"snapshot_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json";
            }
        }


        public List<Entity> Generate() // z tablicy bajtów robimy listę (serializacja w mainie)
        { 
            string? ret;
            List<Entity> l = new List<Entity>();
            mut.WaitOne();
            foreach (string p in bytes.Keys)
            {
                foreach (byte[] obj in bytes[p])
                {
                    ret = System.Text.Encoding.ASCII.GetString(obj[0..3]);
                    Entity temp = generators[ret].Generate(obj[7..]);
                    l.Add(temp);
                }
            }
            mut.ReleaseMutex();
            return l;
        }

        
        public List<Flight> GenerateFlights()
        {
            List<Flight> l = new List<Flight>();
            TCPFlightGenerator f = new TCPFlightGenerator();
            mut.WaitOne();
            foreach (byte[] obj in bytes["NFL"])
            {
                Flight temp = f.Generate(obj[7..]);
                l.Add(temp);
            }
            mut.ReleaseMutex();
            return l;
        }

        public List<Airport> GenerateAirports()
        {
            List<Airport> l = new List<Airport>();
            TCPAirportGenerator f = new TCPAirportGenerator();
            mut.WaitOne();
            foreach (byte[] obj in bytes["NAI"])
            {
                Airport temp = f.Generate(obj[7..]);
                l.Add(temp);
            }
            mut.ReleaseMutex();
            return l;
        }

        public List<IReportable> GenerateReportables()
        {
            List<IReportable> l = new List<IReportable>();
            TCPAirportGenerator f = new TCPAirportGenerator();
            mut.WaitOne();
            foreach (byte[] obj in bytes["NAI"])
            {
                Airport temp = f.Generate(obj[7..]);
                l.Add(temp);
            }
            mut.ReleaseMutex();
            return l;
        }
        public TCPtoObject(string path)
        {
            generators = new Dictionary<string, TCPGenerator>();
            generators.Add("NCR", new TCPCrewGenerator());
            generators.Add("NPA", new TCPPassengerGenerator());
            generators.Add("NCA", new TCPCargoGenerator());
            generators.Add("NCP", new TCPCargoPlaneGenerator());
            generators.Add("NPP", new TCPPassengerPlaneGenerator());
            generators.Add("NAI", new TCPAirportGenerator());
            generators.Add("NFL", new TCPFlightGenerator());
            bytes = new Dictionary<string, List<byte[]>>();
            bytes.Add("NCR", new List<byte[]>());
            bytes.Add("NPA", new List<byte[]>());
            bytes.Add("NCA", new List<byte[]>());               
            bytes.Add("NCP", new List<byte[]>());
            bytes.Add("NPP", new List<byte[]>());
            bytes.Add("NAI", new List<byte[]>());
            bytes.Add("NFL", new List<byte[]>());
            nss = new NetworkSourceSimulator.NetworkSourceSimulator(path, 0, 10);
            nss.OnNewDataReady += reader;
            Task task = new Task(() => { nss.Run(); });
            task.Start();
        }
        public void reader(object sender, NewDataReadyArgs ndra)
        {
            int ind = ndra.MessageIndex;
            Message m = nss.GetMessageAt(ind);
            string ret = System.Text.Encoding.ASCII.GetString(m.MessageBytes[0..3]);
            mut.WaitOne();
            this.bytes[ret].Add(m.MessageBytes);
            mut.ReleaseMutex();
            this.OnUpdate?.Invoke();
        }
    }
}
