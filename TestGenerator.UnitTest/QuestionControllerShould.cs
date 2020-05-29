using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestGenerator.Model.Constants;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;
using TestGenerator.Web.Controllers;
using TestGenerator.Web.Models;
using Xunit;

namespace TestGenerator.UnitTest
{
    public class QuestionControllerShould
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
        public async Task Create_New_Question()
        {
            // Arrange
            var fixture = new Fixture();
            var context = GetFakeContext();

            var questionDbSetMock = new Mock<DbSet<Question>>();
            var answerDbSetMock = new Mock<DbSet<Answer>>();
            context.Questions = questionDbSetMock.Object;
            context.Answers = answerDbSetMock.Object;

            var controller = new QuestionsController(context);

            // Act
            var result = await controller.Create(fixture.Create<QuestionCreationViewModel>());

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            questionDbSetMock.Verify(x => x.AddAsync(It.IsNotNull<Question>(), default), Times.Once);
            answerDbSetMock.Verify(x => x.AddRangeAsync(It.IsNotNull<List<Answer>>(), default), Times.Once);
        }

        [Fact]
        public void Return_Question_By_Id()
        {

        }

        [Fact]
        public void Return_Question_By_Exam_Id()
        {

        }

        [Fact]
        public void Validate_Question()
        {

        }

        [Fact]
        public void Delete_Question()
        {

        }
    }
}
