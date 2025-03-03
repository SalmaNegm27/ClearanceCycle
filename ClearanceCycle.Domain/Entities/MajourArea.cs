using System;
using System.Collections.Generic;

namespace ClearanceCycle.Domain.Entities;

public partial class MajourArea
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
