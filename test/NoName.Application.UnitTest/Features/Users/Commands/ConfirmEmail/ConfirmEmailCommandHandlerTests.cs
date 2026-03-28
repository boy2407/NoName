using Microsoft.AspNetCore.Identity;
using Moq;
using NoName.Application.Features.Users.Commands.ConfirmEmail;
using NoName.Domain.Entities;

namespace NoName.Application.UnitTest.Features.Users.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFail_WhenUserIdOrTokenIsMissing()
    {
        var userManagerMock = CreateUserManagerMock();
        var handler = new ConfirmEmailCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new ConfirmEmailCommand { UserId = string.Empty, Token = string.Empty }, CancellationToken.None);

        Assert.False(result.IsSuccessed);
        Assert.Equal("UserId hoặc Token Invalid.", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUserNotFound()
    {
        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByIdAsync("123"))
            .ReturnsAsync((User?)null);

        var handler = new ConfirmEmailCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None);

        Assert.False(result.IsSuccessed);
        Assert.Equal("No user with this id : 123", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldPass_WhenEmailAlreadyConfirmed()
    {
        var user = new User { EmailConfirmed = true };

        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByIdAsync("123"))
            .ReturnsAsync(user);

        var handler = new ConfirmEmailCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None);

        Assert.True(result.IsSuccessed);
        Assert.True(result.ResultObj);
    }

    [Fact]
    public async Task Handle_ShouldPass_WhenConfirmEmailSuccess()
    {
        var user = new User { EmailConfirmed = false };

        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByIdAsync("123"))
            .ReturnsAsync(user);
        userManagerMock
            .Setup(x => x.ConfirmEmailAsync(user, "valid-token"))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new ConfirmEmailCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "valid-token" }, CancellationToken.None);

        Assert.True(result.IsSuccessed);
        Assert.True(result.ResultObj);
    }

    private static Mock<UserManager<User>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }
}
