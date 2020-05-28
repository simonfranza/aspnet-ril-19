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

namespace TestGenerator.UnitTest
{
    public class ModulesControllerShould
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

            return context;
        }

        [Fact]
        public async Task Create_New_Module()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var context = GetFakeContext();

            var moduleDbSetMock = new Mock<DbSet<Module>>();
            moduleDbSetMock.Setup(_ => _.AddAsync(It.IsNotNull<Module>(), default)).ReturnsAsync(fixture.Create<EntityEntry<Module>>());
            context.Modules = moduleDbSetMock.Object;

            var controller = new ModulesController(context);

            // Act
            var result = await controller.Create(fixture.Create<ModuleCreationViewModel>());

            // Assert
            Assert.IsAssignableFrom<RedirectToRouteResult>(result);
            moduleDbSetMock.Verify(x => x.AddAsync(It.IsNotNull<Module>(), default), Times.Once);
        }
    }
}
