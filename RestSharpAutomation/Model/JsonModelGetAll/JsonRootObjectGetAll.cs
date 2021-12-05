using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceAutomation.Model.JsonModelGetAll
{
    public class JsonRootObjectGetAll
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }
    }
}
