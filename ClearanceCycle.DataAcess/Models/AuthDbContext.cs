using System.Text.Json;
using ClearanceCycle.Domain.Entities;
using ClearanceCycle.WorkFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClearanceCycle.DataAcess.Models;

public partial class AuthDbContext : DbContext
{
    public AuthDbContext()
    {
    }

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cycle> Cycles { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<StepAction> StepActions { get; set; }
    public DbSet<WorkFlow.Models.Action> Actions { get; set; }
    public DbSet<ClearanceRequest> ClearanceRequests { get; set; }
    public DbSet<ClearanceReason> ClearanceReasons { get; set; }
    public DbSet<ClearanceHistory> ClearanceHistories { get; set; }
    public DbSet<StepApprovalGroup> StepApprovalGroups { get; set; }

    public virtual DbSet<ApprovalGroup> ApprovalGroups { get; set; }

    public virtual DbSet<ApprovalGroupEmployee> ApprovalGroupEmployees { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<MajourArea> MajourAreas { get; set; }

    public virtual DbSet<SubDepartment> SubDepartments { get; set; }
    public virtual DbSet<StepApprovalGroupApproval> StepApprovalGroupApproval { get; set; }



    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=172.21.40.10;Initial Catalog=AuthDb;User ID=AppUser;Password=3WZbHigEjnHyCJb8IF2rQo8UBkF5ThpTiyw7FQ6dk94;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);

            entity.ToTable("approvalGroups");

            entity.HasIndex(e => e.GroupTypeId, "IX_approvalGroups_GroupTypeID").HasFillFactor(75);

            entity.Property(e => e.GroupTypeId).HasColumnName("GroupTypeID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<ApprovalGroupEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);

            entity.HasIndex(e => e.ApprovalGroupId, "IX_ApprovalGroupEmployees_ApprovalGroupId").HasFillFactor(75);

            entity.HasIndex(e => e.EmployeeId, "IX_ApprovalGroupEmployees_EmployeeId").HasFillFactor(75);

            entity.HasOne(d => d.ApprovalGroup).WithMany(p => p.ApprovalGroupEmployees).HasForeignKey(d => d.ApprovalGroupId);

            entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalGroupEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);

            entity.HasIndex(e => e.CompanyId, "IX_Departments_CompanyId").HasFillFactor(75);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);

            entity.ToTable(tb => tb.HasTrigger("insteadOfDeleteEmployees"));

            entity.HasIndex(e => e.Active, "IX_Employees_Active_A1BEE").HasFillFactor(75);

            entity.HasIndex(e => new { e.Active, e.HiringDate }, "IX_Employees_Active_HiringDate_152A4").HasFillFactor(75);

            entity.HasIndex(e => new { e.Active, e.HiringDate }, "IX_Employees_Active_HiringDate_F6C90").HasFillFactor(75);

            entity.HasIndex(e => e.CompanyId, "IX_Employees_CompanyId").HasFillFactor(75);

            entity.HasIndex(e => new { e.CompanyId, e.StatusId }, "IX_Employees_CompanyId_StatusId_6D0CB").HasFillFactor(75);

            entity.HasIndex(e => new { e.CompanyId, e.StatusId }, "IX_Employees_CompanyId_StatusId_86C8B").HasFillFactor(75);

            entity.HasIndex(e => new { e.CompanyId, e.StatusId }, "IX_Employees_CompanyId_StatusId_8E816").HasFillFactor(75);

            entity.HasIndex(e => new { e.CompanyId, e.StatusId }, "IX_Employees_CompanyId_StatusId_90593").HasFillFactor(75);

            entity.HasIndex(e => e.ContractTybeId, "IX_Employees_ContractTybeId").HasFillFactor(75);

            entity.HasIndex(e => e.DepartmentId, "IX_Employees_DepartmentId").HasFillFactor(75);

            entity.HasIndex(e => new { e.DepartmentId, e.StatusId }, "IX_Employees_DepartmentId_StatusId_42489").HasFillFactor(75);

