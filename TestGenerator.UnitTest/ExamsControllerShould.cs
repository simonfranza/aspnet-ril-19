using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
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
        public void Throw_ArgumentException_When_Calling_Retrieve_Questions_With_Non_Positive_Limit_Argument()
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
        public void Return_List_Of_ExamQuestion_Object_The_Same_Size_As_Parameter_List()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var controller = new ExamsController (GetFakeContext());
            var questionList = fixture.CreateMany<Question>(7).ToList();

            // Act
            var result = controller.GenerateQuestionsToExamFromQuestionListAndExam(fixture.Create<Exam>(), questionList);

            // Assert
            Assert.IsAssignableFrom<List<ExamQuestion>>(result);
            Assert.IsAssignableFrom<List<Question>>(questionList);
            Assert.Equal(questionList.Count, result.Count);
        }

        [Fact]
        public async Task Redirect_To_Index_After_Successful_Creation()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var context = GetFakeContext();
            var examDbSetMock = new Mock<DbSet<Exam>>();
            var examQuestionDbSetMock = new Mock<DbSet<ExamQuestion>>();

            context.Questions.AddRange(fixture.CreateMany<Question>(7).ToList());
            context.Exams = examDbSetMock.Object;
            context.ExamQuestions = examQuestionDbSetMock.Object;
            context.SaveChanges();

            var controller = new ExamsController (context);
            var viewModel = fixture.Create<ExamCreationViewModel>();
            viewModel.QuestionAmount = 3;

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            examDbSetMock.Verify(dbSet => dbSet.AddAsync(It.IsNotNull<Exam>(), default), Times.Once);
            examQuestionDbSetMock.Verify(dbSet => dbSet.AddRangeAsync(It.IsNotNull<ICollection<ExamQuestion>>(), default), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Exams", ((RedirectToActionResult) result).ControllerName);
            Assert.Equal("Index", ((RedirectToActionResult) result).ActionName);
        }
    }
}
