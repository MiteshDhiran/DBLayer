using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ResolvePropertyDependencyOnAnotherTableInfo
    {
        public string PropertyName { get; set; }
        public string AnotherPKTableName { get; set; }
        public string[] AnotherPKTableColumnName { get; }
        public string[] ThisTableColumnName { get; }

        public ResolvePropertyDependencyOnAnotherTableInfo(string propertyName, string anotherPKTableName, string[] anotherPKTableColumnName, string[] thisTableColumnName)
        {
            PropertyName = propertyName;
            AnotherPKTableName = anotherPKTableName;
            AnotherPKTableColumnName = anotherPKTableColumnName;
            ThisTableColumnName = thisTableColumnName;
        }
    }
    
    
    public class ResolvePropertyDependencyOnAnotherTableInfoWithType
    {
        public readonly ResolvePropertyDependencyOnAnotherTableInfo ResolvePropertyDependencyOnAnotherTableInfo;
        public readonly TableDataContractMetaInfoWithType AnotherPKTableDataContractMetaInfoWithType;
        public readonly PropertyInfo PropertyInfoOnLookupResolvedObjectToBeSetWithResolvedValue;

        public ResolvePropertyDependencyOnAnotherTableInfoWithType(ResolvePropertyDependencyOnAnotherTableInfo resolvePropertyDependencyOnAnotherTableInfo, TableDataContractMetaInfoWithType anotherPKTableDataContractMetaInfoWithType, PropertyInfo propertyInfoOnLookupResolvedObjectToBeSetWithResolvedValue)
        {
            ResolvePropertyDependencyOnAnotherTableInfo = resolvePropertyDependencyOnAnotherTableInfo;
            AnotherPKTableDataContractMetaInfoWithType = anotherPKTableDataContractMetaInfoWithType;
            PropertyInfoOnLookupResolvedObjectToBeSetWithResolvedValue = propertyInfoOnLookupResolvedObjectToBeSetWithResolvedValue;
        }
    }
}
