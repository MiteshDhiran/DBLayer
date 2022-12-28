using System;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract(IsReference = false)]
    [Serializable]
    public abstract class MDRXCoreEntityBase : IMDRXCoreEntity
    {
        protected MDRXCoreEntityBase()
        {
            this.DataEntityState = DomainEntityState.Unchanged;
        }

        [MDRXNotMapped]
        [DataMember]
        public DomainEntityState DataEntityState { get; set; }
    }

}
