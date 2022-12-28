
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DemoApp.DataContract
{
[DataContract(Namespace ="")]
public sealed class TableWithMSRowVersionPrimaryRecordInfo : IPrimaryKeyBase<TableWithMSRowVersion>, IEquatable<TableWithMSRowVersionPrimaryRecordInfo>
{
[DataMember]
 public int Id {get;set;}
public TableWithMSRowVersionPrimaryRecordInfo(int id)
{
	Id = id;
}
private TableWithMSRowVersionPrimaryRecordInfo()
{
}

public bool Equals(TableWithMSRowVersionPrimaryRecordInfo other)
{
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
}

public override bool Equals(object obj)
{
        return ReferenceEquals(this, obj) || obj is TableWithMSRowVersionPrimaryRecordInfo other && Equals(other);
}

public override int GetHashCode()
{
    return Id;;
}

public static bool operator ==(TableWithMSRowVersionPrimaryRecordInfo left, TableWithMSRowVersionPrimaryRecordInfo right)
{
        return Equals(left, right);
}

public static bool operator !=(TableWithMSRowVersionPrimaryRecordInfo left, TableWithMSRowVersionPrimaryRecordInfo right)
{
 return !Equals(left, right);
}

}

}