
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class TableWithMSRowVersionWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public byte[] MSRowVersion {get;set;}


public TableWithMSRowVersionWithResolvedLookupValues()
{

}

}
}