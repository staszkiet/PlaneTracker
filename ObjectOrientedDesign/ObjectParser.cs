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
    public abstract class ObjectParser
    {
        public abstract List<Entity> Generate();
        public abstract List<Flight> GenerateFlights();
        public abstract List<Airport> GenerateAirports();
        public abstract List<IReportable> GenerateReportables();

        public ListsDatabase lists;
        public string outpath
        {
            get;
        }

        public event UpdateDel? OnUpdate;
    }

    public class FTRtoObject : ObjectParser
    {
        Dictionary<string, Generator> generators = new Dictionary<string, Generator>();
        string path;
        public string outpath
        {
            get;
            init;
        }

        public event UpdateDel? OnUpdate;
        public override List<Entity> Generate()
        {
            return lists.entities;
        }

        public override List<Flight> GenerateFlights()
        {
            return lists.flights;
        }
        public override List<Airport> GenerateAirports()
        {
            return lists.airports;
        }

        public List<CargoPlane> GenerateCargoPlanes()
        {
            return lists.cargoPlanes;
        }

        public List<PassengerPlane> GeneratePassengerPlanes()
        {
            return lists.passengerPlanes;
        }

        public override List<IReportable> GenerateReportables()
        {
            List<IReportable> l = new List<IReportable>();
            List<Airport> a = GenerateAirports();
            l.AddRange(a);
            l.AddRange(GenerateCargoPlanes());
            l.AddRange(GeneratePassengerPlanes());
            return l;
        }
        public FTRtoObject(string path, string outpath)
        {
            lists = new ListsDatabase();
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
            StreamReader sr = new StreamReader(path);
            string? s = sr.ReadLine();
            while (s != null)
            {
                string[] tab = s.Split(",");
                generators[tab[0]].Generate(tab, ref lists);
                s = sr.ReadLine();
            }
            sr.Close();

        }


    }
    public class TCPtoObject : ObjectParser
    {
        Dictionary<string, TCPGenerator> generators;
        public NetworkSourceSimulator.NetworkSourceSimulator nss;
        public event UpdateDel? OnUpdate;
        public string outpath
        {
            get
            {
                return $"snapshot_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json";
            }
        }


        public override List<Entity> Generate() // z tablicy bajtów robimy listę (serializacja w mainie)
        { 
            return lists.entities;
        }

        
        public override List<Flight> GenerateFlights()
        {
            List<Flight> l = new List<Flight>();
            l = lists.flights;
            return l;
        }

        public override List<Airport> GenerateAirports()
        {
            List<Airport> l = new List<Airport>();
            l = lists.airports;
            return l;
        }

        public List<CargoPlane> GenerateCargoPlanes()
        {
            List<CargoPlane> l = new List<CargoPlane>();
            l = lists.cargoPlanes;
            return l;
        }

        public List<PassengerPlane> GeneratePassengerPlanes()
        {
            List<PassengerPlane> l = new List<PassengerPlane>();
            l = lists.passengerPlanes;
            return l;
        }
        public override List<IReportable> GenerateReportables()
        {
            List<IReportable> l = new List<IReportable>();
            TCPAirportGenerator f = new TCPAirportGenerator();
            List<Airport> a = GenerateAirports();
            l.AddRange(a);
            l.AddRange(GenerateCargoPlanes());
            l.AddRange(GeneratePassengerPlanes());
            return l;
        }
        public TCPtoObject(string path, string eventpath)
        {
            lists = new ListsDatabase();
            generators = new Dictionary<string, TCPGenerator>();
            generators.Add("NCR", new TCPCrewGenerator());
            generators.Add("NPA", new TCPPassengerGenerator());
            generators.Add("NCA", new TCPCargoGenerator());
            generators.Add("NCP", new TCPCargoPlaneGenerator());
            generators.Add("NPP", new TCPPassengerPlaneGenerator());
            generators.Add("NAI", new TCPAirportGenerator());
            generators.Add("NFL", new TCPFlightGenerator());
            nss = new NetworkSourceSimulator.NetworkSourceSimulator(path, 0, 10);
            nss.OnNewDataReady += reader;
            TaskStarter(eventpath);
        }

        public async Task TaskStarter(string eventpath)
        {
            await Task.Run(() => { nss.Run(); });
            nss = new NetworkSourceSimulator.NetworkSourceSimulator(eventpath, 0, 10);
            nss.OnIDUpdate += UpdateID;
            nss.OnPositionUpdate += UpdatePosition;
            nss.OnContactInfoUpdate += UpdateContactInfo;
            await Task.Run(() => { nss.Run(); });
        }
        public void reader(object sender, NewDataReadyArgs ndra)
        {
            int ind = ndra.MessageIndex;
            Message m = nss.GetMessageAt(ind);
            string ret = System.Text.Encoding.ASCII.GetString(m.MessageBytes[0..3]);
            generators[ret].Generate(m.MessageBytes[7..], ref lists);
            this.OnUpdate?.Invoke();
        }

        public void UpdateID(object sender, IDUpdateArgs e)
        {
            lock (lists)
            {
                List<Entity> entities = this.Generate();
                Entity? found = entities.Find((x) => x.ID == e.ObjectID);
                if(found != null)
                {
                    found.ID = e.NewObjectID;
                }
            }
        }
        public void UpdatePosition(object sender, PositionUpdateArgs e)
        {
            lock (lists)
            {
                List<Flight> flights = lists.flights;
                Flight? found = flights.Find((x) => x.ID == e.ObjectID);
                if (found != null)
                {
                    found.AMSL = e.AMSL;
                    found.Latitude = e.Latitude;
                    found.Longitude = e.Longitude;
                    return;
                }
                List<Airport> airports = lists.airports;
                Airport? foundairport = airports.Find((x) => x.ID == e.ObjectID);
                if (foundairport != null)
                {
                    foundairport.AMSL = e.AMSL;
                    foundairport.Latitude = e.Latitude;
                    foundairport.Longitude = e.Longitude;
                    return;
                }
            }
        }

        public void UpdateContactInfo(object sender, ContactInfoUpdateArgs e)
        {
            lock (lists)
            {
                List<Person> people = new List<Person>();
                people.AddRange(lists.passengers);
                people.AddRange(lists.crews);
                Person? p = people.Find((x) => x.ID == e.ObjectID);
                if (p != null)
                {
                    p.Email = e.EmailAddress;
                    p.Phone = e.PhoneNumber;
                }
            }
        }
    }
}
