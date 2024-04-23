using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public static class LogWriter
    {
        private static string logpath = $"logs_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt";
        public static void Write(string s)
        {
            StreamWriter Writer = new StreamWriter(logpath, append: true);
            Writer.WriteLine(s);
            Writer.Close();
        }
    }
}
