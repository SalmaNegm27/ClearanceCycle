using System;
using System.Collections.Generic;

namespace ClearanceCycle.Domain.Entities;

public partial class SubDepartment
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int DepartmentId { get; set; }

    public string? ManagerHrid { get; set; }

    public int HeadCount { get; set; }

    public int Occupied { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Function> Functions { get; set; } = new List<Function>();
}
