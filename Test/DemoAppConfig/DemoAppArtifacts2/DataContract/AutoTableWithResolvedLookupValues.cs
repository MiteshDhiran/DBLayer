
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class AutoTableWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public string C {get;set;}

[DataMember]
 public List<ChildTableWithResolvedLookupValues> ChildTableList {get;set;}
[DataMember]
 public List<SecondChildTableWithResolvedLookupValues> SecondChildTable {get;set;}
public AutoTableWithResolvedLookupValues()
{
ChildTableList = new List<ChildTableWithResolvedLookupValues>();
SecondChildTable = new List<SecondChildTableWithResolvedLookupValues>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
ChildTableList?.ForEach(c => c.AutoTable = this);
SecondChildTable?.ForEach(c => c.AutoTable = this);
}
}
}