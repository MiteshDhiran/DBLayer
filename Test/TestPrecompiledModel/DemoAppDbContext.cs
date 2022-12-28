

using Microsoft.EntityFrameworkCore;
using ConnectAndSell.DataAccessStandard.Server.Common;
using DemoApp.DataContract;
using Microsoft.Data.SqlClient;

namespace DemoApp.Server
{
      public partial class DemoAppDbContext : DbContext
      {
            private static DbContextOptions<DemoAppDbContext> GetOptions(SecureConnectionString secureConnectionString)
            {
                  var sqlConnection = new SqlConnection(secureConnectionString.ConnectionString);
                  var optionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>();
                  optionsBuilder.EnableSensitiveDataLogging(true);
                  optionsBuilder.UseSqlServer(sqlConnection);
                  optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                  return optionsBuilder.Options;
            }
            
            public DemoAppDbContext(DbContextOptionsBuilder<DemoAppDbContext> builder) : base(options: builder.Options)
            {
            }
            
            /*
            public DemoAppDbContext(SecureConnectionString secureConnectionString) : base(GetOptions(secureConnectionString))
            {
            }
            */
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  
                  
                  modelBuilder.Entity<AutoTable>(entity =>
                  {
                        entity.HasKey(e => new { e.Id })
                                        .HasName("AutoTablePK");
                                        //.IsClustered(false);
                        entity.ToTable("AutoTable");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.Id)
                        .HasColumnName("Id")
                        .ValueGeneratedOnAdd()
                        ;
                        
                        entity.Property(e => e.Name)
                        .HasColumnName("Name")
                        .HasMaxLength(10)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.C)
                        .HasColumnName("C")
                        .HasMaxLength(1)
                        ;
                  
                  });
                  
                  
                  
                  modelBuilder.Entity<ChildTable>(entity =>
                  {
                        entity.HasKey(e => new { e.ChildID })
                                        .HasName("ChildTablePK");
                                        //.IsClustered(false);
                        entity.ToTable("ChildTable");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.ChildID)
                        .HasColumnName("ChildID")
                        .ValueGeneratedOnAdd()
                        ;
                        
                        entity.Property(e => e.ParentID)
                        .HasColumnName("ParentID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ChildName)
                        .HasColumnName("ChildName")
                        .HasMaxLength(10)
                        .IsRequired()
                        ;
                        
                        entity.HasOne(d => d.AutoTable)
                                            .WithMany(p => p.ChildTableList)
                                            .HasForeignKey(d => new { d.ParentID})
                        					.OnDelete(DeleteBehavior.ClientSetNull);
                  });
                  
                  
                  
                  modelBuilder.Entity<GrandChild>(entity =>
                  {
                        entity.HasKey(e => new { e.GrandchildID })
                                        .HasName("GrandChildPK");
                                        //.IsClustered(false);
                        entity.ToTable("GrandChild");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.GrandchildID)
                        .HasColumnName("GrandchildID")
                        .ValueGeneratedOnAdd()
                        ;
                        
                        entity.Property(e => e.GrandChildName)
                        .HasColumnName("GrandChildName")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ChildID)
                        .HasColumnName("ChildID")
                        .IsRequired()
                        ;
                        
                        entity.HasOne(d => d.ChildTable)
                                            .WithMany(p => p.GrandChild)
                                            .HasForeignKey(d => new { d.ChildID})
                        					.OnDelete(DeleteBehavior.ClientSetNull);
                  });
                  
                  
                  
                  modelBuilder.Entity<ManualChildTable>(entity =>
                  {
                        entity.HasKey(e => new { e.ChildID })
                                        .HasName("ManualChildTablePK");
                                        //.IsClustered(false);
                        entity.ToTable("ManualChildTable");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.ChildID)
                        .HasColumnName("ChildID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ParentID)
                        .HasColumnName("ParentID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.ChildName)
                        .HasColumnName("ChildName")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.TouchedWhen)
                        .HasColumnName("TouchedWhen")
                        .IsRequired()
                        ;
                        
                        entity.HasOne(d => d.ManualTable)
                                            .WithMany(p => p.ManualChildTable)
                                            .HasForeignKey(d => new { d.ParentID})
                        					.OnDelete(DeleteBehavior.ClientSetNull);
                  });
                  
                  
                  
                  modelBuilder.Entity<ManualTable>(entity =>
                  {
                        entity.HasKey(e => new { e.Id })
                                        .HasName("ManualTablePK");
                                        //.IsClustered(false);
                        entity.ToTable("ManualTable");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.Id)
                        .HasColumnName("Id")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Name)
                        .HasColumnName("Name")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.TouchedWhen)
                        .HasColumnName("TouchedWhen")
                        .IsRequired()
                        ;
                  
                  });
                  
                  
                  
                  modelBuilder.Entity<SecondChildTable>(entity =>
                  {
                        entity.HasKey(e => new { e.SecondChildID })
                                        .HasName("SecondChildTablePK");
                                        //.IsClustered(false);
                        entity.ToTable("SecondChildTable");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.SecondChildID)
                        .HasColumnName("SecondChildID")
                        .ValueGeneratedOnAdd()
                        ;
                        
                        entity.Property(e => e.ParentID)
                        .HasColumnName("ParentID")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Name)
                        .HasColumnName("Name")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.HasOne(d => d.AutoTable)
                                            .WithMany(p => p.SecondChildTable)
                                            .HasForeignKey(d => new { d.ParentID})
                        					.OnDelete(DeleteBehavior.ClientSetNull);
                  });
                  
                  
                  
                  modelBuilder.Entity<SXARCMWithSysGeneratedColumns>(entity =>
                  {
                        entity.HasKey(e => new { e.ID })
                                        .HasName("SXARCMWithSysGeneratedColumnsPK");
                                        //.IsClustered(false);
                        entity.ToTable("SXARCMWithSysGeneratedColumns");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.Build)
                        .HasColumnName("Build")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.TouchedBy)
                        .HasColumnName("TouchedBy")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.TouchedWhenUTC)
                        .HasColumnName("TouchedWhenUTC")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.CreatedBy)
                        .HasColumnName("CreatedBy")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.CreatedWhenUTC)
                        .HasColumnName("CreatedWhenUTC")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MSRowVersion)
                        .HasColumnName("MSRowVersion")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .ValueGeneratedOnUpdate()
                        ;
                        
                        entity.Property(e => e.ID)
                        .HasColumnName("ID")
                        .ValueGeneratedOnAdd()
                        ;
                        
                        entity.Property(e => e.Amount)
                        .HasColumnName("Amount")
                        .IsRequired()
                        ;
                  
                  });
                  
                  
                  
                  modelBuilder.Entity<TableWithMSRowVersion>(entity =>
                  {
                        entity.HasKey(e => new { e.Id })
                                        .HasName("TableWithMSRowVersionPK");
                                        //.IsClustered(false);
                        entity.ToTable("TableWithMSRowVersion");
                        entity.Ignore(e => e.DataEntityState);
                        
                        entity.Property(e => e.Id)
                        .HasColumnName("Id")
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.Name)
                        .HasColumnName("Name")
                        .HasMaxLength(50)
                        .IsRequired()
                        ;
                        
                        entity.Property(e => e.MSRowVersion)
                        .HasColumnName("MSRowVersion")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .ValueGeneratedOnUpdate()
                        ;
                  
                  });
                  
            }
      }
}