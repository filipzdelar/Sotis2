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
    public class AttemptsController : Controller
    {
        private readonly DBContext _context;

        public AttemptsController(DBContext context)
        {
            _context = context;
        }

        // GET: Attempts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attempts.ToListAsync());
        }

        // GET: Attempts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attempt = await _context.Attempts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attempt == null)
            {
                return NotFound();
            }

            return View(attempt);
        }

        // GET: Attempts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attempts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TakenTime,Accuracy,Grade,StartTime,EndTime")] Attempt attempt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attempt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attempt);
        }

        // GET: Attempts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attempt = await _context.Attempts.FindAsync(id);
            if (attempt == null)
            {
                return NotFound();
            }
            return View(attempt);
        }

        // POST: Attempts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,TakenTime,Accuracy,Grade,StartTime,EndTime")] Attempt attempt)
        {
            if (id != attempt.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attempt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttemptExists(attempt.ID))
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
            return View(attempt);
        }

        // GET: Attempts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attempt = await _context.Attempts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attempt == null)
            {
                return NotFound();
            }

            return View(attempt);
        }

        // POST: Attempts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var attempt = await _context.Attempts.FindAsync(id);
            _context.Attempts.Remove(attempt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttemptExists(long id)
        {
            return _context.Attempts.Any(e => e.ID == id);
        }


        

        public async Task<IActionResult> StartAttempt(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            List<Question> questionsInChaos = await _context.Questions.Where(x => x.Test.ID == test.ID).ToListAsync();
            AttemptDTO attemptDTO = await PreprocessQuestionsAsync(questionsInChaos);
            if (test == null)
            {
                return NotFound();
            }
            return View(attemptDTO);
        }

        private async Task<AttemptDTO> PreprocessQuestionsAsync(List<Question> questionsInChaos)
        {
            Attempt attempt = new Attempt();
            _context.Add(attempt);
            await _context.SaveChangesAsync();

            AttemptDTO attemptDTO = new AttemptDTO();
            attemptDTO.TmpQuestionDTOs = new List<TmpQuestionDTO>();

            List<Question> questionsInOrder = questionsInChaos;
            List<Answare> answares = new List<Answare>();

            foreach (Question question in questionsInOrder)
            {
                TmpQuestionDTO tmpQuestionDTO = new TmpQuestionDTO();
                tmpQuestionDTO.QuestionText = question.QuestionText;
                tmpQuestionDTO.TmpAnswaresDTO = new List<TmpAnsware>();

                answares = await _context.Answares.Where(x => x.QuestionID == question.ID).ToListAsync();

                foreach (Answare answare in answares)
                {
                    TmpAnsware tmpAnsware = new TmpAnsware();
                    tmpAnsware.AnswareText = answare.AnswareText;
                    tmpAnsware.WasChecked = false;
                    tmpAnsware.AnswareID = answare.ID;
                    tmpAnsware.AttemptID = attempt.ID;

                    _context.Add(tmpAnsware);
                    await _context.SaveChangesAsync();

                    tmpQuestionDTO.TmpAnswaresDTO.Add(tmpAnsware);
                }

                attemptDTO.TmpQuestionDTOs.Add(tmpQuestionDTO);
            }

            attemptDTO.TmpSerialQuestion = 0;
            attemptDTO.TotalNumberOfQuestions = attemptDTO.TmpQuestionDTOs.Count();

            return attemptDTO;
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Back(long id, [Bind("ID,TakenTime,Accuracy,Grade,StartTime,EndTime")] Attempt attempt)
        {
            if (id != attempt.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attempt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttemptExists(attempt.ID))
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
            return View(attempt);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Next( AttemptDTO attemptDTO)
        {
            // [Bind("ID,TakenTime,Accuracy,Grade,StartTime,EndTime")]
            //attemptDTO.TmpSerialQuestion
            for (int i = 0; i < attemptDTO.TmpQuestionDTOs.Count(); i++)
            {
                if(attemptDTO.TmpQuestionDTOs[i].QuestionText == null)
                {
                    attemptDTO.TmpSerialQuestion += 1;
                } 
            }

            Answare answare = new Answare();
            //answare.

            if(attemptDTO.TmpSerialQuestion == attemptDTO.TotalNumberOfQuestions)
            {
                return View("Complited");
            }
            else
            {
                return View("StartAttempt", attemptDTO);
            }
            //return Ok(attemptDTO);
        }
    }
}
