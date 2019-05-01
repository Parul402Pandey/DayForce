using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRNX.Connector.DayForce.Connector.MetaDataProvider;

namespace HRNX.Connector.DayForce.Entities
{
   public  class Employees
    {

        [QueryConstraintAttribute]
        public string XRefCode { get; set; }
        // attribute show in filter but not in preview
        [QuerySelectAttribute]
        public string employeeNumber { get; set;}
        [QuerySelectAttribute]
        public string employmentStatusXRefcode { get; set; }
        [QuerySelectAttribute]
        public string orgUnitXRefCode { get; set; }
        [QuerySelectAttribute]
        public DateTime filterHireStartDate { get; set; }
        [QuerySelectAttribute]
        public DateTime filterHireEndDate { get; set; }
        [QuerySelectAttribute]
        public DateTime filterTerminationStartDate { get; set; }
        [QuerySelectAttribute]
        public DateTime filterTerminationEndDate { get; set; }
        [QuerySelectAttribute]
        public DateTime filterUpdatedStartDate { get; set; }
        [QuerySelectAttribute]
        public DateTime filterUpdatedEndDate { get; set; }
        [QuerySelectAttribute]
        public DateTime contextDate { get; set; }
      

    }
}
