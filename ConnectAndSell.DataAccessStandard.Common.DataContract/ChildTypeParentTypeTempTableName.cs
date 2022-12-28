using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ChildTypeParentTypeTempTableName
    {
        public ChildTypeParentType ChildTypeParentType { get; }
        public string ChildTempTableName { get; }

        public ChildTypeParentTypeTempTableName(ChildTypeParentType childTypeParentType, string childTempTableName)
        {
            ChildTypeParentType = childTypeParentType ?? throw new ArgumentNullException($"{nameof(childTypeParentType)}");
            ChildTempTableName = childTempTableName ?? throw new ArgumentNullException($"{nameof(childTempTableName)}");
        }
    }
}
