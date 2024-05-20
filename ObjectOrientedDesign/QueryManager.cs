using ObjectOrientedDesign.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public class QueryManager
    {
        private Dictionary<string, Builder> builders = new Dictionary<string, Builder>();
        public QueryManager()
        {
            builders["display"] = new DisplayBuilder();
            builders["update"] = new UpdateBuilder();
            builders["delete"] = new DeleteBuilder();
            builders["add"] = new AddBuilder();
        }
        public void HandleQuery(string query)
        {
            query = query.Replace(",", "");
            string[] items = query.Split(' ');
            Builder concreteBuilder = builders[items[0]];
            List<string> CommandList = items.ToList();
            List<Entity> list = concreteBuilder.GetRightList(CommandList);
            concreteBuilder.ImposeRestrictions(CommandList, list);
            concreteBuilder.Ultimate(CommandList, list);
            
        }
    }
}
