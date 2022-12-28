using System;
using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ParentChildTableInfo
    {
        public string ParentClassNamespace { get; }
        public string ParentTableName { get; }
        public string ChildClassNamespace { get; }
        public string ChildTableName { get; }
        public string FKName { get; set; }
        
        public string EntitySetPropertyName { get; }
        public string EntityRefPropertyName { get; }
        public string IgnoreFields { get; }
        public string AlwaysDBGeneratedFields { get; }
        public PrimaryIDLookupValueRecordInfo PrimaryIDLookupValueRecordInfo { get; }
        
        public ResolveLookUps ResolveLookUps { get; }
        public string ClassNameWithResolvedProperties { get; }
        public string DomainName { get; }

        public List<ColumnExtraInfo> ColumnExtraInfoList { get; set; }
        public ParentChildTableInfo(string parentClassNamespace,string domainName, string parentTableName, string childClassNamespace , string childTableName, string fkName, string entitySetPropertyName, string entityRefPropertyName, string ignoreFields, string alwaysDBGeneratedFields, PrimaryIDLookupValueRecordInfo primaryIDLookupValueRecordInfo, ResolveLookUps resolveLookUps, string classNameWithResolvedProperties,
             List<ColumnExtraInfo> columnExtraInfoList)
        {
            ParentClassNamespace = parentClassNamespace;
            DomainName = domainName ?? string.Empty;
            this.ParentTableName = parentTableName;
            ChildClassNamespace = childClassNamespace;
            this.ChildTableName = childTableName;
            this.FKName = fkName;
            this.EntitySetPropertyName = entitySetPropertyName;
            this.EntityRefPropertyName = entityRefPropertyName;
            this.IgnoreFields = ignoreFields ?? string.Empty;
            this.AlwaysDBGeneratedFields = alwaysDBGeneratedFields ?? string.Empty;
            this.PrimaryIDLookupValueRecordInfo = primaryIDLookupValueRecordInfo;
            ResolveLookUps = resolveLookUps;
            ClassNameWithResolvedProperties =
                string.IsNullOrEmpty(classNameWithResolvedProperties) 
                    ? $"{ParentTableName}WithResolvedLookupValues"
                    : classNameWithResolvedProperties;
            ColumnExtraInfoList = columnExtraInfoList;
        }
    }
}