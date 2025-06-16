using System.ComponentModel.DataAnnotations;

namespace ToDoListManagement.Entity.ViewModel;

public class ProjectViewModel
{
        public ProjectViewModel()
    {
        Users = [];
    }

    public int? ProjectId { get; set; }

    [Required(ErrorMessage = Constants.Constants.NameRequiredError)]
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }

    [Required(ErrorMessage = Constants.Constants.AssignedToPMRequiredError)]
    public int? AssignedToPM { get; set; }

    public string? PMName { get; set; }

    public List<UserViewModel> Users { get; set; } = [];
}