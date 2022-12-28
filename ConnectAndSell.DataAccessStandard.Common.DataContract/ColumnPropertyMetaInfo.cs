

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// ColumnPropertyMetaInfo Class
    /// </summary>
    [DataContract]
    public class ColumnPropertyMetaInfo
    {
        /// <summary>
        /// The column name
        /// </summary>
        [DataMember] public readonly string ColumnName;
        /// <summary>
        /// The column property name
        /// </summary>
        [DataMember] public readonly string ColumnPropertyName;
        /// <summary>
        /// The net data type
        /// </summary>
        [DataMember] public readonly string NetDataType;
        /// <summary>
        /// The SQL data type
        /// </summary>
        [DataMember] public readonly string SqlDataType;
        /// <summary>
        /// The is identity
        /// </summary>
        [DataMember] public readonly bool IsIdentity;
        /// <summary>
        /// The is version
        /// </summary>
        [DataMember] public readonly bool IsVersion;
        /// <summary>
        /// The SQL default value
        /// </summary>
        [DataMember] public readonly string SqlDefaultValue;
        /// <summary>
        /// The is nullable
        /// </summary>
        [DataMember] public readonly bool IsNullable;
        /// <summary>
        /// The is pk column
        /// </summary>
        [DataMember] public readonly bool IsPKColumn;
        /// <summary>
        /// The pk ordinal position
        /// </summary>
        [DataMember] public readonly int? PKOrdinalPosition;
        /// <summary>
        /// The is required
        /// </summary>
        [DataMember] public readonly bool IsRequired;
        /// <summary>
        /// The string maximum length
        /// </summary>
        [DataMember] public readonly int? StringMaxLength;
        /// <summary>
        /// The is generated on add
        /// </summary>
        [DataMember] public readonly bool IsGeneratedOnAdd;
        /// <summary>
        /// The is generated on update
        /// </summary>
        [DataMember] public readonly bool IsGeneratedOnUpdate;
        /// <summary>
        /// The column exta information
        /// </summary>
        [DataMember] public readonly ColumnExtraInfo ColumnExtaInfo;

        [DataMember] public readonly string BackingFieldName;

        [DataMember] public System.Data.SqlDbType? SqlDbDataType;

        /// <summary>
        /// Gets the net data type for data contract.
        /// </summary>
        /// <value>
        /// The net data type for data contract.
        /// </value>
        public string NetDataTypeForDataContract => !string.IsNullOrEmpty(ColumnExtaInfo?.EnumName)
                                                                    ? IsNullable ? $"Nullable<{ColumnExtaInfo.EnumName}>" : ColumnExtaInfo.EnumName
                                                                    : NetDataType;
        /// <summary>
        /// Gets the type of the SQL data type for table.
        /// </summary>
        /// <value>
        /// The type of the SQL data type for table.
        /// </value>
        public string SqlDataTypeForTableType => IsVersion ? "varchar(200)" : SqlDataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPropertyMetaInfo"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnPropertyName">Name of the column property.</param>
        /// <param name="backingFieldName"></param>
        /// <param name="netDataType">Type of the net data.</param>
        /// <param name="sqlDataType">Type of the SQL data.</param>
        /// <param name="isIdentity">if set to <c>true</c> [is identity].</param>
        /// <param name="isVersion">if set to <c>true</c> [is version].</param>
        /// <param name="sqlDefaultValue">The SQL default value.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        /// <param name="isPKColumn">if set to <c>true</c> [is pk column].</param>
        /// <param name="pkOrdinalPosition">The pk ordinal position.</param>
        /// <param name="stringMaxLength">Maximum length of the string.</param>
        /// <param name="isGeneratedOnAdd">if set to <c>true</c> [is generated on add].</param>
        /// <param name="isGeneratedOnUpdate">if set to <c>true</c> [is generated on update].</param>
        /// <param name="columnExtraInfo">The column extra information.</param>
        /// <exception cref="ArgumentNullException">columnName</exception>
        public ColumnPropertyMetaInfo(string columnName,string columnPropertyName,string backingFieldName,string netDataType,string sqlDataType,bool isIdentity,bool isVersion,string sqlDefaultValue,bool isNullable,bool isPKColumn,int? pkOrdinalPosition,int? stringMaxLength,bool isGeneratedOnAdd,bool isGeneratedOnUpdate,ColumnExtraInfo columnExtraInfo)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (string.IsNullOrEmpty(columnPropertyName))
            {
                throw new ArgumentNullException(nameof(columnPropertyName));
            }

            if (string.IsNullOrEmpty(sqlDataType))
            {
                throw new ArgumentNullException(nameof(sqlDataType));
            }

            this.ColumnName = columnName;
            this.ColumnPropertyName = columnPropertyName;
            this.BackingFieldName = backingFieldName;
            this.NetDataType = netDataType;
            this.SqlDataType = sqlDataType;
            this.IsIdentity = isIdentity;
            this.IsVersion = isVersion;
            this.SqlDefaultValue = sqlDefaultValue;
            this.IsNullable = isNullable;
            this.IsPKColumn = isPKColumn;
            this.PKOrdinalPosition = pkOrdinalPosition;
            this.StringMaxLength = stringMaxLength;
            this.IsRequired = (isIdentity == false && isNullable == false);
            this.IsGeneratedOnAdd = isGeneratedOnAdd;
            this.IsGeneratedOnUpdate = isGeneratedOnUpdate;
            this.ColumnExtaInfo = columnExtraInfo;
            this.SqlDbDataType = TryGetSqlParameterDbType(sqlDataType) ?? throw new ArgumentException($"Could not convert db type string {sqlDataType} to sql type of column {columnName}");
        }

        [OnDeserialized()]
        void OnDeserializedMethod(StreamingContext context)
        {
            if (SqlDbDataType == null)
            {
                SqlDbDataType = TryGetSqlParameterDbType(SqlDataType) ?? throw new ArgumentException($"Could not convert db type string {SqlDataType} to sql type of column {ColumnName}");
            }
        }

        private static readonly ConcurrentDictionary<string,SqlDbType> SqlTypemapper =
            new ConcurrentDictionary<string,SqlDbType>(
                new Dictionary<string,SqlDbType>()
                {
                    {"bigint", SqlDbType.BigInt},
                    {"long", SqlDbType.BigInt}, {"binary", SqlDbType.Binary}, {"bit", SqlDbType.Bit},
                    {"char", SqlDbType.Char}, {"datetime", SqlDbType.DateTime}, {"date", SqlDbType.Date},
                    {"time", SqlDbType.Time}, {"datetime2", SqlDbType.DateTime2},
                    {"datetimeoffset", SqlDbType.DateTimeOffset}, {"smalldatetime", SqlDbType.SmallDateTime},
                    {"decimal", SqlDbType.Decimal}, {"image", SqlDbType.Image}, {"int", SqlDbType.Int},
                    {"nchar", SqlDbType.NChar}, {"ntext", SqlDbType.NText}, {"nvarchar", SqlDbType.NVarChar},
                    {"real", SqlDbType.Real}, {"smallmoney", SqlDbType.SmallMoney}, {"smallint", SqlDbType.SmallInt},
                    {"text", SqlDbType.Text}, {"timestamp", SqlDbType.Timestamp}, {"tinyint", SqlDbType.TinyInt},
                    {"uniqueidentifier", SqlDbType.UniqueIdentifier}, {"varbinary", SqlDbType.VarBinary},
                    {"varchar", SqlDbType.VarChar}, {"variant", SqlDbType.Variant}, {"sql_variant", SqlDbType.Variant},
                    {"xml", SqlDbType.Xml}, {"numeric", SqlDbType.Decimal}
                }
            );

        private static SqlDbType? TryGetSqlParameterDbType(string dbTypeName)
        {
            string str2 = dbTypeName.IndexOf("(",StringComparison.Ordinal) <= 0
                ? dbTypeName.Trim()
                : dbTypeName.Substring(0,dbTypeName.IndexOf("(",StringComparison.InvariantCultureIgnoreCase)).Trim();
            str2 = str2.ToLower();
            str2 = str2.Replace(" not null", "");
            str2 = str2.Replace(" null", "");
            str2 = str2.Replace(" identity", "");
            str2 = str2.Trim();
            
            if (!SqlTypemapper.ContainsKey(str2.ToLower()))
                return null;
            else
            {
                return SqlTypemapper[str2.ToLower()];
            }

        }
    }
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */