using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class LookupColumnMap
    {
        [DataMember]public readonly string ColumnName;
        [DataMember]public readonly string PrimaryKeyColumnName;

        public LookupColumnMap(string columnName, string primaryKeyColumnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (string.IsNullOrEmpty(primaryKeyColumnName)) throw new ArgumentNullException(nameof(primaryKeyColumnName));
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            PrimaryKeyColumnName =
                primaryKeyColumnName ?? throw new ArgumentNullException(nameof(primaryKeyColumnName));
        }

        public static bool TryParse(XElement element, out LookupColumnMap columnMap)
        {
            var columnName = element.Attribute("Name")?.Value;
            var primaryTableColumnName = element.Attribute("PrimaryColumn")?.Value;
            if (string.IsNullOrEmpty(columnName) == false && string.IsNullOrEmpty(primaryTableColumnName) == false)
            {
                columnMap = new LookupColumnMap(columnName,primaryTableColumnName);
                return true;
            }
            else
            {
                columnMap = null;
                return false;
            }
        }
    }

    
    [DataContract]
    public enum ResolveLookupType
    {
         [EnumMember]None =0
        ,[EnumMember]Table =1
        ,[EnumMember]Enum =2
    }

    [DataContract]
    [KnownType(typeof(ResolveLookupType))]
    [KnownType(typeof(ResolveTableLookUpInfo))]
    [KnownType(typeof(ResolveEnumLookUpInfo))]
    public abstract class ResolveLookupBase
    {
        [DataMember]public readonly ResolveLookupType ResolveLookupType;
        [DataMember]public readonly List<string> ResolvableLookupColumnNames;
        [DataMember]public readonly string ResolvedPropertyName;

        protected ResolveLookupBase(ResolveLookupType resolveLookupType, List<string> resolvableLookupColumnNames, string resolvedPropertyName)
        {
            if(resolvableLookupColumnNames?.Any() == false) throw new ArgumentException($"{nameof(resolvableLookupColumnNames)} is null or dosen't contains any item");
            if(string.IsNullOrEmpty(resolvedPropertyName)) throw new ArgumentNullException($"{nameof(resolvedPropertyName)}");
            ResolveLookupType = resolveLookupType;
            ResolvableLookupColumnNames = resolvableLookupColumnNames;
            ResolvedPropertyName = resolvedPropertyName;
        }
    }
    [DataContract]
    public class ResolveTableLookUpInfo : ResolveLookupBase
    {
        [DataMember] public readonly string TableName;
        [DataMember] public readonly List<LookupColumnMap> ColumnMaps;


        public ResolveTableLookUpInfo(string tableName,
            List<LookupColumnMap> columnMaps, string resolvedPropertyName) : base(ResolveLookupType.Table,columnMaps?.Select(c => c.ColumnName).ToList(),resolvedPropertyName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            ColumnMaps = columnMaps ?? throw new ArgumentNullException(nameof(columnMaps));
        }
    }

    [DataContract]
    public class ResolveLookUps
    {
        [DataMember] public readonly List<ResolveLookupBase> ResolveLookups;

        public ResolveLookUps(List<ResolveLookupBase> resolveLookups)
        {
            if(resolveLookups?.Any() == false) throw new ArgumentNullException($"{nameof(resolveLookups)}");
            ResolveLookups = resolveLookups ?? throw new ArgumentNullException(nameof(resolveLookups));
        }

        public static bool TryParseResolveLookUps(XElement element, out ResolveLookUps resolveLookUps)
        {
            if (element == null)
            {
                resolveLookUps = null;
                return false;
            }
            var resolveLookUpParsed = element.Descendants("ResolveLookup")
                ?.Select(e => ResolveLookupParser.TryParse(e, out var resolveLookupInfo) ? resolveLookupInfo : null)
                .Where(c => c != null).ToList();
            if (resolveLookUpParsed.Any())
            {
                resolveLookUps = new ResolveLookUps(resolveLookUpParsed);
                return true;
            }
            else
            {
                resolveLookUps = null;
                return false;
            }
        }
        
    }

    [DataContract]
    public class ResolveEnumLookUpInfo : ResolveLookupBase
    {
        [DataMember] public readonly string EnumName;
        [DataMember] public readonly string ColumnName;

        public ResolveEnumLookUpInfo(string enumName,string columnName, string resolvedPropertyName) : base(ResolveLookupType.Enum, new List<string>(){columnName},resolvedPropertyName)
        {
            EnumName = enumName ?? throw new ArgumentNullException(nameof(enumName));
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
        }
    }

    public static class ResolveLookupParser
    {
        public static bool TryParse(XElement element, out ResolveLookupBase resolveLookUpValue)
        {
            var primaryTableName = element.Attribute("ResolvedTableName")?.Value;
            var resolvedPropertyName = element.Attribute("ResolvedPropertyName")?.Value;
            var enumName = element.Attribute("EnumName")?.Value;
            if (string.IsNullOrEmpty(primaryTableName) == false)
            {
                var columnMaps = element.Descendants("LookupColumn").Select(e => LookupColumnMap.TryParse(e, out var map) ? map : null).Where(c => c!= null).ToList();
                if(columnMaps.Any() == false) throw new InvalidOperationException($"No column mapping specified for ResolvedTableName:{primaryTableName}");
                resolvedPropertyName = string.IsNullOrEmpty(resolvedPropertyName)
                    ? $"{string.Join("_", columnMaps.Select(c => c.ColumnName))}LookupValue"
                    : resolvedPropertyName;
                resolveLookUpValue = new ResolveTableLookUpInfo(primaryTableName,columnMaps,resolvedPropertyName);
                return true;
            }
            else if(string.IsNullOrEmpty(enumName) == false)
            {
                var enumColumnName = element.Attribute("ColumnName")?.Value;
                if(string.IsNullOrEmpty("enumColumnName")) throw new ArgumentException($"ColumnName Attribute missing or is empty for enum {enumName}");
                resolvedPropertyName = $"{enumColumnName}LookupValue";
                resolveLookUpValue = new ResolveEnumLookUpInfo(enumName, enumColumnName,resolvedPropertyName);
                return true;
            }
            else
            {
                resolveLookUpValue = null;
                return false;
            }
        }
    }

    
    
    
}
