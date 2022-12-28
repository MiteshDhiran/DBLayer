using System.Collections.Generic;

namespace ConnectAndSell.Dal.Generator.Common
{
    public class TypeMapper
  {
    private static Dictionary<string, GATMappingTypes> typeMap = new Dictionary<string, GATMappingTypes>();
    private static List<string> typesRequiringSize = new List<string>();
    private static List<string> valueTypes = new List<string>();
    public static List<string> defaultValueTypes = new List<string>();
    

    static TypeMapper()
    {
      TypeMapper.typeMap.Add("bigint", new GATMappingTypes("long", "bigint"));
      TypeMapper.typeMap.Add("binary", new GATMappingTypes("byte[]", "binary"));
      TypeMapper.typeMap.Add("bit", new GATMappingTypes("bool", "bit"));
      TypeMapper.typeMap.Add("char", new GATMappingTypes("string", "char"));
      TypeMapper.typeMap.Add("datetime", new GATMappingTypes("DateTime", "datetime"));
      TypeMapper.typeMap.Add("date", new GATMappingTypes("DateTime", "date"));
      TypeMapper.typeMap.Add("time", new GATMappingTypes("TimeSpan", "time"));
      TypeMapper.typeMap.Add("datetime2", new GATMappingTypes("DateTime", "datetime2"));
      TypeMapper.typeMap.Add("datetimeoffset", new GATMappingTypes("System.DateTimeOffset", "datetimeoffset"));
      TypeMapper.typeMap.Add("smalldatetime", new GATMappingTypes("DateTime", "smalldatetime"));
      TypeMapper.typeMap.Add("decimal", new GATMappingTypes("decimal"));
      TypeMapper.typeMap.Add("float", new GATMappingTypes("double", "float"));
      TypeMapper.typeMap.Add("image", new GATMappingTypes("byte[]", "image"));
      TypeMapper.typeMap.Add("int", new GATMappingTypes("int"));
      TypeMapper.typeMap.Add("money", new GATMappingTypes("decimal", "money"));
      TypeMapper.typeMap.Add("nchar", new GATMappingTypes("string", "nchar"));
      TypeMapper.typeMap.Add("ntext", new GATMappingTypes("string", "ntext"));
      TypeMapper.typeMap.Add("nvarchar", new GATMappingTypes("string", "nvarchar"));
      TypeMapper.typeMap.Add("real", new GATMappingTypes("Single", "real"));
      TypeMapper.typeMap.Add("smallmoney", new GATMappingTypes("decimal", "smallmoney"));
      TypeMapper.typeMap.Add("smallint", new GATMappingTypes("short", "smallint"));
      TypeMapper.typeMap.Add("text", new GATMappingTypes("string", "text"));
      TypeMapper.typeMap.Add("timestamp", new GATMappingTypes("byte[]", "timestamp"));
      TypeMapper.typeMap.Add("tinyint", new GATMappingTypes("byte", "tinyint"));
      TypeMapper.typeMap.Add("uniqueidentifier", new GATMappingTypes("System.Guid", "uniqueidentifier"));
      TypeMapper.typeMap.Add("varbinary", new GATMappingTypes("byte[]", "varbinary"));
      TypeMapper.typeMap.Add("varchar", new GATMappingTypes("string", "varchar"));
      TypeMapper.typeMap.Add("variant", new GATMappingTypes("object", "variant"));
      TypeMapper.typeMap.Add("sql_variant", new GATMappingTypes("object", "sql_variant"));
      TypeMapper.typeMap.Add("numeric", new GATMappingTypes("decimal", "numeric"));
      TypeMapper.typeMap.Add("xml", new GATMappingTypes("string", "xml"));
      TypeMapper.typesRequiringSize.Add("varchar");
      TypeMapper.typesRequiringSize.Add("char");
      TypeMapper.typesRequiringSize.Add("nvarchar");
      TypeMapper.typesRequiringSize.Add("nchar");
      TypeMapper.typesRequiringSize.Add("binary");
      TypeMapper.typesRequiringSize.Add("decimal");
      TypeMapper.typesRequiringSize.Add("numeric");
      TypeMapper.valueTypes.Add("int");
      TypeMapper.valueTypes.Add("real");
      TypeMapper.valueTypes.Add("smalldatetime");
      TypeMapper.valueTypes.Add("smallmoney");
      TypeMapper.valueTypes.Add("money");
      TypeMapper.valueTypes.Add("float");
      TypeMapper.valueTypes.Add("numeric");
      TypeMapper.valueTypes.Add("smallint");
      TypeMapper.valueTypes.Add("decimal");
      TypeMapper.valueTypes.Add("bigint");
      TypeMapper.valueTypes.Add("binary");
      TypeMapper.valueTypes.Add("bit");
      TypeMapper.valueTypes.Add("datetime");
      TypeMapper.valueTypes.Add("datetime2");
      TypeMapper.valueTypes.Add("date");
      TypeMapper.valueTypes.Add("time");
      TypeMapper.valueTypes.Add("datetimeoffset");
      TypeMapper.valueTypes.Add("tinyint");
      TypeMapper.valueTypes.Add("timestamp");
      TypeMapper.defaultValueTypes.Add("int");
      TypeMapper.defaultValueTypes.Add("long");
      TypeMapper.defaultValueTypes.Add("bool");
      TypeMapper.defaultValueTypes.Add("string");
      TypeMapper.defaultValueTypes.Add("double");
      TypeMapper.defaultValueTypes.Add("float");
      TypeMapper.defaultValueTypes.Add("byte");
      TypeMapper.defaultValueTypes.Add("decimal");
      TypeMapper.defaultValueTypes.Add("bigint");
      TypeMapper.defaultValueTypes.Add("bit");
      TypeMapper.defaultValueTypes.Add("tinyint");
    }

    public static GATMappingTypes GetTypeMap(string dbType)
    {
      if (TypeMapper.typeMap.ContainsKey(dbType.ToLower()))
        return TypeMapper.typeMap[dbType.ToLower()];
      return new GATMappingTypes((string) null);
    }

    public static bool DoesTypeRequiresSize(string type)
    {
      return TypeMapper.typesRequiringSize.Contains(type);
    }

    public static bool IsValueType(string type)
    {
      return TypeMapper.valueTypes.Contains(type);
    }

    public static string ConvertToCamelCasing(string literal)
    {
      if (literal != null)
        return literal.Substring(0, 1).ToLower() + literal.Substring(1);
      return (string) null;
    }

    
  }
}
