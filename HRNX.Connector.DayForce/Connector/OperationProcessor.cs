using HRNX.Connector.DayForce.Entities;
using HRNX.Connector.DayForce.Utils;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Connector
{
   public  class OperationProcessor
    {
        private readonly IDictionary<string, string> _connectionInfo;
        private DayForceClient _dayforceClient = new DayForceClient();
        public OperationProcessor(IDictionary<string, string> connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }
        internal OperationResult ExecuteOperation(OperationInput operationInput)
        {
            var operationResult = new OperationResult();
            // arrays to keep track of the operations statuses
            var operationSuccess = new List<bool>();
            var entitiesAffected = new List<int>();
            var errorList = new List<ErrorResult>();
            var entities = new List<DataEntity>();

            switch (operationInput.Name)
            {
                case "Create":
                    foreach (var scribeEntity in operationInput.Input)
                    {
                        var returnEntity = new DataEntity();
                        try
                        {
                           

                             if (scribeEntity.ObjectDefinitionFullName == Utils.ConstantUtils.EmployeeCreateFlat_Entity)
                            {
                                var candidate = ScribeUtils.EntityToObject<EmployeeCreateFlat>(scribeEntity);
                                _dayforceClient.CreateEmployee(_connectionInfo, candidate);
                             //   List<EmployeeCreateFlat> lst = new List<EmployeeCreateFlat>();
                             //   lst.Add(_dayforceClient.responseObject);
                             //   IEnumerable<EmployeeCreateFlat> enumerable = lst;
                                //   IEnumerable<CandidateResponse> enumerable =new[] { _greenhouseClient.responseObject };

                              //  var data = ScribeUtils.ToDataEntity<EmployeeCreateFlat>(enumerable, "Candidates");
                              //  returnEntity = data.FirstOrDefault();


                            }

                            operationSuccess.Add(true);
                            errorList.Add(null);
                            entitiesAffected.Add(1);
                            entities.Add(returnEntity);
                        }
                        catch (Exception ex)
                        {
                            //add an error to our collections: 
                            var errorResult = new ErrorResult
                            {
                                Description = string.Format("An error returned from DayForce in Create Action: {0}", ex.Message),
                                Detail = ex.StackTrace,
                            };
                            operationSuccess.Add(false);
                            errorList.Add(errorResult);
                            entitiesAffected.Add(0);
                            entities.Add(null);

                        }
                    }
                    break;
               
            }

            //Completed the requests, hand back the results: 
            operationResult.Success = operationSuccess.ToArray();
            operationResult.ObjectsAffected = entitiesAffected.ToArray();
            operationResult.ErrorInfo = errorList.ToArray();
            operationResult.Output = entities.ToArray();

            return operationResult;
        }
    }
}
