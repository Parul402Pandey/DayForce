using HRNX.Connector.DayForce.Utils;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Actions;
using Scribe.Core.ConnectorApi.ConnectionUI;
using Scribe.Core.ConnectorApi.Exceptions;
using Scribe.Core.ConnectorApi.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRNX.Connector.DayForce.Connector;

namespace HRNX.Connector.DayForce.Connector
{

    [ScribeConnector(
   ConnectorSettings.ConnectorTypeId,
   ConnectorSettings.Name,
   ConnectorSettings.Description,
   typeof(Connector),
   StandardConnectorSettings.SettingsUITypeName,
   StandardConnectorSettings.SettingsUIVersion,
   StandardConnectorSettings.ConnectionUITypeName,
   StandardConnectorSettings.ConnectionUIVersion,
   StandardConnectorSettings.XapFileName,
   new[] { "Scribe.IS.Source", "Scribe.IS.Target", "Scribe.IS2.Source", "Scribe.IS2.Target" },
   ConnectorSettings.SupportsCloud, ConnectorSettings.ConnectorVersion)]
    public class Connector : IConnector
    {
        internal IDictionary<string, string> _connectionInfo = new Dictionary<string, string>();
        internal MetaDataProvider metadata;
        internal QueryProcessor QueryProcessor { get; set; }
        internal OperationProcessor OperationProcessor { get; set; }
        public Guid ConnectorTypeId
        {
            get { return new Guid(ConnectorSettings.ConnectorTypeId); }
        }
        public bool IsConnected
        {
            get;
            set;
        }
        public void Connect(IDictionary<string, string> properties)
        {
            _connectionInfo[ConstantUtils.Username] = properties[ConstantUtils.Username];
            _connectionInfo[ConstantUtils.Password] = properties[ConstantUtils.Password];
            _connectionInfo[ConstantUtils.clientNamespace] = properties[ConstantUtils.clientNamespace];
            this.QueryProcessor = new QueryProcessor(_connectionInfo);
            this.OperationProcessor = new OperationProcessor(_connectionInfo);
            IsConnected = true;
        }
        public void Disconnect()
        {
            IsConnected = false;
        }
        public MethodResult ExecuteMethod(MethodInput input)
        {
            throw new NotImplementedException();
        }
        public OperationResult ExecuteOperation(OperationInput input)
        {
            OperationResult operationResult = null;

            try
            {
                operationResult = this.OperationProcessor.ExecuteOperation(input);
            }

            catch (Exception ex)
            {
                var message = string.Format("{0} {1}", "Error!", ex.Message);
                throw new InvalidExecuteOperationException(message);
            }
            return operationResult;
        }
        public IEnumerable<DataEntity> ExecuteQuery(Query query)
        {
            IEnumerable<DataEntity> entities = null;
            try
            {
                entities = this.QueryProcessor.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entities;

        }
        public IMetadataProvider GetMetadataProvider()
        {
            return metadata = new MetaDataProvider();
        }
        public string PreConnect(IDictionary<string, string> properties)
        {
            var form = new FormDefinition
            {
                CompanyName = "DayForce Portal",
                CryptoKey = ConstantUtils.CryptoKey,
                HelpUri = new Uri("https://help.scribesoft.com"),
                Entries =
               new Collection<EntryDefinition>
               {
                    new EntryDefinition
                    {
                        InputType=InputType.Text,
                        IsRequired=true,
                        Label="UserName",
                        PropertyName=ConstantUtils.Username
                    },
                     new EntryDefinition
                    {
                        InputType=InputType.Text,
                        IsRequired=true,
                        Label="Password",
                        PropertyName=ConstantUtils.Password
                    },
                      new EntryDefinition
                    {
                        InputType=InputType.Text,
                        IsRequired=true,
                        Label="Client Namespace",
                        PropertyName=ConstantUtils.clientNamespace
                    }
               }

            };


            return form.Serialize();

        }

    }
}
