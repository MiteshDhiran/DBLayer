using System;
using System.Linq;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ChildTypeParentType
    {
        public Type ChildType { get; }
        public Type ParentType { get; }
        
        public Type GrandParentType { get; }

        public string ChildTableName { get; }
        
        public string ParentTableName { get; }
        public string GrandParentTableName { get; }
        
        public EntitySetPropertyInfo EntitySetPropertyInfo { get; }

        public string ChildTempTableNameWithParentName { get; }
        public string ParentTempTableNameWithParentName { get; }
        
        
        public string JoinCondition { get; }

        public ChildTypeParentType(Type childType, Type parentType,Type grandParentType , string childTableName, string parentTableName, string grandParentTableName, EntitySetPropertyInfo entitySetPropertyInfo)
        {
            if(parentType != null && string.IsNullOrEmpty(parentTableName)) throw new ArgumentNullException($"{nameof(parentTableName)}");
            ChildType = childType ?? throw new ArgumentNullException($"{nameof(childType)}");
            ParentType = parentType;
            GrandParentType = grandParentType;
            ChildTableName = childTableName ?? throw new ArgumentNullException($"{nameof(childType)}");
            ParentTableName = parentTableName;
            GrandParentTableName = grandParentTableName;
            EntitySetPropertyInfo = entitySetPropertyInfo;
            ChildTempTableNameWithParentName = string.IsNullOrEmpty(ParentTableName) == false ? $"#{ParentTableName}_{ChildTableName}" : $"#{ChildTableName}";
            ParentTempTableNameWithParentName = string.IsNullOrEmpty(GrandParentTableName) == false ? $"#{GrandParentTableName}_{ParentTableName}" : $"#{ParentTableName}";
            JoinCondition = string.IsNullOrEmpty(ParentTableName) == false ? string.Join(" AND ", entitySetPropertyInfo.ParentTableColumnNames
                    .Zip(entitySetPropertyInfo.ChildTableColumnNames,(parentColumn,childColumn) 
                        => $"{ParentTableName}.{parentColumn} = {ChildTableName}.{childColumn}"))
                : string.Empty;
            

        }

        

        public string GetJoinCondition(string parentTableNamePrefix, string parentTableNameSuffix,
            string childTableNamePrefix, string childTableNameSuffix)
        {
            return string.IsNullOrEmpty(ParentTableName) == false ? string.Join(" AND ", this.EntitySetPropertyInfo.ParentTableColumnNames
                    .Zip(this.EntitySetPropertyInfo.ChildTableColumnNames,(parentColumn,childColumn) 
                        => $"{parentTableNamePrefix}{ParentTableName}{parentTableNameSuffix}.{parentColumn} = {childTableNamePrefix}{ChildTableName}{childTableNameSuffix}.{childColumn}"))
                : string.Empty;        }
    }
}