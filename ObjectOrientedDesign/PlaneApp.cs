using FlightTrackerGUI;
using ObjectOrientedDesign.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public class PlaneApp
    {
        List<Flight> flights;
        ObjectParser parser;
        List<Airport> airports;
        string? path;
        string? outpath;
        string? eventpath;
        public PlaneApp()
        {
            HelperFunctions.ReadFTRPaths(out path, out outpath, out eventpath);
            parser = new TCPtoObject(path, eventpath);
            flights = parser.GenerateFlights();
            airports = parser.GenerateAirports();
            parser.OnUpdate += UpdateHandler;
        }

        public void Run()
        {
            Task task = new Task(() => { Runner.Run(); });
            task.Start();
            Task sstask = new Task(() => { this.UpdateMap(); });
            sstask.Start();
            HelperFunctions.SnapShot(parser);
        }

        public void UpdateMap()
        {
            while (true)
            {
                List<FlightGUI> list = new List<FlightGUI>();
                flights = parser.GenerateFlights();
                lock (parser.lists)
                {
                    foreach (Flight fl in flights)
                    {
                        Airport? origin = airports.Find(x => x.ID == fl.Origin);
                        Airport? dest = airports.Find(x => x.ID == fl.Target);
                        UpdateFunctions.Update(origin, dest, fl, ref list);
                    }
                }   
                FlightsGUIData fgd = new FlightsGUIData();
                fgd.UpdateFlights(list);
                Runner.UpdateGUI(fgd);
                Thread.Sleep(1000);
            }
        }
        public void UpdateHandler()
        {
            airports = parser.GenerateAirports();
            flights = parser.GenerateFlights();
        }
    }
}
