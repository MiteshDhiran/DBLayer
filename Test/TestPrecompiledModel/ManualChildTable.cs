
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ManualChildTable  : MDRXCoreEntityBase
{
[DataMember]
 public int ChildID {get;set;}
[DataMember]
 public int ParentID {get;set;}
[DataMember]
 public string ChildName {get;set;}
[DataMember]
 public System.DateTimeOffset TouchedWhen {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public ManualTable ManualTable{get;set;}

public ManualChildTable()
{

}

}
}