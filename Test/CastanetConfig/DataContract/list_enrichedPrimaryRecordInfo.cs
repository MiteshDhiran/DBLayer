
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CastApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class list_enrichedPrimaryRecordInfo : IPrimaryKeyBase<list_enriched>, IEquatable<list_enrichedPrimaryRecordInfo>
{

public list_enrichedPrimaryRecordInfo()
{
	
}
private list_enrichedPrimaryRecordInfo()
{
}

public bool Equals(list_enrichedPrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is list_enrichedPrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    unchecked
{
 return ; ;
}
}

public static bool operator ==(list_enrichedPrimaryRecordInfo left, list_enrichedPrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(list_enrichedPrimaryRecordInfo left, list_enrichedPrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}