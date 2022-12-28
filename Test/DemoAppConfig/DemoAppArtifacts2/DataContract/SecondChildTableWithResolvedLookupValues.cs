
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class SecondChildTableWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int SecondChildID {get;set;}
[DataMember]
 public int ParentID {get;set;}
[DataMember]
 public string Name {get;set;}
[IgnoreDataMember]
[JsonIgnore]
 public AutoTableWithResolvedLookupValues AutoTable{get;set;}

public SecondChildTableWithResolvedLookupValues()
{

}

}
}