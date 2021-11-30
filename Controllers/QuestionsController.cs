using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sotis2.Data;
using Sotis2.Models;
using Sotis2.Models.DTO;

namespace Sotis2.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly DBContext _context;

        public QuestionsController(DBContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            QuestionWithAnswaresDTO qwaDTO = new QuestionWithAnswaresDTO
            {
                Answares = new List<Answare>
                {
                    new Answare { AnswareText = "Enter Answare here", IsItTrue = false },
                }

                
        };

            List<Subject> Subjects = _context.Subjects.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (Subject subject in Subjects)
            {
                listItems.Add(new SelectListItem
                {
                    Text = subject.NameOfSubject,
                    Value = subject.ID.ToString()
                });

            }

            qwaDTO.Subjects = listItems;

            return View("CreateQWA", qwaDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnsware(QuestionWithAnswaresDTO model)
        {
            if (ModelState.IsValid)
            {
                Question Question = new Question(model.QuestionText);
                if (model.ID == 0)
                {
                    /*
                    byte[] gb = Guid.NewGuid().ToByteArray();

                    long l = BitConverter.ToInt64(gb, 0);
                    Question.ID = l;*/
                    _context.Questions.Add(Question);
                
                    await _context.SaveChangesAsync();
                    for (int i = 0; i < model.Answares.Count; i++)
                    {
                        if (model.Answares[i].AnswareText != null && model.QuestionText != null && model.QuestionText != "" )
                        {
                            model.Answares[i].QuestionID = Question.ID;
                            _context.Answares.Add(model.Answares[i]);
                        }
                    }
                }
                else
                {
                    _context.Questions.Update(Question);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); 
                //View("Index", await _context.Questions.ToListAsync());

            }
            return View(model);
        }

        public IActionResult BlankAnsware()
        {
            return PartialView("_AnswareEditor", new Answare());
        }

        // END OF ADDING


        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,QuestionText")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // POST: Questions/FullCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> FullCreate([Bind("ID,QuestionText")] QuestionWithAnswaresDTO questionWithAnswaresDTO)
        public async Task<IActionResult> FullCreate(FormCollection collection)
        {
            return View();
            /*
            if (ModelState.IsValid)
            {
                _context.Add(questionWithAnswaresDTO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(new Question(questionWithAnswaresDTO.ID, questionWithAnswaresDTO.QuestionText, new Test()));*/
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {

            QuestionWithAnswaresDTO qwaDTO = new QuestionWithAnswaresDTO();
            Question question = _context.Questions.Find(id);
            qwaDTO.QuestionText = question.QuestionText;
            qwaDTO.ID = question.ID;

            qwaDTO.Answares =  _context.Answares.Where(x => x.QuestionID == qwaDTO.ID).ToList();

            List<Subject> Subjects = _context.Subjects.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (Subject subject in Subjects)
            {
                if (subject.ID != qwaDTO.SubjectID) 
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = subject.NameOfSubject,
                        Value = subject.ID.ToString()
                    }
                    );
                }
                else
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = subject.NameOfSubject,
                        Value = subject.ID.ToString(),
                        Selected = true
                    });
                }

            }
            qwaDTO.Subjects = listItems;


            return View("CreateQWA", qwaDTO);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,QuestionText")] Question question)
        {
            if (id != question.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.ID))
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
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(long id)
        {
            return _context.Questions.Any(e => e.ID == id);
        }
    }
}
