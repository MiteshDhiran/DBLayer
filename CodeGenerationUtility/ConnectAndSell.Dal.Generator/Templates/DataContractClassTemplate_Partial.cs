using System;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class DataContractClassTemplate
    {
        public TableDataContractMetaInfo TableDataContractMetaInfo { get; }

        public DataContextGeneratorParams DataContextGeneratorParams { get; }

        public string ColumnPropertiesText { get;  }

        public string EntitySetPropertiesText { get; }

        public string EntitySetPropertiesInitializationText { get;  }

        public string EntityRefPropertyText { get; }
        
        public string OnDeserializedMethodText { get; }

        public string BaseClassText { get; }
        
        public string TableClassName { get; }

        public DataContractClassTemplate(string tableClassName, TableDataContractMetaInfo tableDataContractMetaInfo, DataContextGeneratorParams dataContextGeneratorParams, string columnPropertiesText, string entitySetPropertiesText, string entitySetPropertiesInitializationText, string entityRefPropertyText, string onDeserializedMethodText, string baseClassText)
        {
            TableClassName = tableClassName;
            TableDataContractMetaInfo = tableDataContractMetaInfo;
            DataContextGeneratorParams = dataContextGeneratorParams;
            ColumnPropertiesText = columnPropertiesText;
            EntitySetPropertiesText = entitySetPropertiesText;
            EntitySetPropertiesInitializationText = entitySetPropertiesInitializationText;
            EntityRefPropertyText = entityRefPropertyText;
            OnDeserializedMethodText = onDeserializedMethodText;
            BaseClassText = baseClassText;
        }

    }
}
