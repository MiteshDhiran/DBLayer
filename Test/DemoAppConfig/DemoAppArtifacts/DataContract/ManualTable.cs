
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ManualTable  : MDRXCoreEntityBase,ICompositeRoot
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public System.DateTimeOffset TouchedWhen {get;set;}

[DataMember]
 public List<ManualChildTable> ManualChildTable {get;set;}
public ManualTable()
{
ManualChildTable = new List<ManualChildTable>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
ManualChildTable?.ForEach(c => c.ManualTable = this);
}
}
}