using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Repository.Interfaces;

namespace ToDoListManagement.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ToDoListDbContext _context;
    public EmployeeRepository(ToDoListDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllEmployeesAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.UserId != userId && !u.IsDeleted && u.Role != "Admin")
            .ToListAsync();
    }

    public async Task<int> AddEmployeeAsync(User user)
    {
        if (user == null)
        {
            return 0;
        }

        _context.Users.Add(user);
        return await _context.SaveChangesAsync();
    }

    public async Task<User> GetEmployeeByIdAsync(int employeeId)
    {
        if (employeeId <= 0)
        {
            return null;
        }

        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == employeeId && !u.IsDeleted);
    }

    public async Task<int> UpdateEmployeeAsync(User user)
    {
        if (user == null || user.UserId <= 0)
        {
            return 0;
        }

        User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId && !u.IsDeleted);
        if (existingUser == null)
        {
            return -1;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Role = user.Role;
        existingUser.IsActive = user.IsActive;

        _context.Users.Update(existingUser);
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> DeleteEmployeeAsync(int employeeId)
    {
        if (employeeId <= 0)
        {
            return 0;
        }

        User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == employeeId && !u.IsDeleted);
        if (existingUser == null)
        {
            return -1;
        }

        existingUser.IsDeleted = true;
        _context.Users.Update(existingUser);
        return await _context.SaveChangesAsync();
    }
}
