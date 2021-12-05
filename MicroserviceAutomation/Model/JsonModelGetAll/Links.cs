using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceAutomation.Model.JsonModelGetAll
{
    public class Links
    {
        public object previous { get; set; }
        public string current { get; set; }
        public string next { get; set; }
    }
}
