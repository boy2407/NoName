using NoName.Application.Features.Users.Commands.RegisterUser;

namespace NoName.Application.UnitTest.Features.Users.Commands.RegisterUser;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Validate_ShouldFail_WhenFirstNameIsEmpty()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();
        command.FirstName = string.Empty;

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterUserCommand.FirstName));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUserNameTooShort()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();
        command.UserName = "abc";

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterUserCommand.UserName));
    }

    [Fact]
    public void Validate_ShouldFail_WhenEmailIsInvalid()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();
        command.Email = "invalid-email";

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterUserCommand.Email));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDobUnderMinimumAge()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();
        command.Dob = DateTime.Now.AddYears(-15);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterUserCommand.Dob));
    }

    [Fact]
    public void Validate_ShouldFail_WhenConfirmPasswordDoesNotMatch()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();
        command.ConfirmPassword = "654321";

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterUserCommand.ConfirmPassword));
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var validator = new RegisterUserValidator();
        var command = CreateValidCommand();

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    private static RegisterUserCommand CreateValidCommand()
    {
        return new RegisterUserCommand
        {
            FirstName = "Boy",
            LastName = "Nguyen",
            UserName = "boy2407",
            Email = "boy2407@gmail.com",
            Password = "123456",
            ConfirmPassword = "123456",
            Dob = DateTime.Now.AddYears(-20)
        };
    }
}
