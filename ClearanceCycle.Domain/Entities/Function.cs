using System;
using System.Collections.Generic;

namespace ClearanceCycle.Domain.Entities;

public partial class Function
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SubDepartmentId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual SubDepartment SubDepartment { get; set; } = null!;
}
