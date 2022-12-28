
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class GrandChildWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int GrandchildID {get;set;}
[DataMember]
 public string GrandChildName {get;set;}
[DataMember]
 public int ChildID {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public ChildTableWithResolvedLookupValues ChildTable{get;set;}

public GrandChildWithResolvedLookupValues()
{

}

}
}