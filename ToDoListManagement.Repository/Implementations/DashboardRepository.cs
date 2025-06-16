using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Repository.Interfaces;

namespace ToDoListManagement.Repository.Implementations;

public class DashboardRepository : IDashboardRepository
{
    private readonly ToDoListDbContext _context;
    public DashboardRepository(ToDoListDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjectsAsync(int? userId)
    {
        if (userId.HasValue)
        {
            return await _context.Projects
                .Where(p => !p.IsDeleted)
                .Include(pu => pu.ProjectUsers)
                .ThenInclude(u => u.User)
                // .Where(pu => pu.ProjectUsers.Any(u => u.UserId == userId && !u.User.IsDeleted))
                .OrderBy(p => p.ProjectId)
                .ToListAsync();
                // Modify according to Task Completion forprogress report
        }
        return null;
    }

    public async Task<List<User>> GetProjectManagersAsync(int? userId)
    {
        return await _context.Users
            .Where(u => u.Role == "Program Manager" && u.UserId != userId && !u.IsDeleted)
            .ToListAsync();
    }

    public async Task<User?> GetProjectManagerByIdAsync(int? managerId)
    {
        if (!managerId.HasValue) return null;

        return await _context.Users
            .Where(u => u.UserId == managerId && u.Role == "Program Manager" && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
    }
}
