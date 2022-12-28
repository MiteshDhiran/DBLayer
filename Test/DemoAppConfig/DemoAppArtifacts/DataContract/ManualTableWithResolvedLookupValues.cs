
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ManualTableWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public System.DateTimeOffset TouchedWhen {get;set;}

[DataMember]
 public List<ManualChildTableWithResolvedLookupValues> ManualChildTable {get;set;}
public ManualTableWithResolvedLookupValues()
{
ManualChildTable = new List<ManualChildTableWithResolvedLookupValues>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
ManualChildTable?.ForEach(c => c.ManualTable = this);
}
}
}