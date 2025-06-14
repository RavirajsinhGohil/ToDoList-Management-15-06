using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Repository.Interfaces;

public interface IEmployeeRepository
{
    Task<List<User>> GetAllEmployeesAsync(int userId);
    Task<int> AddEmployeeAsync(User user);
    Task<User> GetEmployeeByIdAsync(int employeeId);
    Task<int> UpdateEmployeeAsync(User user);
    Task<int> DeleteEmployeeAsync(int employeeId);
}
