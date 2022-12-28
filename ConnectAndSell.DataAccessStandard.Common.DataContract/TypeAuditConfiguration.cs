using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class TypeAuditConfiguration
    {
        public string PropertyAuditInfoJSON { get; set; }
        
        public string ChildTablePropertyNames { get; set; }

        public string AuditContext { get; set; }

        public string AttachedColumnName { get; set; }

        public string AuditEntityType { get; set; }

        public string PrimaryFirstColumnName { get; set; }
        
    }

    public class TableMetaInfo
    {
        public string TableName { get; set; }
        public string TypeName { get; set; }
        
        public string TypeNamespace { get; set; }

        public string PrimaryFirstColumnName { get; set; }
        
        public AuditMetaInfo AuditInfo { get; set; }
    }
    
    public class AuditMetaInfo
    {
        public string AuditEntityType { get; set; }
        public string AuditContext { get; set; }

        public List<AuditColumnMetaInfo> AuditColumnMetaInfoList { get; set; }
    }

    public class AuditColumnMetaInfo
    {
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public string FormatString { get; set; }
    }
}
