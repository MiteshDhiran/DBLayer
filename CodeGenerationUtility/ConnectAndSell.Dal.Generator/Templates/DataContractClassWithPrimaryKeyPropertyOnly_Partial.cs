using System;
using System.Collections.Generic;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class DataContractClassWithPrimaryKeyPropertyOnly
    {
        public TableDataContractMetaInfo TableDataContractMetaInfo { get; set; }
        
        public DataContextGeneratorParams DataContextGeneratorParams { get; set; }

        public DataContractClassWithPrimaryKeyPropertyOnly(DataContextGeneratorParams dataContextGeneratorParams, TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            TableDataContractMetaInfo = tableDataContractMetaInfo;
            DataContextGeneratorParams = dataContextGeneratorParams;
        }
    }
}
