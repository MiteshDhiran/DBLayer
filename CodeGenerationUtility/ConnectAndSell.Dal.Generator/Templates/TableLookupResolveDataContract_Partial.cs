using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class TableLookupResolveDataContract
    {
        public TableDataContractMetaInfo TableDataContractMetaInfo { get; }

        public DataContextGeneratorParams DataContextGeneratorParams { get; }

        public TableLookupResolveDataContract(TableDataContractMetaInfo tableDataContractMetaInfo, DataContextGeneratorParams dataContextGeneratorParams)
        {
            TableDataContractMetaInfo = tableDataContractMetaInfo;
            DataContextGeneratorParams = dataContextGeneratorParams;
            /*
            if (TableDataContractMetaInfo.TableName == "SXARCMTransactionCodeMaster")
            {
                Debug.WriteLine(TableDataContractMetaInfo.TablePrimaryKeyLookupResolvePropertiesDefinitionText);
            }
            */
        }
    }
}
