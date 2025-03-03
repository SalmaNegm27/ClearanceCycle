using System;
using System.Collections.Generic;

namespace ClearanceCycle.Domain.Entities;

public partial class ApprovalGroupEmployee
{
    public int Id { get; set; }

    public int ApprovalGroupId { get; set; }

    public int EmployeeId { get; set; }

    public bool IsActive { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ApprovalGroup ApprovalGroup { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
