using System;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Controllers;
using TestGenerator.Web.Models;
using Xunit;

namespace TestGenerator.UnitTest
{
    public class UserControllerShould
    {
        public TestGeneratorContext GetFakeContext()
        {
            var options = new DbContextOptionsBuilder<TestGeneratorContext>()
                .UseInMemoryDatabase("FakeDatabase")
                .Options;

            var context = new TestGeneratorContext(options);

            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            context.Users.Add(fixture.Create<User>());
            context.Users.Add(fixture.Create<User>());
            context.Users.Add(fixture.Create<User>());
            context.SaveChanges();

            return context;
        }


        [Fact]
        public async Task Register_Returns_Bad_Request_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);

            var controller = new UserController(GetFakeContext(), userMgrMock.Object);
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
            var context = GetFakeContext();

            var userStoreMock = Mock.Of<IUserStore<User>>();

            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
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

            var controller = new UserController(context, userMgrMock.Object);

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
            var context = GetFakeContext();

            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            userMgrMock
                .Setup(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            
            userMgrMock
                .Setup(mgr => mgr.FindByIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            userMgrMock
                .Setup(mgr => mgr.AddToRoleAsync(It.IsNotNull<User>(), "Administrator"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var controller = new UserController(context, userMgrMock.Object);
            var userViewModel = fixture.Create<UserRegistrationViewModel>();
            userViewModel.Email = "test@cesi.fr";
            
            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Contains("@cesi.fr", userViewModel.Email);
            userMgrMock.Verify(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Administrator"), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Never);
        }

        [Fact]
        public async Task Register_Creates_New_User_In_Db_Without_Administrator_Role_When_Email_Does_not_Belong_To_Cesi_Staff()
        {
            // Arrange
            var fixture = new Fixture();
            var context = GetFakeContext();

            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            userMgrMock
                .Setup(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            
            userMgrMock
                .Setup(mgr => mgr.FindByIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(fixture.Create<User>())
                .Verifiable();

            userMgrMock
                .Setup(mgr => mgr.AddToRoleAsync(It.IsNotNull<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var controller = new UserController(context, userMgrMock.Object);
            var userViewModel = fixture.Create<UserRegistrationViewModel>();
            userViewModel.Email = "test@viacesi.fr";
            
            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Contains("@viacesi.fr", userViewModel.Email);
            userMgrMock.Verify(mgr => mgr.CreateAsync(It.IsNotNull<User>(), It.IsAny<string>()), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Administrator"), Times.Never);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
        }
    }
}
