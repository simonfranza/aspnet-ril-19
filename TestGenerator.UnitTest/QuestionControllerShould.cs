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
        public async Task Create_New_Single_Choice_Question()
        {
            // Arrange
            var context = GetFakeContext();

            var questionDbSetMock = new Mock<DbSet<Question>>();
            context.Questions = questionDbSetMock.Object;

            var controller = new QuestionsController(context);

            List<Answer> answers = new List<Answer>() {
                new Answer {
                    QuestionId = 3,
                    IsValid = true,
                    Text = "Yes"
                }, new Answer
                {
                    QuestionId = 3,
                    IsValid = false,
                    Text = "No"
                }, new Answer
                {
                    QuestionId = 3,
                    IsValid = false,
                    Text = "No"
                }
            };
            var question = new QuestionCreationViewModel
            {
                
                Text = "Is this a valid question?",
                QuestionType = QuestionTypeEnum.SingleChoice,
                Answers = answers
            };

            // Act
            var result = await controller.Create(question);

            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var questionModel = Assert.IsAssignableFrom<Question>(viewResult.Model);
            Assert.Equal(QuestionTypeEnum.SingleChoice, questionModel.QuestionType);
            Assert.True(questionModel.Answers.Count > 2);
        }

        [Fact]
        public async Task Create_New_Multiple_Choice_Question()
        {
            // Arrange
            var context = GetFakeContext();

            var questionDbSetMock = new Mock<DbSet<Question>>();
            context.Questions = questionDbSetMock.Object;

            var controller = new QuestionsController(context);

            List<Answer> answers = new List<Answer>() {
                new Answer {
                    QuestionId = 2,
                    IsValid = true,
                    Text = "Yes"
                }, new Answer
                {
                    QuestionId = 2,
                    IsValid = false,
                    Text = "No"
                }, new Answer
                {
                    QuestionId = 2,
                    IsValid = false,
                    Text = "No"
                }
            };
            var question = new QuestionCreationViewModel
            {

                Text = "Is this a valid question?",
                QuestionType = QuestionTypeEnum.MultipleChoice,
                Answers = answers
            };

            // Act
            var result = await controller.Create(question);

            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var questionModel = Assert.IsAssignableFrom<Question>(viewResult.Model);
            Assert.Equal(QuestionTypeEnum.MultipleChoice, questionModel.QuestionType);
            Assert.True(questionModel.Answers.Count > 2);
        }

        [Fact]
        public async Task Create_New_Yes_No_Choice_Question()
        {
            // Arrange
            var context = GetFakeContext();

            var questionDbSetMock = new Mock<DbSet<Question>>();
            context.Questions = questionDbSetMock.Object;

            var controller = new QuestionsController(context);

            List<Answer> answers = new List<Answer>() {
                new Answer {
                    QuestionId = 1,
                    IsValid = true,
                    Text = "Yes"
                }, new Answer
                {
                    QuestionId = 1,
                    IsValid = false,
                    Text = "No"
                }
            };
            var question = new QuestionCreationViewModel
            {

                Text = "Is this a valid question?",
                QuestionType = QuestionTypeEnum.YesNo,
                Answers = answers
            };

            // Act
            var result = await controller.Create(question);

            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var questionModel = Assert.IsAssignableFrom<Question>(viewResult.Model);
            Assert.Equal(QuestionTypeEnum.YesNo, questionModel.QuestionType);
            Assert.Equal(2, questionModel.Answers.Count);
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
