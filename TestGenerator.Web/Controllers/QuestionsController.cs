using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestGenerator.Model.Data;
using TestGenerator.Model.Entities;

namespace TestGenerator.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly TestGeneratorContext _context;

        public QuestionsController(TestGeneratorContext context)
        {
            _context = context;
        }

        public List<Question> RetrieveQuestions(int limit = -1, bool random = false )
        {
            var resultList = new List<Question>();

            if (limit > 0)
            {
                if (random)
                {
                    resultList = _context.Questions
                        .OrderBy(r => Guid.NewGuid())
                        .Take(limit)
                        .ToList();
                }
                else
                {
                    resultList = _context.Questions
                        .Take(limit)
                        .ToList();
                }
            }
            else
            {
                if (random)
                {
                    resultList = _context.Questions
                        .OrderBy(r => Guid.NewGuid())
                        .ToList();
                }
                else
                {
                    resultList = _context.Questions
                        .ToList();
                }
            }

            return resultList;
        }
    }
}
