using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NoName.Application.Abstractions.Services;
using NoName.Application.Features.Users.Commands.RegisterUser;
using NoName.Domain.Entities;

namespace NoName.Application.UnitTest.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFail_WhenEmailAlreadyExists()
    {
        var request = CreateValidCommand();

        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(new User { Email = request.Email });

        var mapperMock = new Mock<IMapper>();
        var emailServiceMock = new Mock<IEmailService>();
        var httpContextAccessor = BuildHttpContextAccessor();

        var handler = new RegisterUserCommandHandler(userManagerMock.Object, mapperMock.Object, httpContextAccessor, emailServiceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccessed);
        Assert.Equal("This email address has already been used.", result.Message);
        emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSendVerificationEmail_WhenRegistrationSuccess()
    {
        var request = CreateValidCommand();
        var mappedUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName
        };

        var userManagerMock = CreateUserManagerMock();
        userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);
        userManagerMock
            .Setup(x => x.FindByNameAsync(request.UserName))
            .ReturnsAsync((User?)null);
        userManagerMock
            .Setup(x => x.CreateAsync(mappedUser, request.Password))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock
            .Setup(x => x.AddToRoleAsync(mappedUser, "Customer"))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(mappedUser))
            .ReturnsAsync("confirm-token");

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(x => x.Map<User>(request))
            .Returns(mappedUser);

        var emailServiceMock = new Mock<IEmailService>();
        var httpContextAccessor = BuildHttpContextAccessor();

        var handler = new RegisterUserCommandHandler(userManagerMock.Object, mapperMock.Object, httpContextAccessor, emailServiceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccessed);
        Assert.Equal(mappedUser.Id, result.ResultObj);
        userManagerMock.Verify(x => x.AddToRoleAsync(mappedUser, "Customer"), Times.Once);
        emailServiceMock.Verify(
            x => x.SendEmailAsync(
                request.Email,
                "Xác nhận tài khoản NoName Shop",
                It.Is<string>(body => body.Contains("/api/auth/confirm-email?userId=") && body.Contains("token=") && body.Contains("XÁC NHẬN EMAIL NGAY"))),
            Times.Once);
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

    private static IHttpContextAccessor BuildHttpContextAccessor()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:5001");

        return new HttpContextAccessor { HttpContext = httpContext };
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
