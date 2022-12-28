
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CastApp.DataContract
{
[DataContract]
public sealed class category_counts_pivotWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int Checkout {get;set;}
[DataMember]
 public int RestrictedForNextCall {get;set;}
[DataMember]
 public int DNC {get;set;}
[DataMember]
 public int IsDisqualified {get;set;}
[DataMember]
 public int IsQualifying {get;set;}
[DataMember]
 public int PhonePenalized {get;set;}
[DataMember]
 public int EmergencyArea {get;set;}
[DataMember]
 public int GDPR {get;set;}
[DataMember]
 public int Trigger {get;set;}
[DataMember]
 public int MultiTouch {get;set;}
[DataMember]
 public int MaxAttemptsPerDayReached {get;set;}
[DataMember]
 public int MaxAttemptsPerListReached {get;set;}
[DataMember]
 public int Callable {get;set;}
[DataMember]
 public int NotInBusinessHours {get;set;}
[DataMember]
 public int InLunchHours {get;set;}


public category_counts_pivotWithResolvedLookupValues()
{

}

}
}