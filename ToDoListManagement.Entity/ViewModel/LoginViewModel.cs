using System.ComponentModel.DataAnnotations;

namespace ToDoListManagement.Entity.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = Constants.Constants.EmailRequiredError)]
    [EmailAddress(ErrorMessage = Constants.Constants.EmailInvalidError)]
    public string? Email { get; set; }

    [Required(ErrorMessage = Constants.Constants.PasswordRequiredError)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }    
}
