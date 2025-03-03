using System;
using System.Collections.Generic;
using ClearanceCycle.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClearanceCycle.Domain.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? HrId { get; set; }

    public string? UserName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? NationalId { get; set; }

    public string? Addresss { get; set; }

    public DateTime HiringDate { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public int DepartmentId { get; set; }

    public int SubDepartmentId { get; set; }

    public int MajourAreaId { get; set; }

    public int ContractTybeId { get; set; }

    public int CompanyId { get; set; }

    public int StatusId { get; set; }

    public int LayerId { get; set; }

    public int PlanOptionId { get; set; }

    public string Gender { get; set; } = null!;

    public string? DirectMangerHrId { get; set; }

    public int Grade { get; set; }

    public int VactionBalance { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool Active { get; set; }

    public string? ProfilePicture { get; set; }

    public int DueVacation { get; set; }

    public int PreviousVacation { get; set; }

    public int TakenVacation { get; set; }

    public bool ForceToChangePassword { get; set; }

    public DateTime PasswordExpiryDate { get; set; }

    public string? ArabicFullName { get; set; }

    public int? EmployeePositionId { get; set; }

    public int? TeamId { get; set; }

    public DateTime LastReset { get; set; }

    public string? ResetBy { get; set; }

    public int? LocationId { get; set; }

    public DateTime SmsRestPasswordDate { get; set; }

    public int? FunctionId { get; set; }

    public string? Location { get; set; }

    public string? NavColor { get; set; }

    public string? Position { get; set; }

    public string? SidbarColor { get; set; }

    public virtual ICollection<ApprovalGroupEmployee> ApprovalGroupEmployees { get; set; } = new List<ApprovalGroupEmployee>();

    public virtual Department Department { get; set; } = null!;

    public virtual Function? Function { get; set; }

    public virtual MajourArea MajourArea { get; set; } = null!;

    public virtual SubDepartment SubDepartment { get; set; } = null!;
}
