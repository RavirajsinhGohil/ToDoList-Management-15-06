using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Web.Controllers;

public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IAuthService authService, IEmployeeService employeeService) : base(authService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        if (SessionUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        List<EmployeeViewModel> employees = await _employeeService.GetAllEmployeesAsync(SessionUser.UserId);
        return View(employees);
    }

    [HttpGet]
    public IActionResult AddEmployee()
    {
        if (SessionUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee(EmployeeViewModel employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.AddEmployeeAsync(employee);
            return RedirectToAction("Index", "Employee");
        }
        return View(employee);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Program Manager")]
    public async Task<IActionResult> GetEmployeeById(int employeeId)
    {
        if (SessionUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        EditEmployeeViewModel employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
        if (employee == null)
        {
            return RedirectToAction("ErrorPage", "ShowError", new { statusCode = 404 });
        }

        return View("EditEmployee", employee);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(EditEmployeeViewModel employee)
    {
        if (ModelState.IsValid)
        {
            int isUpdated = await _employeeService.UpdateEmployeeAsync(employee);
            if (isUpdated == 1)
            {
                TempData["SuccessMessage"] = "Employee data updated successfully.";
            }
            else if (isUpdated == -1)
            {
                TempData["ErrorMessage"] = "Employee not found.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update employee data.";
            }
            return RedirectToAction("Index", "Employee");
        }
        return View(employee);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Program Manager")]
    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        if (SessionUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        int isDeleted = await _employeeService.DeleteEmployeeAsync(employeeId);
        if (isDeleted == 1)
        {
            TempData["SuccessMessage"] = "Employee deleted successfully.";
        }
        else if (isDeleted == -1)
        {
            TempData["ErrorMessage"] = "Employee not found.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete employee.";
        }
        
        return RedirectToAction("Index", "Employee");
    }
    
}
