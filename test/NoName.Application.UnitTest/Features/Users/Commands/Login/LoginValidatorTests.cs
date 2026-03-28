using NoName.Application.Features.Auth.Commands.Login;
using NoName.Application.Features.Users.Commands.Login;

namespace NoName.Application.UnitTest.Features.Users.Commands.Login;

public class LoginValidatorTests
{
    [Fact]
    public void Validate_ShouldFail_WhenUsernameIsEmpty()
    {
        var validator = new LoginValidator();
        var command = new LoginCommand(string.Empty, "123456");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Username));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUsernameTooShort()
    {
        var validator = new LoginValidator();
        var command = new LoginCommand("ab", "123456");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Username));
    }

    [Fact]
    public void Validate_ShouldFail_WhenPasswordIsEmpty()
    {
        var validator = new LoginValidator();
        var command = new LoginCommand("boy2407", string.Empty);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Password));
    }

    [Fact]
    public void Validate_ShouldFail_WhenPasswordTooShort()
    {
        var validator = new LoginValidator();
        var command = new LoginCommand("boy2407", "123");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Password));
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var validator = new LoginValidator();
        var command = new LoginCommand("boy2407", "123456");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
