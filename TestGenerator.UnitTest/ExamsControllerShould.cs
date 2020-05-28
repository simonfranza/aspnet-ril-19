using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
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
    public class ExamsControllerShould
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
        public async Task Return_Bad_Request_Result_When_ModelState_Is_Invalid_On_Create_Post()
        {
            // Arrange
            var controller = new ExamsController(GetFakeContext());
            controller.ModelState.AddModelError("Name", "Required");
            var viewModel = new ExamCreationViewModel();

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Retrieve_X_Questions_When_User_Inputs_X_To_The_QuestionsAmount_Field()
        {
            // Arrange
            var fixture = new Fixture();
            var context = GetFakeContext();
            var questionsDbSetMock = new Mock<DbSet<Question>>();

            context.Questions = questionsDbSetMock.Object;

            var viewModel = fixture.Create<ExamCreationViewModel>();
            viewModel.QuestionAmount = 12;

            var controller = new ExamsController(GetFakeContext());

            // Act
            await controller.Create(viewModel);

            // Assert
            questionsDbSetMock.Verify(dbSet => dbSet.OrderBy(It.IsAny<Func<Question, It.IsAnyType>>()).Take(12).ToList(), Times.Never);
        }
    }
}
