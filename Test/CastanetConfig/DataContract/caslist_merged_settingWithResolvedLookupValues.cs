
using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CastApp.DataContract
{
[DataContract]
public sealed class caslist_merged_settingWithResolvedLookupValues  : MDRXCoreEntityBase
{
[DataMember]
 public int ListID {get;set;}
[DataMember]
 public Nullable<int> LeadPoolSize {get;set;}
[DataMember]
 public bool IsConnectOnHello {get;set;}
[DataMember]
 public int Delay {get;set;}
[DataMember]
 public bool RestrictedAttemptsEnabled {get;set;}
[DataMember]
 public bool MaxAttemptsRestricted {get;set;}
[DataMember]
 public int MaxCallPerContact {get;set;}
[DataMember]
 public bool LeadPoolCheck {get;set;}
[DataMember]
 public bool ManagerCanEditLeadPoolSize {get;set;}
[DataMember]
 public int ViewCallSummary {get;set;}
[DataMember]
 public int hasDialerMode {get;set;}
[DataMember]
 public bool hadLightningMode {get;set;}
[DataMember]
 public Nullable<int> SalesforceCRMUsersError {get;set;}
[DataMember]
 public bool OODEnabled {get;set;}
[DataMember]
 public Nullable<bool> hasCOLVMode {get;set;}
[DataMember]
 public string CallerID {get;set;}
[DataMember]
 public Nullable<bool> UseSystemCallerId {get;set;}
[DataMember]
 public byte ListAssignmentType {get;set;}
[DataMember]
 public Nullable<bool> IsConnectOnHelloSetting {get;set;}
[DataMember]
 public byte ListType {get;set;}
[DataMember]
 public Nullable<int> DilaerModeInt {get;set;}
[DataMember]
 public int ListCompanyID {get;set;}
[DataMember]
 public Nullable<bool> AllowNumberToDial {get;set;}
[DataMember]
 public string phoneNumberFieldString {get;set;}
[DataMember]
 public int IS_applyMaxAttemptsPerDayFilter {get;set;}
[DataMember]
 public int IS_applyMaxAttemptsPerListFilter {get;set;}
[DataMember]
 public Nullable<bool> CallFutureCallBackContacts {get;set;}
[DataMember]
 public Nullable<DateTime> NextCallReferenceDateTime {get;set;}
[DataMember]
 public Nullable<bool> EnableCompanytoRestrictCallingonEmergencyAreas {get;set;}
[DataMember]
 public Nullable<TimeSpan> BusinessStartHours {get;set;}
[DataMember]
 public Nullable<TimeSpan> BusinessEndhours {get;set;}
[DataMember]
 public Nullable<TimeSpan> BusinessLunchStartHours {get;set;}
[DataMember]
 public Nullable<TimeSpan> BusinessLunchEndhours {get;set;}
[DataMember]
 public Nullable<bool> AutomaticTimeZoneDetectionEnabled {get;set;}
[DataMember]
 public Nullable<bool> LunchTimeZoneDetectionEnabled {get;set;}
[DataMember]
 public int IsBusinessHours {get;set;}
[DataMember]
 public int IsLunchHours {get;set;}
[DataMember]
 public Nullable<int> MaxAttempts {get;set;}
[DataMember]
 public Nullable<int> MaxAttemptsList {get;set;}
[DataMember]
 public Nullable<bool> BadNumberFiltering {get;set;}
[DataMember]
 public Nullable<bool> WrongNumberFiltering {get;set;}
[DataMember]
 public Nullable<bool> DncFiltering {get;set;}
[DataMember]
 public Nullable<bool> UseProbableDirectNumberEnabled {get;set;}


public caslist_merged_settingWithResolvedLookupValues()
{

}

}
}