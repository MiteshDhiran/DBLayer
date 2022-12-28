using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISFMContractMetaInfo
    {
        List<string> GetAllChildPaths(string tableName);
        List<string> GetImmediateChildPropertyNames(string tableName);

        ConcurrentDictionary<SystemDefinedColumn, string> GetSystemDefinedColumnTypeNameDictionary();

        TypeAuditConfiguration GetAuditConfigurationOfType(string typeName);

        
    }
    
    
}
