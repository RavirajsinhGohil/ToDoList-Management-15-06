using ToDoListManagement.Entity.ViewModel;

namespace ToDoListManagement.Service.Interfaces;

public interface IProjectService
{
    Task<int> AddProject(ProjectViewModel model, int userId);
    Task<ProjectViewModel?> GetProjectByIdAsync(int projectId   );
    Task<int> UpdateProjectAsync(ProjectViewModel model, int userId);
    Task<bool> DeleteProjectAsync(int projectId, int userId);
}
