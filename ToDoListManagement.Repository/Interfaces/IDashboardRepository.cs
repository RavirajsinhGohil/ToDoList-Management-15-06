using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Repository.Interfaces;

public interface IDashboardRepository
{
    Task<List<Project>> GetProjectsAsync(int? userId);
    Task<List<User>> GetUsersAsync();
    Task<List<User>> GetProjectManagersAsync(int? userId);
    Task<User?> GetProjectManagerByIdAsync(int? managerId);
}
