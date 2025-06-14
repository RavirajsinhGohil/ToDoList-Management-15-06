using ToDoListManagement.Entity.ViewModel;

namespace ToDoListManagement.Service.Interfaces;

public interface IAuthService
{
    Task<bool> ValidateUserAsync(LoginViewModel model);
    Task<bool> CheckEmailExistsAsync(string email);
    Task<string> GenerateJwtToken(string email);
    Task<bool> RegisterUserAsync(RegisterViewModel model);
    void LogoutUser();
    Task<UserViewModel> GetUserFromToken(string token);
    Task<int?> SendResetPasswordEmailAsync(string email, string resetUrl);
    void LogError(Exception? exception);
    Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);
}
