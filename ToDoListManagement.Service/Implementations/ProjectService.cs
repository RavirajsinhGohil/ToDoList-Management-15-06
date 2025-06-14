using ToDoListManagement.Entity.Models;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Service.Implementations;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<int> AddProject(ProjectViewModel model, int userId)
    {
        Project project = new()
        {
            ProjectName = model.ProjectName,
            CreatedBy = userId,
            Description = model.Description,
            CreatedOn = DateTime.UtcNow,
            Status = "Active",
            AssignedToPM = model.AssignedToPM,
            IsDeleted = false
        };

        return await _projectRepository.AddProject(project, model.AssignedToPM ?? 0);
    }

    public async Task<ProjectViewModel?> GetProjectByIdAsync(int projectId)
    {
        if (projectId <= 0 )
        {
            return null;
        }
        
        Project? project = await _projectRepository.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return null;
        }

        return new ProjectViewModel
        {
            ProjectId = project.ProjectId,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.CreatedOn,
            EndDate = project.EndDate,
            Status = project.Status
        };
    }

    public async Task<int> UpdateProjectAsync(ProjectViewModel model, int userId)
    {
        if (model == null || model.ProjectId <= 0 || userId <= 0)
        {
            return 0;
        }
        Project project = new()
        {
            ProjectId = model.ProjectId ?? 0,
            ProjectName = model.ProjectName,
            Description = model.Description
        };

        return await _projectRepository.UpdateProjectAsync(project);
    }

    public async Task<bool> DeleteProjectAsync(int projectId, int userId)
    {
        if (projectId <= 0 || userId <= 0)
        {
            return false;
        }
        
        return await _projectRepository.DeleteProjectAsync(projectId, userId);
    }
}   