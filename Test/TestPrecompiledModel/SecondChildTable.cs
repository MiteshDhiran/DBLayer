
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class SecondChildTable  : CoreEntityBase
{
[DataMember]
 public int SecondChildID {get;set;}
[DataMember]
 public int ParentID {get;set;}
[DataMember]
 public string Name {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public AutoTable AutoTable{get;set;}

public SecondChildTable()
{

}

}
}