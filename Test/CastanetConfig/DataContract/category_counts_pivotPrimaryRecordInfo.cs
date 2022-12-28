
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CastApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class category_counts_pivotPrimaryRecordInfo : IPrimaryKeyBase<category_counts_pivot>, IEquatable<category_counts_pivotPrimaryRecordInfo>
{

public category_counts_pivotPrimaryRecordInfo()
{
	
}
private category_counts_pivotPrimaryRecordInfo()
{
}

public bool Equals(category_counts_pivotPrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is category_counts_pivotPrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    unchecked
{
 return ; ;
}
}

public static bool operator ==(category_counts_pivotPrimaryRecordInfo left, category_counts_pivotPrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(category_counts_pivotPrimaryRecordInfo left, category_counts_pivotPrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}