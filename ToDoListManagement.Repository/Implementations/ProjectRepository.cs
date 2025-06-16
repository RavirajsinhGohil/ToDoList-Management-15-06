using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Repository.Interfaces;

namespace ToDoListManagement.Repository.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly ToDoListDbContext _context;
    public ProjectRepository(ToDoListDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddProject(Project project, int pMId)
    {
        Project? existingProject = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectName.ToLower() == project.ProjectName.ToLower());
        if (existingProject != null)
        {
            return -1;
        }

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        // ProjectUser projectUser = new()
        // {
        //     ProjectId = project.ProjectId,
        //     UserId = pMId
        // };
        // await _context.ProjectUsers.AddAsync(projectUser);
        // await _context.SaveChangesAsync();

        return project.ProjectId;
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        if (projectId <= 0)
        {
            return null;
        }

        // return await _context.Projects
        //     .Include(p => p.ProjectUsers)
        //     .Include(p => p.AssignedPM)
        //     .FirstOrDefaultAsync(p => p.ProjectId == projectId && !p.IsDeleted);
        return await _context.Projects
        .Include(p => p.ProjectUsers)
            .ThenInclude(pu => pu.User)
        .Include(p => p.AssignedPM) // 👈 This includes the PM (AssignedToPM)
        .FirstOrDefaultAsync(p => p.ProjectId == projectId && !p.IsDeleted);
    }

    public async Task<int> UpdateProjectAsync(Project project)
    {
        if (project == null || project.ProjectId <= 0 )
        {
            return 0;
        }

        Project? existingProject = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId && !p.IsDeleted);
        if (existingProject == null)
        {
            return -1;
        }

        existingProject.ProjectName = project.ProjectName;
        existingProject.Description = project.Description;

        _context.Projects.Update(existingProject);
        await _context.SaveChangesAsync();
        return 1;
    }

    public async Task<bool> DeleteProjectAsync(int projectId, int userId)
    {
        Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId && !p.IsDeleted);

        if (project == null)
        {
            return false;
        }

        project.IsDeleted = true;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        return true;
    }
}
