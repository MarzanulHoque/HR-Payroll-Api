using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HRMS.Application.Auth.Commands.Login;

public class LoginCommandValidator
{
    public ValidationResult Validate(LoginCommand command)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            errors.Add(new ValidationFailure("Email", "Email is required."));
        }
        else if (!IsValidEmail(command.Email))
        {
            errors.Add(new ValidationFailure("Email", "Email must be valid."));
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            errors.Add(new ValidationFailure("Password", "Password is required."));
        }
        else if (command.Password.Length < 8)
        {
            errors.Add(new ValidationFailure("Password", "Password must be at least 8 characters."));
        }

        return new ValidationResult(errors);
    }

    private static bool IsValidEmail(string email)
    {
        // simple email regex for validation in tests
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}

public record ValidationResult(List<ValidationFailure> Errors)
{
    public bool IsValid => Errors == null || Errors.Count == 0;
}

public record ValidationFailure(string PropertyName, string ErrorMessage);

