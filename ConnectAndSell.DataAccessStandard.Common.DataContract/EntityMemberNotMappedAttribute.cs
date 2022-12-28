using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    class EntityMemberNotMappedAttribute : Attribute
    {
        public EntityMemberNotMappedAttribute()
        {
        }
    }
}
