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
            userMgrMock.Setup(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Administrator"));
            
            var userDbSetMock = new Mock<DbSet<User>>();
            context.Users = userDbSetMock.Object;

            var controller = new UserController(context, userMgrMock.Object);

            // Act
            var result = await controller.Register(fixture.Create<UserRegistrationViewModel>());

            // Assert
            Assert.IsType<ViewResult>(result);
            userDbSetMock.Verify(x=> x.AddAsync(It.IsNotNull<User>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task Register_Creates_New_User_In_Db_With_Administrator_Role_When_Email_Belongs_To_Cesi_Staff()
        {
            // Arrange
            var fixture = new Fixture();
            var context = GetFakeContext();

            var userStoreMock = Mock.Of<IUserStore<User>>();
            var userMgrMock = new Mock<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Administrator"));

            var userDbSetMock = new Mock<DbSet<User>>();
            context.Users = userDbSetMock.Object;

            var controller = new UserController(context, userMgrMock.Object);
            var userViewModel = fixture.Create<UserRegistrationViewModel>();
            userViewModel.Email = "test@cesi.fr";
            
            // Act
            var result = await controller.Register(userViewModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Contains("@cesi.fr", userViewModel.Email);
            userDbSetMock.Verify(x=> x.AddAsync(It.IsNotNull<User>(), default(CancellationToken)), Times.Once);
            userMgrMock.Verify(mgr => mgr.AddToRoleAsync(It.IsAny<User>(), "Administrator"), Times.Once);
        }
    }
}
