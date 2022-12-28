using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class ChildPropertyMetaInfo
    {
        [DataMember] public readonly string ChildEntitySetPropertyName;
        [DataMember] public readonly string ChildEntitySetPropertyType;
        [DataMember] public readonly string FKName;
        [DataMember] public readonly List<string> ChildPropertyNames;
        [DataMember] public readonly List<string> ParentPropertyNames;
        [DataMember] public readonly string EntityRefPropertyName;
        [DataMember] public readonly string ChildClassNamespace;
        [DataMember] public readonly string ChildClassName;
        [DataMember] public readonly string ChildTableName;
        [DataMember] public readonly string ParentTableName;
        public string ChildClassFullName => $"{ChildClassNamespace}.{ChildClassName}";

        public ChildPropertyMetaInfo(string childEntitySetPropertyName, string childEntitySetPropertyType, string fkName, List<string> childPropertyNames, List<string> parentPropertyNames, string entityRefPropertyName, string childClassNamespace,string childClassName, string childTableName, string parentTableName)
        {
            this.ChildEntitySetPropertyName = childEntitySetPropertyName;
            this.ChildEntitySetPropertyType = childEntitySetPropertyType;
            this.FKName = fkName;
            this.ChildPropertyNames = childPropertyNames;
            this.ParentPropertyNames = parentPropertyNames;
            this.EntityRefPropertyName = entityRefPropertyName;
            ChildClassNamespace = childClassNamespace;
            ChildClassName = childClassName;
            ChildTableName = childTableName;
            ParentTableName = parentTableName;
        }
	
    }
}