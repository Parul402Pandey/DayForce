using HRNX.Connector.DayForce.Entities;
using HRNX.Connector.DayForce.Utils;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Connector
{
   public class QueryProcessor
    {
        private readonly IDictionary<string, string> _connectionInfo;
        private DayForceClient _dayforceClient = new DayForceClient();
        public QueryProcessor(IDictionary<string, string> connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }
        public IEnumerable<DataEntity> ExecuteQuery(Query query)
        {
            IEnumerable<DataEntity> results = null;
            string entityName = query.RootEntity.ObjectDefinitionFullName;
            DayForceFilter filter = null;
            if (query.Constraints != null)
            {
                filter = ScribeUtils.CreateFilter(query);
            }
          

            switch (entityName)
            {
               
                
                case ConstantUtils.Employee_Entity:
                        List<Employees> employeeEntity = new List<Employees>();
                        employeeEntity = _dayforceClient.GetEmployees(_connectionInfo, filter);
                        results = ScribeUtils.ToDataEntities<Employees>(employeeEntity);
                    
                    break;
                case ConstantUtils.EmployeeDetails_Entity:
                    List<EmployeeDetails> employeeDetailsEntity = new List<EmployeeDetails>();
                    employeeDetailsEntity = _dayforceClient.GetEmployeeDetails(_connectionInfo, filter);
                    results = ScribeUtils.ToDataEntities<EmployeeDetails>(employeeDetailsEntity);

                    break;

                default:
                    break;
            }

            return results;

        }
    }
}
