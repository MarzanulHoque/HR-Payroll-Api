using HRMS.Application.Auth.Commands.Login;
using Xunit;

namespace HRMS.Application.UnitTests.Auth.Login;

public class LoginCommandValidatorTests
{
    [Fact]
    public void Validate_InvalidEmailAndPassword_ReturnsErrors()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("not-an-email", "short");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validate_ValidInput_Passes()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("user@example.com", "longenoughpassword");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }
}
