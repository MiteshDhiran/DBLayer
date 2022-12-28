
using System;
using MDRX.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DemoApp.DataContract
{
[DataContract]
public sealed class TableWithMSRowVersion  : MDRXCoreEntityBase,ICompositeRoot
{
[DataMember]
 public int Id {get;set;}
[DataMember]
 public string Name {get;set;}
[DataMember]
 public byte[] MSRowVersion {get;set;}


public TableWithMSRowVersion()
{

}

}
}