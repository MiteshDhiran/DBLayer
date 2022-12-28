using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class AssociatedParentChildTableInfo
    {
        [DataMember] public readonly string AssociateRootTableName; 
        [DataMember] public readonly string AssociateRootClassNamespace; 
        [DataMember] public readonly string AssociateRootClassName; 
        [DataMember] public readonly string AssociateRootClassFullName; 
        [DataMember] public readonly List<AssociateChildInfo> AssociateChildrenInfo;
        [DataMember] public readonly string DomainName;

        public AssociatedParentChildTableInfo(string domainName,string associateRootClassNamespace, string associateRootTableName, string associateRootClassName, List<AssociateChildInfo> associateChildrenInfo )
        {
            DomainName = domainName ?? string.Empty;
            AssociateRootClassNamespace = associateRootClassNamespace ?? throw new ArgumentNullException($"{nameof(associateRootClassNamespace)}");
            AssociateRootTableName = associateRootTableName ?? throw new ArgumentNullException($"{nameof(associateRootTableName)}");
            AssociateRootClassName = associateRootClassName ?? throw new ArgumentNullException($"{nameof(associateRootClassName)}");
            AssociateChildrenInfo = associateChildrenInfo;
            AssociateRootClassFullName = $"{AssociateRootClassNamespace}.{AssociateRootClassName}";
        }
    }
}