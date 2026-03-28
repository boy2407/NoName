using Microsoft.AspNetCore.Identity;
using Moq;
using NoName.Application.Features.Users.Commands.Logout;
using NoName.Domain.Entities;

namespace NoName.Application.UnitTest.Features.Users.Commands.Logout;

public class LogoutCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFail_WhenUserDoesNotExist()
    {
        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByNameAsync("missing-user"))
            .ReturnsAsync((User?)null);

        var handler = new LogoutCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new LogoutCommand { Username = "missing-user" }, CancellationToken.None);

        Assert.False(result.IsSuccessed);
        Assert.Equal("Invalid User .", result.Message);
        userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldClearRefreshTokenAndUpdateUser_WhenUserExists()
    {
        var user = new User
        {
            UserName = "boy2407",
            RefreshToken = "old-refresh-token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
        };

        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByNameAsync(user.UserName!))
            .ReturnsAsync(user);
        userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new LogoutCommandHandler(userManagerMock.Object);

        var result = await handler.Handle(new LogoutCommand { Username = user.UserName! }, CancellationToken.None);

        Assert.True(result.IsSuccessed);
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
        userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
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
