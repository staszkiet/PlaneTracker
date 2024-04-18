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
        public static void ReadFTRPaths(out string? path, out string? outpath, out string? eventpath)
        {
            Console.WriteLine("Podaj nazwę pliku (musi znajdować się w tym samym pliku co .exe)");
            path = Console.ReadLine();
            Console.WriteLine("Podaj nazwę pliku do zapisu (w przypadku źródła TCP podaj cokolwiek)");
            outpath = Console.ReadLine();
            Console.WriteLine("Podaj nazwę pliku zawierającego updaty (TCP only)");
            eventpath = Console.ReadLine();
        }

        public static void Report(ObjectParser parser)
        {
            List<IReportable> r = parser.GenerateReportables();
            List<IMedia> medias = new List<IMedia>();
            medias.Add(new Television("Telewizja Abelowa"));
            medias.Add(new Television("Kanał TV-sensor"));
            medias.Add(new Radio("Radio Kwantyfikator"));
            medias.Add(new Radio("Radio Shmem"));
            medias.Add(new Newspaper("Gazeta Kategoryczna"));
            medias.Add(new Newspaper("Dziennik Politechniczny"));
            NewsGenerator ng = new NewsGenerator(medias, r);
            lock (parser.lists)
            {
                foreach (string? msg in ng.GenerateNextNews())
                {
                    if (msg != null)
                    {
                        Console.WriteLine(msg);
                    }
                    else
                    {
                        return;
                    }
                }
            }
          
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
                else if (s == "report")
                {
                    Task task = new Task(() => Report(parser));
                    task.Start();
                }
            }
        }


    }
}
