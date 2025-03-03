using System;
using System.Collections.Generic;

namespace ClearanceCycle.Domain.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? MangerHrId { get; set; }

    public string? DirHrId { get; set; }

    public int? CompanyId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<SubDepartment> SubDepartments { get; set; } = new List<SubDepartment>();
}
