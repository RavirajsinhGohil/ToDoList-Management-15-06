using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using ToDoListManagement.Entity.Constants;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(IAuthRepository authRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ValidateUserAsync(LoginViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return false;
        }

        bool IsValid = await _authRepository.ValidateUser(model.Email, model.Password);

        if (IsValid)
        {
            string? token = await GenerateJwtToken(model.Email);

            CookieOptions? Token = new()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            if (model.RememberMe)
            {
                Token.Expires = DateTime.Now.AddDays(30);
            }
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("Token", token, Token);

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        return await _authRepository.CheckEmailExistsAsync(email.Trim().ToLower());
    }

    public async Task<string> GenerateJwtToken(string email)
    {
        User user = await _authRepository.GetUserByEmailAsync(email);

        Claim[]? claims =
        [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        ];

        SymmetricSecurityKey? key = new(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        SigningCredentials? creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken? token = new(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(360),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> RegisterUserAsync(RegisterViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return false;
        }
        User user = new()
        {
            Name = model.Name?.Trim(),
            Email = model.Email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password.Trim()),
            Role = "Member",
            IsDeleted = false
        };

        bool isRegistered = await _authRepository.RegisterUserAsync(user);

        return isRegistered;
    }

    public void LogoutUser()
    {
        HttpContext? context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            foreach (string? cookie in context.Request.Cookies.Keys)
            {
                context.Response.Cookies.Delete(cookie);
            }
        }
    }

    public async Task<UserViewModel> GetUserFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(token))
        {
            JwtSecurityToken? jwtToken = tokenHandler.ReadJwtToken(token);
            Claim? emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string email = emailClaim?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                User user = await _authRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    UserViewModel userViewModel = new()
                    {
                        Email = email,
                        Name = user.Name,
                        UserId = user.UserId,
                        Role = user.Role
                    };
                    return userViewModel;
                }
            }
        }

        return null;
    }

    public async Task<int?> SendResetPasswordEmailAsync(string email, string resetUrl)
    {
        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(resetUrl))
        {
            return 0;
        }

        User user = await _authRepository.GetUserByEmailAsync(email.Trim().ToLower());
        if (user == null)
        {
            return -1;
        }

        string emailBody = Constants.ResetPasswordEmailBody;

        emailBody = emailBody.Replace("{ResetLink}", resetUrl);

        string subject = Constants.ResetPasswordEmailSubject;
        await SendEmailAsync(email, subject, emailBody);

        return 1;
    }

    public static async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MimeMessage? emailToSend = new();
        emailToSend.From.Add(MailboxAddress.Parse("test.dotnet@etatvasoft.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        using (SmtpClient? emailClient = new())
        {
            emailClient.Connect("mail.etatvasoft.com", 587, SecureSocketOptions.StartTls);
            emailClient.Authenticate("test.dotnet@etatvasoft.com", "P}N^{z-]7Ilp");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

        await Task.CompletedTask;
    }

    public void LogError(Exception? exception)
    {
        if (exception != null)
        {
            _authRepository.LogError(exception);
        }
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        if (string.IsNullOrEmpty(model.Email))
        {
            return false;
        }

        User user = await _authRepository.GetUserByEmailAsync(model.Email.Trim().ToLower());
        if (user == null)
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword.Trim());
        bool isUpdated = await _authRepository.UpdateUserPassword(user);

        return isUpdated;
    }
}
