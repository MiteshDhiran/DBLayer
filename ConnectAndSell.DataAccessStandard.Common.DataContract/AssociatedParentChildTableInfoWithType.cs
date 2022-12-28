using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class AssociatedParentChildTableInfoWithType
    {
        public readonly AssociatedParentChildTableInfo AssociatedParentChildTableInfo;
        public readonly Type AssociateRootTableType; 

        public AssociatedParentChildTableInfoWithType(AssociatedParentChildTableInfo associatedParentChildTableInfo, Type associateRootTableType)
        {
            AssociatedParentChildTableInfo = associatedParentChildTableInfo;
            AssociateRootTableType = associateRootTableType;

        }
        
    }
}
