using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListManagement.Entity.Models;

public partial class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectId { get; set; }

    [StringLength(200)]
    public string? ProjectName { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime? EndDate { get; set; }
    
    public string? Description { get; set; }
    
    public string? Status { get; set; }

    public int? AssignedToPM { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual User? User { get; set; }

    [ForeignKey("AssignedToPM")]
    public virtual User? AssignedPM { get; set; }

    public virtual ICollection<ProjectUser>? ProjectUsers { get; set; } = [];
}
