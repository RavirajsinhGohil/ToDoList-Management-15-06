namespace ToDoListManagement.Entity.Constants;

public class Constants
{
    #region Error Messages

    public const string ErrorCode404 = "The page you are looking for was not found.";
    public const string ErrorCode500 = "An unexpected server error occurred.";
    public const string ErrorCode403 = "Unauthorized";
    public const string ErrorCode401 = "Unauthenticated";
    public const string ErrorCodeDefault = "An unexpected error occurred.";

    #endregion

    #region Error Status

    public const string ErrorStatus404 = "Oops, the page you're looking for isn't here.";
    public const string ErrorStatus500 = "An unexpected server error occurred.";
    public const string ErrorStatus403 = "You do not have permission to access this resource.";
    public const string ErrorStatus401 = "You are not authenticated to access this resource.";
    public const string ErrorStatusDefault = "An unexpected error occurred.";

    #endregion

    #region ViewModel Error
    
    public const string EmailRequiredError = "Email is required.";
    public const string EmailInvalidError = "Invalid Email";
    public const string PasswordRequiredError = "Password is required.";
    public const string ValidPasswordMessage = "Password must contain minimum 8 characters and at least one uppercase letter, one lowercase letter, one number, and one special character (.@$!%*?&).";
    public const string NameRequiredError = "Name is required.";
    public const string NameMaxLengthError = "Name cannot be longer than 100 characters.";
    public const string EmailAlreadyExistsError = "Email already registered.";
    public const string ConfirmPasswordRequiredError = "Confirm Password is required.";
    public const string PasswordMismatchError = "Password and Confirm Password do not match.";
    public const string AssignedToPMRequiredError = "Assign PM is required.";

    #endregion

    #region Login

    public const string LoginSuccessMessage = "Login successful!";
    public const string LoginErrorMessage = "Invalid Credentials! Please try again.";

    #endregion

    #region Registration

    public const string RegistrationSuccessMessage = "Registration successful! Please log in.";
    public const string RegistrationErrorMessage = "Registration failed! Please try again.";
    
    #endregion

    #region Forgot Password

    public const string EmailNotRegisteredMessage = "Email not registered!";
    public const string InvalidEmailMessage = "Invalid email address.";
    public const string InvalidResetPasswordLinkMessage = "Invalid reset password link.";

    #endregion

    #region Reset Password

    public const string ResetPasswordEmailSubject = "Reset Password";
    public const string ResetPasswordEmailBody = "Click the link below to reset your password: <a href='{ResetLink}' target='_blank'>Reset Password</a>";
    public const string ResetPasswordSuccessMessage = "Password reset successfully! Please log in.";
    public const string ResetPasswordErrorMessage = "Failed to reset password. Please try again.";

    #endregion

    #region Project

    public const string ProjectAddedMessage = "Project added successfully!";
    public const string ProjectAddFailedMessage = "Failed to add the project.";
    public const string ProjectAlreadyExistsMessage = "Project with the same name already exists.";
    public const string ProjectNotFoundMessage = "Project not found.";
    public const string ProjectUpdatedMessage = "Project updated successfully!";
    public const string ProjectUpdateFailedMessage = "Failed to update the project.";
    public const string ProjectDeletedMessage = "Project deleted successfully!";
    public const string ProjectDeleteFailedMessage = "Failed to delete the project.";
    

    #endregion

}
