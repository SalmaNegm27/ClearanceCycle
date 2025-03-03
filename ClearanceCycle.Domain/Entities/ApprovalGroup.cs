namespace ClearanceCycle.Domain.Entities;

public partial class ApprovalGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int GroupTypeId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ApprovalGroupEmployee> ApprovalGroupEmployees { get; set; } = new List<ApprovalGroupEmployee>();
}
