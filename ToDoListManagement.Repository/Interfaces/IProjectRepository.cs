using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Repository.Interfaces;

public interface IProjectRepository
{
    Task<int> AddProject(Project project, int userId);
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<int> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(int projectId, int userId);
}
