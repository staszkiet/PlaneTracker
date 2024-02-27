using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace projektowanie
{
    public interface ISerializer
    {
        void SerializeToFile(StreamWriter sw, object obj);
    }

    public class JSONSerializer : ISerializer
    {
        public void SerializeToFile(StreamWriter sw, object obj)
        {
            string help;
            help = JsonSerializer.Serialize(obj);
            sw.Write(help);
        }
    }
}
