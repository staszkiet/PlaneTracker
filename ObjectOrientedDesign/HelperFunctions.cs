using FlightTrackerGUI;
using ObjectOrientedDesign.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public static class HelperFunctions
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


    }
}
