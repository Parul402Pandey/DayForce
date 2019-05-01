using HRNX.Connector.DayForce.Utils;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Connector
{
    class MetaDataProvider : IMetadataProvider
    {
        private Dictionary<string, System.Type> EntityCollection = new Dictionary<string, System.Type>();
        public MetaDataProvider()
        {
            EntityCollection = PopulateEntityCollection();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public void ResetMetadata() { }
        public IEnumerable<IActionDefinition> RetrieveActionDefinitions()
        {
            var actionDefinitions = new List<IActionDefinition>();
            //for query
            var queryDef = new ActionDefinition
            {
                SupportsConstraints = true,
                SupportsRelations = true,
                SupportsLookupConditions = false,
                SupportsSequences = true,
                KnownActionType = KnownActions.Query,
                SupportsBulk = false,
                Name = KnownActions.Query.ToString(),
                FullName = KnownActions.Query.ToString(),
                Description = string.Empty
            };
            actionDefinitions.Add(queryDef);
            //Create
            var createDef = new ActionDefinition
            {
                SupportsConstraints = true,
                SupportsRelations = true,
                SupportsLookupConditions = false,
                SupportsSequences = true,
                KnownActionType = KnownActions.Create,
                SupportsBulk = false,
                Name = KnownActions.Create.ToString(),
                FullName = KnownActions.Create.ToString(),
                Description = string.Empty
            };
            actionDefinitions.Add(createDef);

            return actionDefinitions;

        }
        public IMethodDefinition RetrieveMethodDefinition(string objectName, bool shouldGetParameters = false)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IMethodDefinition> RetrieveMethodDefinitions(bool shouldGetParameters = false)
        {
            throw new NotImplementedException();

        }
        public IObjectDefinition RetrieveObjectDefinition(string objectName, bool shouldGetProperties = false, bool shouldGetRelations = false)
        {
            IObjectDefinition objectDefinition = null;
            if (EntityCollection.Count > 0)
            {
                foreach (var keyValuePair in EntityCollection)
                {
                    if (keyValuePair.Key == objectName)
                    {
                        System.Type entityType = keyValuePair.Value;
                        if (entityType != null)
                        {
                            objectDefinition = GetObjectDefinition(keyValuePair, true);
                        }
                    }
                }

            }
            return objectDefinition;
        }
        private IObjectDefinition GetObjectDefinition(KeyValuePair<string, System.Type> entity, bool shouldGetFields)
        {
            IObjectDefinition objectDefinition = null;
            objectDefinition = new ObjectDefinition
            {
                Name = entity.Key,
                FullName = entity.Key,
                Description = string.Empty,
                Hidden = false,
                RelationshipDefinitions = new List<IRelationshipDefinition>(),
                PropertyDefinitions = new List<IPropertyDefinition>(),
                SupportedActionFullNames = new List<string>()
            };

            if (entity.Key == ConstantUtils.Employee_Entity)
            {
                objectDefinition.SupportedActionFullNames.Add("Query");
              //  objectDefinition.SupportedActionFullNames.Add("Delete");
            }

            if (entity.Key == ConstantUtils.EmployeeDetails_Entity)
            {
                objectDefinition.SupportedActionFullNames.Add("Lookup");
                objectDefinition.SupportedActionFullNames.Add("Query");

            }
            if (entity.Key == ConstantUtils.EmployeeCreateFlat_Entity)
            {
                objectDefinition.SupportedActionFullNames.Add("Create");
            }
            if (shouldGetFields)
            {
                objectDefinition.PropertyDefinitions = GetFieldDefinitions(entity.Value, entity.Key);
            }
            return objectDefinition;
        }

        public IEnumerable<IObjectDefinition> RetrieveObjectDefinitions(bool shouldGetProperties = false, bool shouldGetRelations = false)
        {
            foreach (var entityType in EntityCollection)
            {
                yield return RetrieveObjectDefinition(entityType.Key, shouldGetProperties, shouldGetRelations);
            };
        }


        private List<IPropertyDefinition> GetFieldDefinitions(System.Type entityType, string entityKey)
        {
            var fields = new List<IPropertyDefinition>();

            //Pull a collection from the incoming entity:
            var fieldsFromType = entityType.GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                BindingFlags.Public | BindingFlags.GetProperty);

            foreach (var field in fieldsFromType)
            {
                var fieldDescription = (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute);
                var propertyDefinition = new PropertyDefinition
                {
                    Name = field.Name,
                    FullName = field.Name,
                    PropertyType = field.PropertyType.ToString(),
                    PresentationType = field.PropertyType.ToString(),
                    Nullable = false,
                    IsPrimaryKey = false,
                    UsedInQueryConstraint = true,
                    UsedInQuerySelect = true,
                    UsedInActionOutput = true,
                    UsedInQuerySequence = true,
                    Description = (fieldDescription != null) ? fieldDescription.Description : field.Name
                };

                //Find any of the following fields that may be attached to the entity. 
                //These are defined by the attributes attached to the property
                foreach (var attribute in field.GetCustomAttributes(false))
                {
                    //whether the field is readonly or not
                    if (attribute is ReadOnlyAttribute)
                    {
                        var readOnly = (ReadOnlyAttribute)attribute;
                        propertyDefinition.UsedInActionInput = readOnly == null || !readOnly.IsReadOnly;
                    }
                    if (attribute is KeyAttribute)
                    {
                        propertyDefinition.UsedInLookupCondition = true;
                    }
                    if (attribute is QueryConstraintAttribute)
                    {
                        propertyDefinition.UsedInQueryConstraint = false;
                    }
                    if (attribute is RequiredAttribute)
                    {
                        propertyDefinition.RequiredInActionInput = true;
                    }
                    if (attribute is QuerySelectAttribute)
                    {
                        propertyDefinition.UsedInQuerySelect = false;
                    }

                }
                fields.Add(propertyDefinition);
            }

            return fields;
        }
        private Dictionary<string, System.Type> PopulateEntityCollection()
        {
            Dictionary<string, System.Type> entities = new Dictionary<string, System.Type>();

            entities.Add(ConstantUtils.Employee_Entity, typeof(HRNX.Connector.DayForce.Entities.Employees));
            entities.Add(ConstantUtils.EmployeeDetails_Entity, typeof(HRNX.Connector.DayForce.Entities.EmployeeDetails));
            entities.Add(ConstantUtils.EmployeeCreateFlat_Entity, typeof(HRNX.Connector.DayForce.Entities.EmployeeCreateFlat));

            return entities;
        }
        public class QueryConstraintAttribute : Attribute
        {
        }

        public class QuerySelectAttribute : Attribute
        {
        }
    }
}
