
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ChildTable  : CoreEntityBase
{
[DataMember]
 public int ChildID {get;set;}
[DataMember]
 public int ParentID {get;set;}
[DataMember]
 public string ChildName {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public AutoTable AutoTable{get;set;}
[DataMember]
 public List<GrandChild> GrandChild {get;set;}
public ChildTable()
{
GrandChild = new List<GrandChild>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
GrandChild?.ForEach(c => c.ChildTable = this);
}
}
}