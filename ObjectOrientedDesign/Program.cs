using ObjectOrientedDesign;
using System.IO;
using System.Text.Json;
public class Program
{
    public static void ReadFTRPaths(out string? path, out string? outpath)
    {
        Console.WriteLine("Podaj nazwę pliku (musi znajdować się w tym samym pliku co .exe)");
        path = Console.ReadLine();
        Console.WriteLine("Podaj nazwę pliku do zapisu ");
        outpath = Console.ReadLine();
    }
    public static void Main()
    {
        string? path, outpath;
        ReadFTRPaths(out path, out outpath);
        ObjectParser parser = new FTRtoObject();
        var objects = parser.Generate(path);
        StreamWriter sw = new StreamWriter(outpath);
        ISerializer jsons = new JSONSerializer();
        foreach (string k in objects.Keys)
        {
            foreach (var e in objects[k])
            {
                jsons.SerializeToFile(sw, e);
            }
        }
        sw.Close();

    }
}