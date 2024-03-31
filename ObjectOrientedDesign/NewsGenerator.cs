using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectOrientedDesign.Objects;

namespace ObjectOrientedDesign
{
    public class NewsGenerator
    {
        List<IMedia> MediaList;
        List<IReportable> ObjectsList;
        public NewsGenerator(List<IMedia> MediaList, List<IReportable> ObjectList)
        {
            this.MediaList = MediaList;
            this.ObjectsList = ObjectList;
        }
        public IEnumerable<string?> GenerateNextNews()
        {
            foreach(IMedia media in MediaList)
            {
                foreach (IReportable reportable in ObjectsList)
                {
                    yield return reportable.Accept(media);
                }
            }
            while (true)
            {
                yield return null;
            }
        }
    }
}
