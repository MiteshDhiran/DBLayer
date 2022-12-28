
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class AutoTable  : MDRXCoreEntityBase,ICompositeRoot
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public string C {get;set;}

[DataMember]
 public List<ChildTable> ChildTableList {get;set;}
[DataMember]
 public List<SecondChildTable> SecondChildTable {get;set;}
public AutoTable()
{
ChildTableList = new List<ChildTable>();
SecondChildTable = new List<SecondChildTable>();
}
[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{
ChildTableList?.ForEach(c => c.AutoTable = this);
SecondChildTable?.ForEach(c => c.AutoTable = this);
}
}
}