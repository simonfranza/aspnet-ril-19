using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoFixture;
using TestGenerator.Model.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Controllers;
using TestGenerator.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoFixture.AutoMoq;
using System.Threading;

namespace TestGenerator.UnitTest
{
    public class ModulesControllerShould
    {
        public TestGeneratorContext GetFakeContext()
        {
            var options = new DbContextOptionsBuilder<TestGeneratorContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TestGeneratorContext(options);

            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return context;
        }

        [Fact]
        public async Task Create_New_Module()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var context = GetFakeContext();

            var moduleDbSetMock = new Mock<DbSet<Module>>();

            context.Modules = moduleDbSetMock.Object;

            var controller = new ModulesController(context);

            // Act
            var result = await controller.Create(fixture.Create<ModuleCreationViewModel>());

            // Assert
            Assert.IsAssignableFrom<RedirectToRouteResult>(result);
            moduleDbSetMock.Verify(x => x.AddAsync(It.IsNotNull<Module>(), default), Times.Once);
        }

        [Fact]
        public async Task Return_Bad_Request_On_Create_With_Invalid_Model()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var context = GetFakeContext();

            var moduleDbSetMock = new Mock<DbSet<Module>>();

            context.Modules = moduleDbSetMock.Object;

            var controller = new ModulesController(context);
            controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await controller.Create(fixture.Create<ModuleCreationViewModel>());

            // Assert
            var badRequestResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Return_Module_Details()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var module = fixture.Create<Module>();

            var context = GetFakeContext();
            context.Modules.RemoveRange(context.Modules.ToList());
            context.SaveChanges();
            context.Modules.Add(module);
            context.SaveChanges();

            var controller = new ModulesController(context);

            // Act
            var result = await controller.Details(module.ModuleId);

            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task Return_Not_Found_On_Null_Id()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var context = GetFakeContext();

            var module = fixture.Create<Module>();

            context.Modules.Add(module);
            context.SaveChanges();

            var controller = new ModulesController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task Return_Not_Found_On_Invalid_Id()
        {
            // Arrange
            var controller = new ModulesController(GetFakeContext());

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
    }
}
