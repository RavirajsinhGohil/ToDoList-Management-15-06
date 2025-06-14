using ToDoListManagement.Entity.Models;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Service.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync(int? userId)
    {
        DashboardViewModel model = new();

        List<Project> projects = await _dashboardRepository.GetProjectsAsync(userId);
        foreach(Project project in projects)
        {
            List<UserViewModel> users = new();
            if (project.ProjectUsers != null )
            {
                foreach (var projectUser in project.ProjectUsers)
                {
                    if (projectUser.UserId != userId)
                    {
                        users.Add(new UserViewModel
                        {
                            UserId = projectUser.User.UserId,
                            Name = projectUser.User.Name ?? string.Empty,
                            Email = projectUser.User.Email ?? string.Empty
                        });
                    }
                }
            }
            model.Projects.Add(new ProjectViewModel
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName ?? string.Empty,
                Description = project.Description ?? string.Empty,
                StartDate = project.CreatedOn.ToLocalTime(),
                EndDate = project.EndDate,
                Status = project.Status ?? string.Empty,
                AssignedToPM = project.AssignedToPM,
                PMName = _dashboardRepository.GetProjectManagerByIdAsync(project.AssignedToPM).Result?.Name ?? string.Empty,
                Users = users
            });
        }

        List<User> projectManagers = await _dashboardRepository.GetProjectManagersAsync(userId);

        model.ProjectManagers = projectManagers.Select(pm => new UserViewModel
        {
            UserId = pm.UserId,
            Name = pm.Name ?? string.Empty,
            Email = pm.Email ?? string.Empty
        }).ToList();

        return model;
    }
}