            entity.HasIndex(e => new { e.DepartmentId, e.StatusId }, "IX_Employees_DepartmentId_StatusId_57CDB").HasFillFactor(75);

            entity.HasIndex(e => new { e.DepartmentId, e.StatusId }, "IX_Employees_DepartmentId_StatusId_DCF26").HasFillFactor(75);

            entity.HasIndex(e => e.EmployeePositionId, "IX_Employees_EmployeePositionId").HasFillFactor(75);

            entity.HasIndex(e => e.ForceToChangePassword, "IX_Employees_ForceToChangePassword_2991F").HasFillFactor(75);

            entity.HasIndex(e => e.FunctionId, "IX_Employees_FunctionId");

            entity.HasIndex(e => e.LayerId, "IX_Employees_LayerId").HasFillFactor(75);

            entity.HasIndex(e => e.LocationId, "IX_Employees_LocationId");

            entity.HasIndex(e => e.MajourAreaId, "IX_Employees_MajourAreaId").HasFillFactor(75);

            entity.HasIndex(e => e.PlanOptionId, "IX_Employees_PlanOptionId").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_0C5CB").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_20594").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_225DE").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_3AF0E").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_57B12").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active }, "IX_Employees_StatusId_Active_44491").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active }, "IX_Employees_StatusId_Active_558CA").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active }, "IX_Employees_StatusId_Active_65349").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active }, "IX_Employees_StatusId_Active_C4715").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active }, "IX_Employees_StatusId_Active_DAF2E").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active, e.HiringDate }, "IX_Employees_StatusId_Active_HiringDate_37994").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.Active, e.HiringDate }, "IX_Employees_StatusId_Active_HiringDate_A6761").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_B7443").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_C3177").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_D6D18").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_F0BB8").HasFillFactor(75);

            entity.HasIndex(e => e.StatusId, "IX_Employees_StatusId_FC9AA").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_07DB3").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_726FA").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_79C72").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_7C71D").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_930ED").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_9B93F").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_D42D6").HasFillFactor(75);

            entity.HasIndex(e => new { e.StatusId, e.HiringDate }, "IX_Employees_StatusId_HiringDate_F2B4C").HasFillFactor(75);

            entity.HasIndex(e => e.SubDepartmentId, "IX_Employees_SubDepartmentId").HasFillFactor(75);

            entity.HasIndex(e => e.TeamId, "IX_Employees_TeamId").HasFillFactor(75);

            entity.Property(e => e.Gender).HasMaxLength(1);
            entity.Property(e => e.LocationId).HasDefaultValue(0);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MajourArea).WithMany(p => p.Employees)
                .HasForeignKey(d => d.MajourAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SubDepartment).WithMany(p => p.Employees)
                .HasForeignKey(d => d.SubDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MajourArea>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);
        });

        modelBuilder.Entity<SubDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(75);

            entity.HasIndex(e => e.DepartmentId, "IX_SubDepartments_DepartmentId").HasFillFactor(75);

            entity.Property(e => e.ManagerHrid)
                .HasMaxLength(10)
                .HasColumnName("ManagerHRID");

            entity.HasOne(d => d.Department).WithMany(p => p.SubDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ClearanceRequest>()
    .HasOne(c => c.Employee)
    .WithMany()
    .HasForeignKey(c => c.EmployeeId);


        modelBuilder.Entity<ClearanceRequest>()
          .Property(e => e.LastWorkingDayDate)
          .HasColumnType("DATETIME2(0)");


        modelBuilder.Entity<ClearanceRequest>()
        .Property(e => e.CreatedAt)
        .HasColumnType("DATETIME2(0)");
        modelBuilder.Entity<ClearanceHistory>()
        .Property(e => e.ActionAt)
        .HasColumnType("DATETIME2(0)");

        modelBuilder.Entity<Step>()
       .Property(e => e.ApprovalGroupIds)
       .HasConversion(
           v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
           v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null));

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}

