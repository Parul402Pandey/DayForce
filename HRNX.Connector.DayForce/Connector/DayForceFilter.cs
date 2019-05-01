using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Connector
{
   public class DayForceFilter
    {
        public List<string> name { get; set; }
        public List<string> value { get; set; }
        public List<string> comparisonOperator { get; set; }
        public List<string> logicalOperator { get; set; }
    }
}
