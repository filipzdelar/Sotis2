using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sotis2.Data;
using Sotis2.Models;
using Sotis2.Models.DTO;

namespace Sotis2.Controllers
{
    public class TestsController : Controller
    {
        private readonly DBContext _context;

        public TestsController(DBContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tests.ToListAsync());
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);



            List<Question> questions = _context.Questions.Where(x => x.Test.ID == test.ID).ToList();
            TestQuestionAnswerDTO dTO = new TestQuestionAnswerDTO();
            dTO.Id = test.ID;
            dTO.TestDuration = test.TestDuration;
            dTO.qWA = new List<QuestionWithAnswaresDTO>();
            for (int i = 0; i < questions.Count(); i++)
            {
                List<Answare> answares = _context.Answares.Where(x => x.QuestionID == questions[i].ID).ToList();
                dTO.qWA.Add(new QuestionWithAnswaresDTO(answares, questions[i].QuestionText));
            }

            if (test == null)
            {
                return NotFound();
            }

            return View(dTO);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            TestDTO testDTO = new TestDTO();
            testDTO.Questions = new List<Question>();

            Question question = new Question();
            question.QuestionText = "EnterText here... ";
            testDTO.Questions.Add(question);
            return View(testDTO);
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TestDuration")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        // GET: Tests/Create
        /*
        public IActionResult CreateFullTest()
        {
            TestQuestionAnswerDTO testDTO = new TestQuestionAnswerDTO();
            testDTO.QuestionsDTO = new List<QuestionDTO>();

            QuestionDTO question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            return View(testDTO);
        }
        */
        private static QuestionWithAnswaresDTO CreateTmpQuestion()
        {
            QuestionWithAnswaresDTO question = new QuestionWithAnswaresDTO();
            question.QuestionText = "Enter Question here... ";
            question.Answares = new List<Answare>();

            Answare answare = new Answare();
            answare.AnswareText = "Answare text 1";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 2";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 3";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 4";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 5";
            question.Answares.Add(answare);
            return question;
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFullTest([Bind("ID,TestDuration")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }*/


        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,TestDuration")] Test test)
        {
            if (id != test.ID)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        public IActionResult BlankAnsware()
        {
            return PartialView("_AnswareEditor", new Answare());
        }

        
        public ActionResult AddAnsware(int? k, int? i)
        {
            ViewBag.k = i;
            ViewBag.i = i;
            return PartialView();
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(long id)
        {
            return _context.Tests.Any(e => e.ID == id);
        }


        public IActionResult AddQuestion([Bind("ID,TestDuration")] TestDTO testDTO)
        {

            return View(testDTO);
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>


        // GET: Questions/Create
        public IActionResult CreateFullTest()
        {

            TestQuestionAnswerDTO testDTO = new TestQuestionAnswerDTO();
            testDTO.qWA = new List<QuestionWithAnswaresDTO>();

            QuestionWithAnswaresDTO question = CreateTmpQuestion();
            question.ID = 1;
            testDTO.qWA.Add(question);

            question = CreateTmpQuestion();
            question.ID = 2;
            testDTO.qWA.Add(question);

            question = CreateTmpQuestion();
            question.ID = 3;
            testDTO.qWA.Add(question);

            return View(testDTO);
        }

        /// <param name="question"></param>
        /// <returns></returns>
        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFullTest(TestQuestionAnswerDTO question)
        {
            //[Bind("ID,QuestionText,AnswareText")]  , [Bind("AnswareText")] string AnswareText
            if (ModelState.IsValid)
            {
                Test t = new Test(question.TestDuration);
                
                _context.Tests.Add(t);

                await _context.SaveChangesAsync();
                for (int i = 0; i < question.qWA.Count(); i++)
                {
                    Question q = new Question(t, question.qWA[i].QuestionText);
                    _context.Questions.Add(q);
                    await _context.SaveChangesAsync();

                    for (int j = 0; j < question.qWA[i].Answares.Count(); j++)
                    {
                        _context.Answares.Add(new Answare(question.qWA[i].Answares[j].AnswareText, question.qWA[i].Answares[j].IsItTrue, q.ID));
                    }
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public ActionResult QuestionEditor(int? i)
        {
            ViewBag.i = i;
            QuestionWithAnswaresDTO questionWithAnswaresDTO = new QuestionWithAnswaresDTO();
            questionWithAnswaresDTO.QuestionText = "Enter question here ";
            questionWithAnswaresDTO.ID = ((int)i) + 1;
            questionWithAnswaresDTO.Answares = new List<Answare>();
            questionWithAnswaresDTO.Answares.Append(new Answare("Odgovori", true));
            return PartialView("QuestionEditor", questionWithAnswaresDTO);
        }
    }
}
