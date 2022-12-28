using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class ParentPropertyMetaInfo
    {

        [DataMember] public readonly string ParentClassNamespace;
        [DataMember] public readonly string ParentClassName;
        [DataMember] public readonly string ChildClassNamespace;
        [DataMember] public readonly string ChildClassName;
        [DataMember] public readonly string EntityRefPropertyName;
        [DataMember] public readonly string EntitySetPropertyName;
        [DataMember] public readonly List<string> ChildPropertyNames;
        [DataMember] public readonly List<string> ParentPropertyNames;
        [DataMember] public readonly string ParentClassFullName;
        [DataMember] public readonly string ChildClassFullName;
        [DataMember] public readonly string ParentTableName;
        public ParentPropertyMetaInfo(string parentClassNamespace, string parentClassName,string childClassNamespace ,string childClassName ,List<string> childPropertyNames, List<string> parentPropertyNames,string entityRefPropertyName, string entitySetPropertyName, string parentTableName)
        {
            ParentClassNamespace = parentClassNamespace;
            ParentClassName = parentClassName;
            ChildClassNamespace = childClassNamespace;
            ChildClassName = childClassName;
            ChildPropertyNames = childPropertyNames ?? throw new ArgumentNullException($"Invalid argument{nameof(childPropertyNames)} passed in constructor {nameof(ParentPropertyMetaInfo)} " );
            ParentPropertyNames = parentPropertyNames ?? throw new ArgumentNullException($"Invalid argument{nameof(parentPropertyNames)} passed in constructor {nameof(ParentPropertyMetaInfo)} " );
            EntityRefPropertyName = entityRefPropertyName ?? throw new ArgumentNullException($"Invalid argument{nameof(entityRefPropertyName)} passed in constructor {nameof(ParentPropertyMetaInfo)} " );
            EntitySetPropertyName = entitySetPropertyName ?? throw new ArgumentNullException($"Invalid argument{nameof(entitySetPropertyName)} passed in constructor {nameof(ParentPropertyMetaInfo)} " );
            ParentClassFullName = $"{ParentClassNamespace}.{parentClassName}";
            ChildClassFullName = $"{ChildClassNamespace}.{ChildClassName}";
            ParentTableName = parentTableName;
        }

        public string GetForeignKeyStringForTemplate(string prefix)
        {
            return $"{prefix} => new " + "{ " + string.Join(",", this.ChildPropertyNames.Select(t => $"{prefix}.{t}")) + "}";
        }
    }
}