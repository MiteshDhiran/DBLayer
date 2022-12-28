
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CastApp.DataContract
{
[DataContract]
public sealed class list_enrichedWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int CID {get;set;}
[DataMember]
 public string FN {get;set;}
[DataMember]
 public string LN {get;set;}
[DataMember]
 public string CN {get;set;}
[DataMember]
 public string Title {get;set;}
[DataMember]
 public string P1 {get;set;}
[DataMember]
 public string P2 {get;set;}
[DataMember]
 public string P3 {get;set;}
[DataMember]
 public string C {get;set;}


public list_enrichedWithResolvedLookupValues()
{

}

}
}