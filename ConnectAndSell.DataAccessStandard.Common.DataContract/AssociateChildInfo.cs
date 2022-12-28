using System;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class AssociateChildInfo
    {
        [DataMember] public readonly string AssociatePropertyName;
        [DataMember] public readonly string AssociateTableName;
        [DataMember] public readonly string AssociateTableJoinCondition;
        [DataMember] public readonly string FkName;
        [DataMember] public readonly string InterTableName;
        [DataMember] public readonly string InterParentJoin;
        [DataMember] public readonly string InterParentJoinFkName;
        [DataMember] public readonly string InterChildJoin;
        [DataMember] public readonly string InterChildJoinFkName;
        [DataMember] public readonly bool IsDirect;

        public AssociateChildInfo(string associatePropertyName
            , string associateTableName
            , string associateTableJoinCondition
            , string fkName
            , string interTableName
            , string interParentJoin
            , string interParentJoinFKName
            , string interChildJoin
            , string interChildJoinFKName 
            , bool isDirect
        )
        {
            if (string.IsNullOrEmpty(interTableName) == false)
            {
                if (string.IsNullOrEmpty(interParentJoin) || string.IsNullOrEmpty(interParentJoin) || string.IsNullOrEmpty(interChildJoin))
                {
                    throw new ArgumentException($"Argument {nameof(interParentJoin)} or {nameof(interParentJoin)} or {nameof(interChildJoin)} cannot be null when argument {nameof(interTableName)} is having value. Value of argument {nameof(interTableName)} is not null. Value of argument {nameof(interTableName)} is {interTableName}");
                }
            }
            AssociatePropertyName = associatePropertyName ?? throw new ArgumentNullException($"{nameof(associatePropertyName)}");
            AssociateTableName = associateTableName ?? throw new ArgumentNullException($"{nameof(associateTableName)}");
            AssociateTableJoinCondition = associateTableJoinCondition ?? throw new ArgumentNullException($"{nameof(associateTableJoinCondition)}");
            FkName = fkName;
            if (string.IsNullOrEmpty(interTableName) == false)
            {
                InterTableName = interTableName;
                InterParentJoin = interParentJoin?? throw new ArgumentNullException($"{nameof(interParentJoin)}");
                InterParentJoinFkName = interParentJoinFKName ?? throw new ArgumentNullException($"{nameof(interParentJoinFKName)}");
                InterChildJoin = interChildJoin ?? throw new ArgumentNullException($"{nameof(interChildJoin)}");
                InterChildJoinFkName = interChildJoinFKName ?? throw new ArgumentNullException($"{nameof(interChildJoinFKName)}");
            }
            else
            {
                InterTableName = string.Empty;
                InterParentJoin = string.Empty;
                InterParentJoinFkName = string.Empty;
                InterChildJoin = string.Empty;
                InterChildJoinFkName = string.Empty;
            }
            IsDirect = isDirect;
        }
    }
}