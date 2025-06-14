using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Entity.ViewModel;

public class DashboardViewModel
{
    public ProjectViewModel? Project { get; set; }
    public List<ProjectViewModel> Projects { get; set; } = [];
    public List<UserViewModel> ProjectManagers { get; set; } = [];
    public List<UserViewModel> Users { get; set; } = [];
    public int TotalTasks { get; set; }
}
