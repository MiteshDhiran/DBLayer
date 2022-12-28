

using Microsoft.EntityFrameworkCore;
using ConnectAndSell.DataAccessCore.Server.Common;
using CastApp.DataContract;

namespace CastApp.Server
{
      public partial class CastAppDbContext : DbContext
      {
            private static DbContextOptions<CastAppDbContext> GetOptions(SecureConnectionString secureConnectionString)
            {
                  var sqlConnection = new SqlConnection(secureConnectionString.ConnectionString);
                  var optionsBuilder = new DbContextOptionsBuilder<CastAppDbContext>();
                  optionsBuilder.EnableSensitiveDataLogging(true);
                  optionsBuilder.UseSqlServer(sqlConnection);
                  optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                  return optionsBuilder.Options;
            }
            
            public CastAppDbContext(DbContextOptionsBuilder<CastAppDbContext> builder) : base(options: builder.Options)
            {
            }
            
            /*
            public CastAppDbContext(SecureConnectionString secureConnectionString) : base(GetOptions(secureConnectionString))
            {
            }
            */
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  
                  
                  modelBuilder.Entity<caslist_merged_setting>(entity =>
                  {
                        entity.HasKey(e => new {  })
                                        .HasName("caslist_merged_settingPK");
                                        //.IsClustered(false);
                        entity.ToTable("caslist_merged_setting");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.ListID)
                        .HasColumnName("ListID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.LeadPoolSize)
                        .HasColumnName("LeadPoolSize")
                        ;
                        
                        entity.Property(e => e.IsConnectOnHello)
                        .HasColumnName("IsConnectOnHello")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Delay)
                        .HasColumnName("Delay")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.RestrictedAttemptsEnabled)
                        .HasColumnName("RestrictedAttemptsEnabled")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MaxAttemptsRestricted)
                        .HasColumnName("MaxAttemptsRestricted")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MaxCallPerContact)
                        .HasColumnName("MaxCallPerContact")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.LeadPoolCheck)
                        .HasColumnName("LeadPoolCheck")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ManagerCanEditLeadPoolSize)
                        .HasColumnName("ManagerCanEditLeadPoolSize")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ViewCallSummary)
                        .HasColumnName("ViewCallSummary")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.hasDialerMode)
                        .HasColumnName("hasDialerMode")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.hadLightningMode)
                        .HasColumnName("hadLightningMode")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.SalesforceCRMUsersError)
                        .HasColumnName("SalesforceCRMUsersError")
                        ;
                        
                        entity.Property(e => e.OODEnabled)
                        .HasColumnName("OODEnabled")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.hasCOLVMode)
                        .HasColumnName("hasCOLVMode")
                        ;
                        
                        entity.Property(e => e.CallerID)
                        .HasColumnName("CallerID")
                        .HasMaxLength(20)
                        ;
                        
                        entity.Property(e => e.UseSystemCallerId)
                        .HasColumnName("UseSystemCallerId")
                        ;
                        
                        entity.Property(e => e.ListAssignmentType)
                        .HasColumnName("ListAssignmentType")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.IsConnectOnHelloSetting)
                        .HasColumnName("IsConnectOnHelloSetting")
                        ;
                        
                        entity.Property(e => e.ListType)
                        .HasColumnName("ListType")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.DilaerModeInt)
                        .HasColumnName("DilaerModeInt")
                        ;
                        
                        entity.Property(e => e.ListCompanyID)
                        .HasColumnName("ListCompanyID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.AllowNumberToDial)
                        .HasColumnName("AllowNumberToDial")
                        ;
                        
                        entity.Property(e => e.phoneNumberFieldString)
                        .HasColumnName("phoneNumberFieldString")
                        .HasMaxLength(50)
                        ;
                        
                        entity.Property(e => e.IS_applyMaxAttemptsPerDayFilter)
                        .HasColumnName("IS_applyMaxAttemptsPerDayFilter")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.IS_applyMaxAttemptsPerListFilter)
                        .HasColumnName("IS_applyMaxAttemptsPerListFilter")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.CallFutureCallBackContacts)
                        .HasColumnName("CallFutureCallBackContacts")
                        ;
                        
                        entity.Property(e => e.NextCallReferenceDateTime)
                        .HasColumnName("NextCallReferenceDateTime")
                        ;
                        
                        entity.Property(e => e.EnableCompanytoRestrictCallingonEmergencyAreas)
                        .HasColumnName("EnableCompanytoRestrictCallingonEmergencyAreas")
                        ;
                        
                        entity.Property(e => e.BusinessStartHours)
                        .HasColumnName("BusinessStartHours")
                        ;
                        
                        entity.Property(e => e.BusinessEndhours)
                        .HasColumnName("BusinessEndhours")
                        ;
                        
                        entity.Property(e => e.BusinessLunchStartHours)
                        .HasColumnName("BusinessLunchStartHours")
                        ;
                        
                        entity.Property(e => e.BusinessLunchEndhours)
                        .HasColumnName("BusinessLunchEndhours")
                        ;
                        
                        entity.Property(e => e.AutomaticTimeZoneDetectionEnabled)
                        .HasColumnName("AutomaticTimeZoneDetectionEnabled")
                        ;
                        
                        entity.Property(e => e.LunchTimeZoneDetectionEnabled)
                        .HasColumnName("LunchTimeZoneDetectionEnabled")
                        ;
                        
                        entity.Property(e => e.IsBusinessHours)
                        .HasColumnName("IsBusinessHours")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.IsLunchHours)
                        .HasColumnName("IsLunchHours")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MaxAttempts)
                        .HasColumnName("MaxAttempts")
                        ;
                        
                        entity.Property(e => e.MaxAttemptsList)
                        .HasColumnName("MaxAttemptsList")
                        ;
                        
                        entity.Property(e => e.BadNumberFiltering)
                        .HasColumnName("BadNumberFiltering")
                        ;
                        
                        entity.Property(e => e.WrongNumberFiltering)
                        .HasColumnName("WrongNumberFiltering")
                        ;
                        
                        entity.Property(e => e.DncFiltering)
                        .HasColumnName("DncFiltering")
                        ;
                        
                        entity.Property(e => e.UseProbableDirectNumberEnabled)
                        .HasColumnName("UseProbableDirectNumberEnabled")
                        ;
                  
                  });
                  
                  
                  
                  modelBuilder.Entity<category_counts_pivot>(entity =>
                  {
                        entity.HasKey(e => new {  })
                                        .HasName("category_counts_pivotPK");
                                        //.IsClustered(false);
                        entity.ToTable("category_counts_pivot");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.Checkout)
                        .HasColumnName("Checkout")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.RestrictedForNextCall)
                        .HasColumnName("RestrictedForNextCall")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.DNC)
                        .HasColumnName("DNC")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.IsDisqualified)
                        .HasColumnName("IsDisqualified")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.IsQualifying)
                        .HasColumnName("IsQualifying")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.PhonePenalized)
                        .HasColumnName("PhonePenalized")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.EmergencyArea)
                        .HasColumnName("EmergencyArea")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.GDPR)
                        .HasColumnName("GDPR")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Trigger)
                        .HasColumnName("Trigger")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MultiTouch)
                        .HasColumnName("MultiTouch")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MaxAttemptsPerDayReached)
                        .HasColumnName("MaxAttemptsPerDayReached")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MaxAttemptsPerListReached)
                        .HasColumnName("MaxAttemptsPerListReached")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Callable)
                        .HasColumnName("Callable")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.NotInBusinessHours)
                        .HasColumnName("NotInBusinessHours")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.InLunchHours)
                        .HasColumnName("InLunchHours")
                        .IsRequired()
                        ;
                  
                  });
                  
                  
                  
                  modelBuilder.Entity<list_enriched>(entity =>
                  {
                        entity.HasKey(e => new {  })
                                        .HasName("list_enrichedPK");
                                        //.IsClustered(false);
                        entity.ToTable("list_enriched");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.CID)
                        .HasColumnName("CID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.FN)
                        .HasColumnName("FN")
                        .HasMaxLength(100)
                        ;
                        
                        entity.Property(e => e.LN)
                        .HasColumnName("LN")
                        .HasMaxLength(100)
                        ;
                        
                        entity.Property(e => e.CN)
                        .HasColumnName("CN")
                        .HasMaxLength(500)
                        ;
                        
                        entity.Property(e => e.Title)
                        .HasColumnName("Title")
                        .HasMaxLength(1000)
                        ;
                        
                        entity.Property(e => e.P1)
                        .HasColumnName("P1")
                        .HasMaxLength(50)
                        ;
                        
                        entity.Property(e => e.P2)
                        .HasColumnName("P2")
                        .HasMaxLength(50)
                        ;
                        
                        entity.Property(e => e.P3)
                        .HasColumnName("P3")
                        .HasMaxLength(50)
                        ;
                        
                        entity.Property(e => e.C)
                        .HasColumnName("C")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                  
                  });
                  
            }
      }
}