
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DemoApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class ManualTablePrimaryRecordInfo : IPrimaryKeyBase<ManualTable>, IEquatable<ManualTablePrimaryRecordInfo>
{
[DataMember]
 public int Id {get;set;}
public ManualTablePrimaryRecordInfo(int id)
{
	Id = id;
}
private ManualTablePrimaryRecordInfo()
{
}

public bool Equals(ManualTablePrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is ManualTablePrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    return Id;;
}

public static bool operator ==(ManualTablePrimaryRecordInfo left, ManualTablePrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(ManualTablePrimaryRecordInfo left, ManualTablePrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}