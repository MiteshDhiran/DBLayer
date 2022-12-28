using System;
using System.Collections.Generic;
using System.Data;

namespace ConnectAndSell.Dal.Generator.Common
{
    public class SPMetaInfoForCodeGeneration
    {
        public SPMetaInfoForCodeGeneration(string spName, DataTable spiNfoDataTable,
            List<SPResultSetMetaInfo> resultSetTypes, string dalHelperName)
        {
            SPName = spName ?? throw new ArgumentNullException(nameof(spName));
            StoredProcInfo = spiNfoDataTable ?? throw new ArgumentNullException(nameof(spiNfoDataTable));
            this.ResultSetTypes = resultSetTypes ?? throw new ArgumentNullException(nameof(resultSetTypes));
            DALHelperName = dalHelperName ?? throw new ArgumentNullException(nameof(dalHelperName));
        }

        public string SPName { get; }
        public DataTable StoredProcInfo { get; }
        public List<SPResultSetMetaInfo> ResultSetTypes { get; }
        
        public string DALHelperName { get; }
    }

    public class SPResultSetMetaInfo
    {
        public SPResultSetMetaInfo(string className, string ns)
        {
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
            Namespace = ns ?? throw new ArgumentNullException(nameof(ns));
        }

        public string ClassName { get; }
        public string Namespace { get; }

        
    }
}
