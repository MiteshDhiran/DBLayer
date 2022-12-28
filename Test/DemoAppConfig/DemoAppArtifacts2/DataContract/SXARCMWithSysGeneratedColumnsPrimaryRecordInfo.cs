
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DemoApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class SXARCMWithSysGeneratedColumnsPrimaryRecordInfo : IPrimaryKeyBase<SXARCMWithSysGeneratedColumns>, IEquatable<SXARCMWithSysGeneratedColumnsPrimaryRecordInfo>
{
[DataMember]
 public long ID {get;set;}
public SXARCMWithSysGeneratedColumnsPrimaryRecordInfo(long id)
{
	ID = id;
}
private SXARCMWithSysGeneratedColumnsPrimaryRecordInfo()
{
}

public bool Equals(SXARCMWithSysGeneratedColumnsPrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ID == other.ID;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is SXARCMWithSysGeneratedColumnsPrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    return ID.GetHashCode();;
}

public static bool operator ==(SXARCMWithSysGeneratedColumnsPrimaryRecordInfo left, SXARCMWithSysGeneratedColumnsPrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(SXARCMWithSysGeneratedColumnsPrimaryRecordInfo left, SXARCMWithSysGeneratedColumnsPrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}