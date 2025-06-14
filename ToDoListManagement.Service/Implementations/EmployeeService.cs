using ToDoListManagement.Entity.Models;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Service.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync(int userId)
    {
        List<User> users = await _employeeRepository.GetAllEmployeesAsync(userId);
        List<EmployeeViewModel> employees = new();
        foreach (User user in users)
        {
            employees.Add(new EmployeeViewModel
            {
                EmployeeId = user.UserId,
                Name = user.Name ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Role = user.Role ?? string.Empty,
                Status = user.IsActive ? "Active" : "Inactive",
            });
        }
        return employees ?? new List<EmployeeViewModel>();
    }

    public async Task<int> AddEmployeeAsync(EmployeeViewModel employee)
    {
        User user = new()
        {
            Name = employee.Name,
            Email = employee.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(employee.Password.Trim()), 
            PhoneNumber = employee.PhoneNumber,
            Role = employee.Role
            // CreatedBy = employee.CreatedBy
        };
        return await _employeeRepository.AddEmployeeAsync(user);
    }

    public async Task<EditEmployeeViewModel> GetEmployeeByIdAsync(int employeeId)
    {
        User user = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if (user == null)
        {
            return null;
        }
        return new EditEmployeeViewModel
        {
            EmployeeId = user.UserId,
            Name = user.Name ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Role = user.Role ?? string.Empty,
            Status = user.IsActive ? "Active" : "Inactive",
        };
    }

    public async Task<int> UpdateEmployeeAsync(EditEmployeeViewModel employee)
    {
        User user = new()
        {
            UserId = employee.EmployeeId ?? 0,
            Name = employee.Name,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Role = employee.Role,
            IsActive = employee.Status == "Active" ? true : false
        };
        return await _employeeRepository.UpdateEmployeeAsync(user);
    }

    public async Task<int> DeleteEmployeeAsync(int employeeId)
    {
        return await _employeeRepository.DeleteEmployeeAsync(employeeId);
    }
}
