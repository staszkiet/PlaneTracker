using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NetworkSourceSimulator;
using ObjectOrientedDesign.Objects;

namespace ObjectOrientedDesign
{
    public interface ObjectParser
    {
        List<IEntity> Generate();
    }

    public class FTRtoObject : ObjectParser
    {
        Dictionary<string, IGenerator> generators = new Dictionary<string, IGenerator>();
        string path;
        public List<IEntity> Generate()
        {
            StreamReader sr = new StreamReader(path);
            List<IEntity> objects = new List<IEntity>();
            string? s = sr.ReadLine();
            while (s != null)
            {
                string[] tab = s.Split(",");
                IEntity e = generators[tab[0]].Generate(tab);
                objects.Add(e);
                s = sr.ReadLine();
            }
            return objects;
        }
        public FTRtoObject(string path)
        {
            this.path = path;
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
    public class TCPtoObject : ObjectParser
    {
        Dictionary<string, ITCPGenerator> generators;
        public NetworkSourceSimulator.NetworkSourceSimulator nss;
        public List<Byte[]> bytes;
        private static Mutex mut = new Mutex();
        Thread thread;

        CancellationTokenSource cts = new CancellationTokenSource();
        public List<IEntity> Generate() // z tablicy bajtów robimy listę (serializacja w mainie)
        {
            string? ret;
            List<IEntity> l = new List<IEntity>();
            mut.WaitOne();
            foreach (byte[] obj in bytes)
            {
                ret = System.Text.Encoding.ASCII.GetString(obj[0..3]);
                IEntity temp = generators[ret].Generate(obj[7..]);
                l.Add(temp);
            }
            mut.ReleaseMutex();
            return l;
        }
        public TCPtoObject(string path)
        {
            generators = new Dictionary<string, ITCPGenerator>();
            generators.Add("NCR", new TCPCrewGenerator());
            generators.Add("NPA", new TCPPassengerGenerator());
            generators.Add("NCA", new TCPCargoGenerator());
            generators.Add("NCP", new TCPCargoPlaneGenerator());
            generators.Add("NPP", new TCPPassengerPlaneGenerator());
            generators.Add("NAI", new TCPAirportGenerator());
            generators.Add("NFL", new TCPFlightGenerator());
            bytes = new List<byte[]>();
            nss = new NetworkSourceSimulator.NetworkSourceSimulator(path, 0, 10);
            nss.OnNewDataReady += reader;
            Task task = new Task(()=> { nss.Run(); });
            task.Start();
        }
        public void reader(object sender, NewDataReadyArgs ndra)
        {
            int ind = ndra.MessageIndex;
            Message m = nss.GetMessageAt(ind);
            mut.WaitOne();
            this.bytes.Add(m.MessageBytes);
            mut.ReleaseMutex();
        }
    }
}
