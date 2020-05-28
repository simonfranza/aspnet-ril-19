using System;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Controllers;
using TestGenerator.Web.Models;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TestGenerator.UnitTest
{
    public class UserControllerShould
    {
        public Mock<SignInManager<User>> GetFakeSignInManager(Mock<UserManager<User>> mockedUserManager)
        {
            return new Mock<SignInManager<User>>(
                mockedUserManager.Object,
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object
            );
        }

        public Mock<IHttpContextAccessor> GetFakeHttpContextAccessor()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenantId = "abcd";
            context.Request.Headers["Tenant-ID"] = fakeTenantId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockHttpContextAccessor;
        }

        [Fact]
        public async Task Register_Returns_Bad_Request_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);
            
            

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);
            controller.ModelState.AddModelError("Email", "Required");
            var userViewModel = new UserRegistrationViewModel();

            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Register_Creates_New_User_In_Db()
        {
            // Arrange
            var fixture = new Fixture();
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            userMgrMock
                .Setup(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            
            userMgrMock
                .Setup(mgr => mgr.FindByIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            userMgrMock
                .Setup(mgr => mgr.AddToRoleAsync(It.IsNotNull<User>(), It.IsNotNull<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);

            // Act
            var result = await controller.Register(fixture.Create<UserRegistrationViewModel>());

            // Assert
            Assert.IsType<ViewResult>(result);
            userMgrMock.Verify(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Register_Creates_New_User_In_Db_With_Administrator_Role_When_Email_Belongs_To_Cesi_Staff()
        {
            // Arrange
            var fixture = new Fixture();

            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            userMgrMock
                .Setup(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            
            userMgrMock
                .Setup(mgr => mgr.FindByIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            userMgrMock
                .Setup(mgr => mgr.AddToRoleAsync(It.IsNotNull<User>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);
            var userViewModel = fixture.Create<UserRegistrationViewModel>();
            userViewModel.Email = "test@cesi.fr";
            
            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Contains("@cesi.fr", userViewModel.Email);
            userMgrMock.Verify(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Admin"), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Never);
        }

        [Fact]
        public async Task Register_Creates_New_User_In_Db_Without_Administrator_Role_When_Email_Does_not_Belong_To_Cesi_Staff()
        {
            // Arrange
            var fixture = new Fixture();
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            userMgrMock
                .Setup(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            
            userMgrMock
                .Setup(mgr => mgr.FindByIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);
            var userViewModel = fixture.Create<UserRegistrationViewModel>();
            userViewModel.Email = "test@viacesi.fr";
            
            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Contains("@viacesi.fr", userViewModel.Email);
            userMgrMock.Verify(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Admin"), Times.Never);
        }

        [Fact]
        public async Task Login_Should_Display_Error_Message_When_ModelState_Is_Invalid()
        {
            // Arrange
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);
            controller.ModelState.AddModelError("Email", "Required");
            var userViewModel = new UserLoginViewModel();

            // Act
            await controller.Login(userViewModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.Contains("Global", modelState.Keys);
            Assert.True(modelState["Global"].Errors.Count > 0);
            Assert.Equal("Identifiants erronés.", modelState["Global"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Login_Should_Display_Error_Message_When_Bad_Credentials_Inputted()
        {
            // Arrange
            var fixture = new Fixture();
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            signinMgrMock
                .Setup(mgr => mgr.PasswordSignInAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed)
                .Verifiable();

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);

            // Act
            await controller.Login(fixture.Create<UserLoginViewModel>());

            // Assert
            var modelState = controller.ModelState;
            Assert.Contains("Global", modelState.Keys);
            Assert.True(modelState["Global"].Errors.Count > 0);
            Assert.Equal("Identifiants erronés.", modelState["Global"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Login_Should_Redirect_To_Home_Index_On_Success()
        {
            // Arrange
            var fixture = new Fixture();
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            var signinMgrMock = GetFakeSignInManager(userMgrMock);

            userMgrMock
                .Setup(mgr => mgr.FindByEmailAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            signinMgrMock
                .Setup(mgr => mgr.PasswordSignInAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success)
                .Verifiable();

            var controller = new UserController(userMgrMock.Object, signinMgrMock.Object, GetFakeHttpContextAccessor().Object);

            // Act
            var result = await controller.Login(fixture.Create<UserLoginViewModel>()) as RedirectToActionResult;

            // Assert
            signinMgrMock.Verify(mgr => mgr.PasswordSignInAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
