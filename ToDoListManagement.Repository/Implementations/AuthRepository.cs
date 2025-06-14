using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Repository.Interfaces;

namespace ToDoListManagement.Repository.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly ToDoListDbContext _context;
    public AuthRepository(ToDoListDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ValidateUser(string email, string password)
    {
        try
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower() && !u.IsDeleted);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while validating user: {ex.Message}");
        }
        return false;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving user", ex);
        }
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        try
        {
            return await _context.Users.AnyAsync(u => u.Email == email.Trim().ToLower());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while checking email existence: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return false;
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while registering user: {ex.Message}");
            return false;
        }
    }

    public async Task LogError(Exception exception)
    {
        try
        {
            if (exception == null)
            {
                return;
            }

            ErrorLog errorLog = new()
            {
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace,
                CreatedOn = DateTime.Now
            };

            await _context.ErrorLogs.AddAsync(errorLog);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while logging error: {ex.Message}");
        }
    }

    public async Task<bool> UpdateUserPassword(User user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return false;
            }

            User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && !u.IsDeleted);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.PasswordHash = user.PasswordHash;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating user password: {ex.Message}");
            return false;
        }
    }
}
