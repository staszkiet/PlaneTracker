using ObjectOrientedDesign;
using ObjectOrientedDesign.Objects;
using System.Collections.Generic;
using System.IO;
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

    public static void ParseTCP(string path)
    {
        List<IEntity> list = new List<IEntity>();
        TCPtoObject parser = new TCPtoObject(path);
        string s = "";
        string outpath;
        while (String.Compare(s, "exit") != 0)
        {
            s = Console.ReadLine();
            if (s == "print")
            {
                outpath = $"snapshot_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json";

                list = parser.Generate();
                ISerializer jsons = new JSONSerializer();
                jsons.SerializeToFile(outpath, list);
            }
        }
    }
    public static void ParseFTR(string path, string outpath)
    {
        List<IEntity> list = new List<IEntity>();
        FTRtoObject parser = new FTRtoObject(path);

        list = parser.Generate();
        ISerializer jsons = new JSONSerializer();
        jsons.SerializeToFile(outpath, list);
    }
    public static void Main()
    {
        string? path, outpath;
        ReadFTRPaths(out path, out outpath);
        ParseTCP(path);
    }
}