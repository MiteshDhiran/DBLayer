
using System;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class SQLTableTypeColumnInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnPosition"></param>
        /// <param name="columnName"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="columnInfo"></param>
        /// <param name="isAllowedForInsert"></param>
        /// <param name="dataTableType"></param>
        /// <param name="convertFromDataContractToDataTableTypeFunc"></param>
        /// <param name="syncRequired"></param>
        /// <param name="isPrimaryKey"></param>
        public SQLTableTypeColumnInfo(int columnPosition,string columnName, IColumnNetInfo propertyInfo, MDRXColumnInfo columnInfo, bool isAllowedForInsert, Type dataTableType, Func<object, object> convertFromDataContractToDataTableTypeFunc, bool syncRequired,bool isPrimaryKey)
        {
            ColumnPosition = columnPosition;
            if (propertyInfo != null)
            {
                UnderlyingType = propertyInfo.PropertyType.IsGenericType ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
            }
            else
            {
                UnderlyingType = dataTableType;
            }
            ColumnName = columnName;
            PropertyInfo = propertyInfo;
            ColumnInfo = columnInfo;
            IsAllowedForInsert = isAllowedForInsert;
            DataTableType = dataTableType;
            ConvertFromDataContractToDataTableTypeFunc = convertFromDataContractToDataTableTypeFunc;
            InsertUpdateColumnExpression = Utility.TypeConversion.ContainsKey(UnderlyingType) ? Utility.TypeConversion[UnderlyingType].Item3(columnName) : columnName;
            SyncRequired = syncRequired;
            IsPrimaryKey = isPrimaryKey;
        }
        /// <summary>
        /// True if the column is synched with DB value on insert/update
        /// </summary>
        public bool SyncRequired { get; private set; }
        /// <summary>
        /// Column ordinal in User defined table type
        /// </summary>
        public int ColumnPosition { get; private set; }
        /// <summary>
        /// Name of the column
        /// </summary>
        public string ColumnName { get; private set; }
        /// <summary>
        /// PropertyInfo of column property
        /// </summary>
        public IColumnNetInfo PropertyInfo { get; private set; }
        /// <summary>
        /// ColumnAttribute associated with column property
        /// </summary>
        public MDRXColumnInfo ColumnInfo { get; private set; }
        /// <summary>
        /// True for the columns whose value can be passed from client. For e.g. Identity column and MSRowversion column will have false value
        /// </summary>
        public bool IsAllowedForInsert { get; private set; }
        /// <summary>
        /// Type of column in User defined table  
        /// </summary>
        public Type DataTableType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Func<object, object> ConvertFromDataContractToDataTableTypeFunc { get; private set; }
        /// <summary>
        /// TSQL expression that converts the value thats compatible with table column type
        /// </summary>
        public string InsertUpdateColumnExpression { get; private set; }
        /// <summary>
        /// If the property is nullable of T..then it represents T otherwise it returns T
        /// </summary>
        public Type UnderlyingType { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrimaryKey { get; private set; }

    }
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */