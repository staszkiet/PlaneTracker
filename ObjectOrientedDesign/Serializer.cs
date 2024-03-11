using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public interface ISerializer
    {
        void SerializeToFile(string outpath, object obj);
    }

    public class JSONSerializer : ISerializer
    {
        public void SerializeToFile(string outpath, object obj)
        {
            StreamWriter sw = new StreamWriter(outpath);
            string help;
            help = JsonSerializer.Serialize(obj);
            sw.Write(help);
            sw.Close();
        }
    }
}
