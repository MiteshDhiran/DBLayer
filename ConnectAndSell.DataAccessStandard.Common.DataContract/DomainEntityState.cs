using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public enum DomainEntityState
    {
        [EnumMember] New,
        [EnumMember] Deleted,
        [EnumMember] Modified,
        [EnumMember] Unchanged,
    }
}
