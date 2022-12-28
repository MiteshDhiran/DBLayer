
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class SXARCMWithSysGeneratedColumnsWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int Build {get;set;}
[DataMember]
 public string TouchedBy {get;set;}
[DataMember]
 public System.DateTimeOffset TouchedWhenUTC {get;set;}
[DataMember]
 public string CreatedBy {get;set;}
[DataMember]
 public System.DateTimeOffset CreatedWhenUTC {get;set;}
[DataMember]
 public byte[] MSRowVersion {get;set;}
[DataMember]
 public long ID {get;set;}
[DataMember]
 public int Amount {get;set;}


public SXARCMWithSysGeneratedColumnsWithResolvedLookupValues()
{

}

}
}