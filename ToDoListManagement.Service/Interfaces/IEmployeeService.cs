using ToDoListManagement.Entity.ViewModel;

namespace ToDoListManagement.Service.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeViewModel>> GetAllEmployeesAsync(int userId);
    Task<int> AddEmployeeAsync(EmployeeViewModel employee);
    Task<EditEmployeeViewModel> GetEmployeeByIdAsync(int employeeId);
    Task<int> UpdateEmployeeAsync(EditEmployeeViewModel employee);
    Task<int> DeleteEmployeeAsync(int employeeId);
}
