using Avalonia.Controls.Shapes;
using Avalonia.Rendering;
using FlightTrackerGUI;
using ObjectOrientedDesign;
using ObjectOrientedDesign.Objects;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
public class Program
{
    public static void ReadFTRPaths(out string? path, out string? outpath)
    {
        Console.WriteLine("Podaj nazwę pliku (musi znajdować się w tym samym pliku co .exe)");
        path = Console.ReadLine();
        Console.WriteLine("Podaj nazwę pliku do zapisu (w przypadku źródła TCP podaj cokolwiek)");
        outpath = Console.ReadLine();
    }
    public static void SnapShot(ObjectParser parser)
    {
        string s = "";
        string outpath;
        List<Entity> list = new List<Entity>();
        while (String.Compare(s, "exit") != 0)
        {
            s = Console.ReadLine();
            if (s == "print")
            {
                list = parser.Generate();
                ISerializer jsons = new JSONSerializer();
                jsons.SerializeToFile(parser.outpath, list);
            }
        }
    }
    public static void UpdateMap(ObjectParser parser)
    {
        while (true)
        {
            List<FlightGUI> list = new List<FlightGUI>();
            List<Airport> airports = parser.GenerateAirports();
            List<Flight> flights = parser.GenerateFlights();
            foreach (Flight fl in flights)
            {
                Airport? origin = airports.Find(x => x.ID == fl.Origin);
                Airport? dest = airports.Find(x => x.ID == fl.Target);
                UpdateFunctions.Update(origin, dest, fl, ref list);
            }
            FlightsGUIData fgd = new FlightsGUIData();
            fgd.UpdateFlights(list);
            Runner.UpdateGUI(fgd);
            Thread.Sleep(1000);
        }
    }
    public static void Main()
    {
        string? path, outpath;
        ReadFTRPaths(out path, out outpath);
        ObjectParser parser = new FTRtoObject(path, outpath);

        Task task = new Task(() => { Runner.Run(); });
        task.Start();
        List<Airport> airports = parser.GenerateAirports();
        Task sstask = new Task(() => { UpdateMap(parser); });
        sstask.Start();

        SnapShot(parser);
    }
}