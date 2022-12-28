using System;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract(IsReference = false)]
    [Serializable]
    public abstract class CoreEntityBase : ICoreEntity
    {
        protected CoreEntityBase()
        {
            this.DataEntityState = DomainEntityState.Unchanged;
        }

        [EntityMemberNotMapped]
        [DataMember]
        public DomainEntityState DataEntityState { get; set; }
    }

}
