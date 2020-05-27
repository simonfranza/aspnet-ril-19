using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            var controller = new UserController(GetFakeContext());
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
            var context = GetFakeContext();
            var initialAmount = context.Users.Count();
            var fixture = new Fixture();
            var controller = new UserController(context);

            // Act
            var result = await controller.Register(fixture.Create<UserRegistrationViewModel>());

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(context.Users.Count(), initialAmount + 1);
        }
    }
}
