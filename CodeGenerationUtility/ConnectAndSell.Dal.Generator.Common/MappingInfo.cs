using System.Collections.Generic;

namespace ConnectAndSell.Dal.Generator.Common
{
    public static class MappingInfo
    {
        public static readonly Dictionary<string, string> SQLDataTypeDic = new Dictionary<string, string>()
        {
            {"bigint", "long"}
            ,{"binary", "byte[]"}
            ,{"bit", "bool"}
            ,{"char", "string"}
            ,{"datetime", "DateTime"}
            ,{"date", "DateTime"}
            ,{"time", "TimeSpan"}
            ,{"datetime2", "DateTime"}
            ,{"datetimeoffset", "System.DateTimeOffset"}
            ,{"smalldatetime", "DateTime"}
            ,{"decimal", "decimal"}
            ,{"float", "double"}
            ,{"image", "byte[]"}
            ,{"int", "int"}
            ,{"money", "decimal"}
            ,{"nchar", "string"}
            ,{"ntext", "string"}
            ,{"nvarchar", "string"}
            ,{"real", "Single"}
            ,{"smallmoney", "decimal"}
            ,{"smallint", "short"}
            ,{"text", "string"}
            ,{"timestamp", "byte[]"}
            ,{"tinyint", "byte"}
            ,{"uniqueidentifier", "System.Guid"}
            ,{"varbinary", "byte[]"}
            ,{"varchar", "string"}
            ,{"variant", "object"}
            ,{"sql_variant", "object"}
            ,{"numeric", "decimal"}
            ,{"xml", "string"}
        };

        public static string ConvertToNullableDataType(string netDataType)
        {
            return netDataType == "string" ? netDataType : $"Nullable<{netDataType}>";
        }
	
    }
}