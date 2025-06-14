using System.ComponentModel.DataAnnotations;

namespace ToDoListManagement.Entity.ViewModel;

public class EditEmployeeViewModel
{
    public int? EmployeeId { get; set; }

    [Required(ErrorMessage = Constants.Constants.NameRequiredError)]
    [StringLength(100, ErrorMessage = Constants.Constants.NameMaxLengthError)]
    public string? Name { get; set; }

    [Required(ErrorMessage = Constants.Constants.EmailRequiredError)]
    [EmailAddress(ErrorMessage = Constants.Constants.EmailInvalidError)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string? Role { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [RegularExpression(@"^\(?([6-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string? Status { get; set; }
}
