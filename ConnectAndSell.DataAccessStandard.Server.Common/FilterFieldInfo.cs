using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{

    public enum ConditionJoinType : int
    {
        /// <summary>
        /// 
        /// </summary>
        And = 0,
        /// <summary>
        /// 
        /// </summary>
        
        Or = 1
    }

    public interface IFilterFieldInfo
    {
        MemberExpression PropertyLambda { get; }
        object ColumnValue { get; }
        ConditionJoinType ConditionType { get; }
        PropertyInfo PropertyInfo { get; }

        Type PropertyDefinedInClassType { get; }
    }
    public class FilterFieldInfo<T> : IFilterFieldInfo
    {
        public MemberExpression PropertyLambda { get; }
        public object ColumnValue { get; }
        public ConditionJoinType ConditionType { get; }

        public PropertyInfo PropertyInfo { get; }

        public Type PropertyDefinedInClassType { get; }

        private static PropertyInfo GetPropertyInfo<TSource>(System.Linq.Expressions.MemberExpression propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.",propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",propertyLambda.ToString()));

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format("Expresion '{0}' refers to a property that is not from type {1}.",propertyLambda.ToString(),type));

            return propInfo;
        }
        
        public FilterFieldInfo(MemberExpression propertyLambda,object columnValue,ConditionJoinType conditionType)
        {
            PropertyLambda = propertyLambda ?? throw new ArgumentNullException(nameof(propertyLambda));
            ColumnValue = columnValue;
            ConditionType = conditionType;
            this.PropertyInfo = GetPropertyInfo<T>(propertyLambda) ?? throw new ArgumentException($"Could not find property form expression");
            PropertyDefinedInClassType = typeof(T);
        }

    }

    public class FilterFieldDetailInfo
    {
        public MemberExpression PropertyLambda { get; }
        public object ColumnValue { get; }
        public ConditionJoinType ConditionType { get; }

        public PropertyInfo PropertyInfo { get; }

        public Type PropertyDefinedInClassType { get; }

        public string ColumnName { get; }

        public SqlDbType ColumnSqlDbType { get; }

        public int? StringMaxLength { get; }

        public FilterFieldDetailInfo(ORMModelMetaInfo ormMetaInfo,IFilterFieldInfo filterFieldInfo)
        {
            PropertyLambda = filterFieldInfo.PropertyLambda;
            ColumnValue = filterFieldInfo.ColumnValue;
            PropertyInfo = filterFieldInfo.PropertyInfo;
            PropertyDefinedInClassType = filterFieldInfo.PropertyDefinedInClassType;
            ConditionType = filterFieldInfo.ConditionType;
            var columnPropertyDetails = ormMetaInfo.TableTypeMetaInfoDic[PropertyDefinedInClassType]
                .ColumnPropertiesWithTypeList.FirstOrDefault(p => string.Equals(p.ColumnPropertyMetaInfo.ColumnPropertyName,
                    PropertyInfo.Name, StringComparison.InvariantCultureIgnoreCase));
            if (columnPropertyDetails == null)
            {
                throw new ApplicationException($"Property:{PropertyInfo.Name} defined in class {PropertyDefinedInClassType.FullName} does not have column definition.Filter needs the properties which are mapped to column.");
            }

            if (columnPropertyDetails.ColumnPropertyMetaInfo.SqlDbDataType.HasValue)
            {
                ColumnSqlDbType = columnPropertyDetails.ColumnPropertyMetaInfo.SqlDbDataType.Value;
            }
            else
            {
                throw new ApplicationException($"Property:{PropertyInfo.Name} defined in class {PropertyDefinedInClassType.FullName} Having Column Name: {columnPropertyDetails.ColumnPropertyMetaInfo.ColumnName} does not have sqldbtype in metadata");
            }

            ColumnName = columnPropertyDetails.ColumnPropertyMetaInfo.ColumnName;
            StringMaxLength = columnPropertyDetails.ColumnPropertyMetaInfo.StringMaxLength;
        }

    }
}
