using HRNX.Connector.DayForce.Connector;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Utils
{
    class ScribeUtils
    {
        public static string expression;
        public static LogicalExpression LogField { get; set; }
        internal static T EntityToObject<T>(DataEntity scribeEntity) where T : new()
        {
            //create a new entity from the specified type: 
            var restEntity = new T();

            //get the list of fields from the Ultipro entity. Extract into a Dictionary.
            var fieldInfo =
                typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public
                | BindingFlags.GetProperty).ToDictionary(key => key.Name, value => value);

            //create a matching set of key value pairs
            var matchingFieldValues = scribeEntity.Properties.Where(field => fieldInfo.ContainsKey(field.Key));

            //Loop through the field dictionary and assign the values from Scribe Online to the
            //Entity fields:
            foreach (var field in matchingFieldValues)
            {
                Type t = fieldInfo[field.Key].PropertyType;
                dynamic changedObj = Convert.ChangeType(field.Value, t);
                fieldInfo[field.Key].SetValue(restEntity, changedObj, null);
            }
            //values are all assigned to our Ultipro entity, hand it back:
            return restEntity;
        }

        private static string Operator(ComparisonExpression expression)
        {
            if (expression.Operator == ComparisonOperator.GreaterOrEqual)
            {
                return ">=";
            }
            else if (expression.Operator == ComparisonOperator.Like || expression.Operator == ComparisonOperator.Equal)
            {
                return "=";
            }
            else if (expression.Operator == ComparisonOperator.NotLike || expression.Operator == ComparisonOperator.NotEqual)
            {
                return "!=";
            }
            else if (expression.Operator == ComparisonOperator.LessOrEqual)
            {
                return "<=";
            }
            else if (expression.Operator == ComparisonOperator.Greater)
            {
                return ">";
            }
            else if (expression.Operator == ComparisonOperator.Less)
            {
                return "<";
            }
            else
            {
                throw new Exception("Operator not provided by HRNX");
            }
        }
        internal static DayForceFilter CreateFilter(Query query)
        {
            DayForceFilter filter = new DayForceFilter();
            List<LogicalExpression> leftExpression = new List<LogicalExpression>();
            List<ComparisonExpression> rightExpression = new List<ComparisonExpression>();
            filter.name = new List<string>();
            filter.value = new List<string>();
            filter.comparisonOperator = new List<string>();
            filter.logicalOperator = new List<string>();
            if (query.Constraints != null)
            {
                var logicalExpression = query.Constraints as LogicalExpression;
                var logicalExpressionNew = query.Constraints as ComparisonExpression;
                if (logicalExpression != null)
                {
                    expression = logicalExpression.ToString();
                    while (true)
                    {
                        if (expression.Contains("&&") || expression.Contains("||"))
                        {
                            leftExpression.Add(logicalExpression.LeftExpression as LogicalExpression);
                            LogField = logicalExpression.LeftExpression as LogicalExpression;
                            rightExpression.Add(logicalExpression.RightExpression as ComparisonExpression);
                            filter.logicalOperator.Add(logicalExpression.Operator.ToString());
                            if (LogField == null)
                            {
                                logicalExpressionNew = logicalExpression.LeftExpression as ComparisonExpression;
                                expression = logicalExpressionNew.ToString();
                            }
                            else
                            {
                                logicalExpression = LogField;
                                expression = logicalExpression.ToString();
                            }
                            continue;
                        }
                        else
                            break;
                    }
                    filter.name.Add(logicalExpressionNew.LeftValue.Value.ToString());
                    filter.value.Add(logicalExpressionNew.RightValue.Value.ToString());
                    filter.comparisonOperator.Add(Operator(logicalExpressionNew));
                    filter.logicalOperator.Reverse();
                    rightExpression.Reverse();
                    foreach (ComparisonExpression field in rightExpression)
                    {
                        filter.name.Add(field.LeftValue.Value.ToString());
                        filter.value.Add(field.RightValue.Value.ToString());
                        filter.comparisonOperator.Add(Operator(field));
                    }
                }
                else
                {
                    filter.name.Add(logicalExpressionNew.LeftValue.Value.ToString());
                    filter.value.Add(logicalExpressionNew.RightValue.Value.ToString());
                    filter.comparisonOperator.Add(Operator(logicalExpressionNew));
                }
            }


            else
            {
                throw new Exception("Please specify at least one field in filter");
            }

            return filter;
        }
      
        //
        internal static IEnumerable<DataEntity> ToDataEntities<T>(IEnumerable<T> entities)
        {
            //get the type and its properties. We'll use this to build the fields with Reflection
            var type = typeof(T);

            var fields = type.GetProperties(BindingFlags.Instance |
            BindingFlags.FlattenHierarchy |
            BindingFlags.Public |
            BindingFlags.GetProperty);

            //Loop through the retrieved entities and create a DataEntity from it: 
            foreach (var entity in entities)
            {

                var dataEntity = new QueryDataEntity
                {
                    ObjectDefinitionFullName = type.Name,
                    Name = type.Name
                };

                //Add the fields to the entity:

                /* Each KeyValuePair in the fields must be complete.
                 * If the field's value is NULL here, Scribe OnLine will throw
                 * an exception. 
                 * To send a NULL field to Scribe Online, just don't add it to this dictionary.
                 */

                foreach (var field in fields)
                {
                    dataEntity.Properties.Add(
                        field.Name,
                        field.GetValue(entity, null));
                }

                //if (entity. == "CustomFieldValue")
                //{

                //}

                //Hand back the completed object: 
                yield return dataEntity.ToDataEntity();
            }
        }

    }
}

