using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Entities
{
    public class EmployeeResponse
    {
        public List<Datum> Data { get; set; }
    }

    public class Datum
    {
        public string XRefCode { get; set; }
    }

}
