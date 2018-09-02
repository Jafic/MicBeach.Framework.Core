using MicBeach.Develop.CQuery.CriteriaConvert;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Develop.CQuery.Translator
{
    /// <summary>
    /// Query Translator Implement For SqlServer DataBase
    /// </summary>
    public class MySqlQueryTranslator : IQueryTranslator
    {
        #region Fields

        const string EqualOperator = "=";
        const string GreaterThanOperator = ">";
        const string GreaterThanOrEqualOperator = ">=";
        const string NotEqualOperator = "<>";
        const string LessThanOperator = "<";
        const string LessThanOrEqualOperator = "<=";
        const string InOperator = "IN";
        const string NotInOperator = "NOT IN";
        const string LikeOperator = "LIKE";
        public const string ObjPetName = "TB";
        int subObjectSequence = 0;
        string parameterPrefix = "?";
        const string TreeTableName = "RecurveTable";
        const string TreeTablePetName = "RTT";

        #endregion

        #region Propertys

        /// <summary>
        /// Query Object Pet Name
        /// </summary>
        public string ObjectPetName
        {
            get
            {
                return ObjPetName;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Translate Query Object
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>translate result</returns>
        public TranslateResult Translate(IQuery query)
        {
            return ExecuteTranslate(query);
        }

        /// <summary>
        /// Execute Translate
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="paras">parameters</param>
        /// <param name="objectName">query object name</param>
        /// <returns></returns>
        public TranslateResult ExecuteTranslate(IQuery query, Dictionary<string, object> paras = null, string objectName = "", bool subQuery = false)
        {
            if (query == null)
            {
                return TranslateResult.Empty;
            }
            StringBuilder conditionBuilder = new StringBuilder();
            if (query.QueryType == QueryCommandType.QueryObject)
            {
                StringBuilder orderBuilder = new StringBuilder();
                Dictionary<string, object> parameters = paras ?? new Dictionary<string, object>();
                objectName = string.IsNullOrWhiteSpace(objectName) ? ObjPetName : objectName;
                if (query.Criterias != null && query.Criterias.Count > 0)
                {
                    int index = 0;
                    foreach (var queryItem in query.Criterias)
                    {
                        conditionBuilder.AppendFormat("{0} {1}", index > 0 ? " " + queryItem.Item1.ToString() : "", TranslateCondition(queryItem, parameters, objectName));
                        index++;
                    }
                }
                if (!subQuery && query.Orders != null && query.Orders.Count > 0)
                {
                    foreach (var orderItem in query.Orders)
                    {
                        orderBuilder.AppendFormat("{0} {1},", ConvertOrderCriteriaName(objectName,orderItem), orderItem.Desc ? "DESC" : "ASC");
                    }
                }
                string conditionString = conditionBuilder.ToString();
                string preScript = string.Empty;
                if (query.RecurveCriteria != null)
                {
                    string nowConditionString = conditionBuilder.ToString();
                    conditionString = string.Format("{0}.`{1}` IN (SELECT {3}.`{1}` FROM `{2}` AS {3})", objectName, query.RecurveCriteria.Key, TreeTableName, TreeTablePetName);
                    string firstObjectPetName = objectName;
                    string secondObjectPetName = objectName;
                    string firstTreeTablePetName = TreeTablePetName;
                    preScript = string.Format("WITH RECURSIVE {0} AS (SELECT {1}.`{2}`,{1}.`{3}` FROM `{4}` AS {1} {5} UNION ALL SELECT {6}.`{2}`,{6}.`{3}` FROM `{4}` AS {6} JOIN {0} AS {7}"
                        , TreeTableName
                        , firstObjectPetName
                        , query.RecurveCriteria.Key
                        , query.RecurveCriteria.RelationKey
                        , query.ObjectName
                        , string.IsNullOrWhiteSpace(nowConditionString) ? "" : string.Format("WHERE {0}", nowConditionString)
                        , secondObjectPetName
                        , firstTreeTablePetName);
                    if (query.RecurveCriteria.Direction == RecurveDirection.Up)
                    {
                        preScript = string.Format("{0} ON {1}.`{2}`={3}.`{4}`)", preScript, secondObjectPetName, query.RecurveCriteria.Key, firstTreeTablePetName, query.RecurveCriteria.RelationKey);
                    }
                    else
                    {
                        preScript = string.Format("{0} ON {1}.`{2}`={3}.`{4}`)", preScript, secondObjectPetName, query.RecurveCriteria.RelationKey, firstTreeTablePetName, query.RecurveCriteria.Key);
                    }
                }
                var result = TranslateResult.CreateNewResult(conditionString, orderBuilder.ToString().Trim(','), parameters);
                result.PreScript = preScript;
                return result;
            }
            else
            {
                conditionBuilder.Append(query.QueryText);
                return TranslateResult.CreateNewResult(conditionBuilder.ToString(), string.Empty, query.QueryTextParameters);
            }
        }

        /// <summary>
        /// translate query condition
        /// </summary>
        /// <param name="queryItem">query condition</param>
        /// <returns></returns>
        string TranslateCondition(Tuple<QueryOperator, IQueryItem> queryItem, Dictionary<string, object> parameters, string objectName)
        {
            if (queryItem == null)
            {
                return string.Empty;
            }
            Criteria criteria = queryItem.Item2 as Criteria;
            if (criteria != null)
            {
                return TranslateCriteria(criteria, parameters, objectName);
            }
            IQuery query = queryItem.Item2 as IQuery;
            if (query != null && query.Criterias != null && query.Criterias.Count > 0)
            {
                if (query.Criterias.Count == 1)
                {
                    var firstCriterias = query.Criterias[0];
                    if (firstCriterias.Item2 is Criteria)
                    {
                        return TranslateCriteria(firstCriterias.Item2 as Criteria, parameters, objectName);
                    }
                    return TranslateCondition(firstCriterias, parameters, objectName);
                }
                StringBuilder subCondition = new StringBuilder("(");
                int index = 0;
                foreach (var subQueryItem in query.Criterias)
                {
                    subCondition.AppendFormat("{0} {1}", index > 0 ? " " + subQueryItem.Item1.ToString() : "", TranslateCondition(subQueryItem, parameters, objectName));
                    index++;
                }
                return subCondition.Append(")").ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Translate Single Criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        string TranslateCriteria(Criteria criteria, Dictionary<string, object> parameters, string objectName)
        {
            if (criteria == null)
            {
                return string.Empty;
            }
            IQuery valueQuery = criteria.Value as IQuery;
            string parameterName = criteria.Name + parameters.Count.ToString();
            string sqlOperator = GetOperator(criteria.Operator);
            if (valueQuery != null)
            {
                string subObjName = "TSB" + subObjectSequence;
                subObjectSequence++;
                var subQueryResult = ExecuteTranslate(valueQuery, parameters, subObjName, true);
                string topString = "";
                if (sqlOperator != InOperator && sqlOperator != NotInOperator)
                {
                    topString = "LIMIT 0,1";
                }
                string conditionString = subQueryResult.ConditionString;
                if (!string.IsNullOrWhiteSpace(conditionString))
                {
                    conditionString = "WHERE " + conditionString;
                }
                return string.Format("{0} {1} (SELECT {2}.`{3}` FROM `{4}` {5} {6} {7} {8})", ConvertCriteriaName(objectName, criteria), sqlOperator, subObjName, valueQuery.QueryFields[0], valueQuery.ObjectName, subObjName, conditionString, subQueryResult.OrderString, topString);
            }
            parameters.Add(parameterName, FormatCriteriaValue(criteria.Operator, criteria.GetCriteriaRealValue()));
            return string.Format("{0} {1} {2}{3}", ConvertCriteriaName(objectName, criteria), sqlOperator, parameterPrefix, parameterName);
        }

        /// <summary>
        /// get sql operator by condition operator
        /// </summary>
        /// <param name="criteriaOperator"></param>
        /// <returns></returns>
        string GetOperator(CriteriaOperator criteriaOperator)
        {
            string sqlOperator = string.Empty;
            switch (criteriaOperator)
            {
                case CriteriaOperator.Equal:
                    sqlOperator = EqualOperator;
                    break;
                case CriteriaOperator.GreaterThan:
                    sqlOperator = GreaterThanOperator;
                    break;
                case CriteriaOperator.GreaterThanOrEqual:
                    sqlOperator = GreaterThanOrEqualOperator;
                    break;
                case CriteriaOperator.NotEqual:
                    sqlOperator = NotEqualOperator;
                    break;
                case CriteriaOperator.LessThan:
                    sqlOperator = LessThanOperator;
                    break;
                case CriteriaOperator.LessThanOrEqual:
                    sqlOperator = LessThanOrEqualOperator;
                    break;
                case CriteriaOperator.In:
                    sqlOperator = InOperator;
                    break;
                case CriteriaOperator.NotIn:
                    sqlOperator = NotInOperator;
                    break;
                case CriteriaOperator.Like:
                case CriteriaOperator.BeginLike:
                case CriteriaOperator.EndLike:
                    sqlOperator = LikeOperator;
                    break;
            }
            return sqlOperator;
        }

        /// <summary>
        /// Format Value
        /// </summary>
        /// <param name="criteriaOperator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        dynamic FormatCriteriaValue(CriteriaOperator criteriaOperator, dynamic value)
        {
            dynamic realValue = value;
            switch (criteriaOperator)
            {
                case CriteriaOperator.Like:
                    realValue = string.Format("%{0}%", value);
                    break;
                case CriteriaOperator.BeginLike:
                    realValue = string.Format("{0}%", value);
                    break;
                case CriteriaOperator.EndLike:
                    realValue = string.Format("%{0}", value);
                    break;
            }
            return realValue;
        }

        /// <summary>
        /// convert criteria
        /// </summary>
        /// <param name="objectName">object name</param>
        /// <param name="criteria">criteria</param>
        /// <returns></returns>
        string ConvertCriteriaName(string objectName, Criteria criteria)
        {
            return FormatCriteriaName(objectName, criteria.Name, criteria.Convert);
        }

        string ConvertOrderCriteriaName(string objectName, OrderCriteria orderCriteria)
        {
            return FormatCriteriaName(objectName, orderCriteria.Name, orderCriteria.Convert);
        }

        string FormatCriteriaName(string objectName, string fieldName, ICriteriaConvert convert)
        {
            if (convert == null)
            {
                return string.Format("{0}.`{1}`", objectName, fieldName);
            }
            string convertValue = string.Empty;
            switch (convert.Type)
            {
                case CriteriaConvertType.StringLength:
                    convertValue = string.Format("char_length({0}.`{1}`)", objectName, fieldName);
                    break;
                default:
                    convertValue = string.Format("{0}.`{1}`", objectName, fieldName);
                    break;
            }
            return convertValue;
        }

        #endregion
    }
}
