
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CastApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class caslist_merged_settingPrimaryRecordInfo : IPrimaryKeyBase<caslist_merged_setting>, IEquatable<caslist_merged_settingPrimaryRecordInfo>
{

public caslist_merged_settingPrimaryRecordInfo()
{
	
}
private caslist_merged_settingPrimaryRecordInfo()
{
}

public bool Equals(caslist_merged_settingPrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is caslist_merged_settingPrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    unchecked
{
 return ; ;
}
}

public static bool operator ==(caslist_merged_settingPrimaryRecordInfo left, caslist_merged_settingPrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(caslist_merged_settingPrimaryRecordInfo left, caslist_merged_settingPrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}