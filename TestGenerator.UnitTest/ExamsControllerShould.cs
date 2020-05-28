using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public void Throws_ArgumentException_When_Calling_Retrieve_Questions_With_Non_Positive_Limit_Argument()
        {
            // Arrange
            var controller = new ExamsController (GetFakeContext());

            // Act
            var positiveResult = controller.RetrieveQuestions(1);

            // Assert
            Assert.IsAssignableFrom<List<Question>>(positiveResult);
            Assert.Throws<ArgumentException>(() => controller.RetrieveQuestions(0));
            Assert.Throws<ArgumentException>(() => controller.RetrieveQuestions(-1));
        }

        [Fact]
        public async Task Retrieve_X_Questions_When_User_Inputs_X_To_The_QuestionsAmount_Field()
        {
            // Arrange
            var fixture = new Fixture();
            var context = GetFakeContext();
            context.Questions.AddRange(fixture.CreateMany<Question>(15));
            context.SaveChanges();

            var controllerMock = new Mock<ExamsController>(context);

            controllerMock
                .Setup(ctrl => ctrl.RetrieveQuestions(12))
                .Returns(Mock.Of<List<Question>>())
                .Verifiable();

            var viewModel = fixture.Create<ExamCreationViewModel>();
            viewModel.QuestionAmount = 12;

            // Act
            await controllerMock.Object.Create(viewModel);

            // Assert
            controllerMock.Verify(c => c.RetrieveQuestions(12), Times.Once);
        }
    }
}
