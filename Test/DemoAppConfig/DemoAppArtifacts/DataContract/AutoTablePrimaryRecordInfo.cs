
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DemoApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class AutoTablePrimaryRecordInfo : IPrimaryKeyBase<AutoTable>, IEquatable<AutoTablePrimaryRecordInfo>
{
[DataMember]
 public int Id {get;set;}
public AutoTablePrimaryRecordInfo(int id)
{
	Id = id;
}
private AutoTablePrimaryRecordInfo()
{
}

public bool Equals(AutoTablePrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is AutoTablePrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    return Id;;
}

public static bool operator ==(AutoTablePrimaryRecordInfo left, AutoTablePrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(AutoTablePrimaryRecordInfo left, AutoTablePrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}