// See https://aka.ms/new-console-template for more information
using projektowanie;
using System.Text.Json;
public class Program
{
    public static void Main()
    { 
        ObjectParser parser = new FTRtoObject();
        Console.WriteLine("Podaj nazwę pliku (musi znajdować się w tym samym pliku co .exe)");
        string? path;
        path = Console.ReadLine();
        string? outpath;
        var objects = parser.Generate(path);
        Console.WriteLine("Podaj nazwę pliku do zapisu ");
        outpath = Console.ReadLine();
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