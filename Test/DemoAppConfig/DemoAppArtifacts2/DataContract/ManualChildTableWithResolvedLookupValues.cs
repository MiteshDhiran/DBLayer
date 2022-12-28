
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class ManualChildTableWithResolvedLookupValues  : MDRXCoreEntityBase
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
 public ManualTableWithResolvedLookupValues ManualTable{get;set;}

public ManualChildTableWithResolvedLookupValues()
{

}

}
}