
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ChildTableWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int ChildID {get;set;}
[DataMember]
 public int ParentID {get;set;}
[DataMember]
 public string ChildName {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public AutoTableWithResolvedLookupValues AutoTable{get;set;}
[DataMember]
 public List<GrandChildWithResolvedLookupValues> GrandChild {get;set;}
public ChildTableWithResolvedLookupValues()
{
GrandChild = new List<GrandChildWithResolvedLookupValues>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
GrandChild?.ForEach(c => c.ChildTable = this);
}
}
}