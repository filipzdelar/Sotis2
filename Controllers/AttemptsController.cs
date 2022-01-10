using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sotis2.Data;
using Sotis2.Models;
using Sotis2.Models.DTO;
using Sotis2.Models.Relations;
using Sotis2.Models.Users;

namespace Sotis2.Controllers
{
    public class AttemptsController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AttemptsController(DBContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
            attempt.StartTime = DateTime.Now;
            _context.Add(attempt);
            await _context.SaveChangesAsync();

            AttemptDTO attemptDTO = new AttemptDTO();
            attemptDTO.TmpQuestionDTOs = new List<TmpQuestionDTO>();

            List<Question> questionsInOrder = questionsInChaos; //OrderAsync
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

        private async Task<List<Question>> OrderAsync(List<Question> questionsInChaos)
        {
            List<Domain> domains = await _context.Domains.ToListAsync();
            List<EdgeDD> edgeDDs = await _context.EdgeDDs.ToListAsync();
            List<EdgeQD> edgeQDs = await _context.EdgeQDs.ToListAsync();

            //int[,] matrix = new int[questionsInChaos.Count + domains.Count, questionsInChaos.Count + domains.Count];
            int[,] matrix = new int[domains.Count, domains.Count];

            for (int e = 0; e < edgeDDs.Count; e++)
            {
                for (int d = 0; d < domains.Count; d++)
                {
                    if (edgeDDs[e].DomainFromID == domains[d].ID)
                    {
                        for (int d2 = 0; d2 < domains.Count; d2++)
                        {
                            if(edgeDDs[e].DomainToID == domains[d2].ID)
                            {
                                matrix[d, d2] = 1;
                            }
                        }
                    }
                }
            }

            List<int> ord = new List<int>();
            bool hasNonNegativeValues = true;
            bool hasNotZeroes = true;
            
            while(hasNonNegativeValues) 
            {
                hasNonNegativeValues = false;
                for (int d = 0; d < domains.Count; d++)
                {
                    hasNotZeroes = true;
                    for (int d2 = 0; d2 < domains.Count; d2++)
                    {
                        if(matrix[d2, d] == 1)
                        {
                            hasNonNegativeValues = true;
                            hasNotZeroes = false;
                            break;
                        }

                        if (matrix[d2, d] == 0)
                        {
                            hasNonNegativeValues = true;
                        }
                    }
                } 
            }
            
            return questionsInChaos;
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
        public async Task<IActionResult> NextAsync( AttemptDTO attemptDTO)
        {
            // [Bind("ID,TakenTime,Accuracy,Grade,StartTime,EndTime")]
            //attemptDTO.TmpSerialQuestion

            AppUser applicationUser = await _userManager.GetUserAsync(User);

            for (int i = 0; i < attemptDTO.TmpQuestionDTOs.Count(); i++)
            {
                if(attemptDTO.TmpQuestionDTOs[i].QuestionText == null)
                {
                    attemptDTO.TmpSerialQuestion += 1;
                } 
            }

            for (int k = 0; k < attemptDTO.TmpQuestionDTOs[attemptDTO.TmpSerialQuestion-1].TmpAnswaresDTO.Count(); k++)
            {
                TmpAnsware tmpAnsware = await _context.TmpAnswares.FindAsync(attemptDTO.TmpQuestionDTOs[attemptDTO.TmpSerialQuestion-1].TmpAnswaresDTO[k].ID);
                tmpAnsware.WasChecked = attemptDTO.TmpQuestionDTOs[attemptDTO.TmpSerialQuestion-1].TmpAnswaresDTO[k].WasChecked;
                _context.TmpAnswares.Update(tmpAnsware);
                await _context.SaveChangesAsync();

            }
            //answare.

            Answare answare;
            float points = 0;
            int numberOfAnswares = 0;
            if (attemptDTO.TmpSerialQuestion == attemptDTO.TotalNumberOfQuestions)
            {
                for (int q = 0; q < attemptDTO.TmpQuestionDTOs.Count(); q++)
                {
                    for (int a = 0; a < attemptDTO.TmpQuestionDTOs[q].TmpAnswaresDTO.Count(); a++)
                    {
                        answare = await _context.Answares.FindAsync(attemptDTO.TmpQuestionDTOs[q].TmpAnswaresDTO[a].AnswareID);


                        StudentsAnsware studentsAnsware = new StudentsAnsware();
                        studentsAnsware.AnswareID = attemptDTO.TmpQuestionDTOs[q].TmpAnswaresDTO[a].AnswareID;
                        studentsAnsware.AnswareText = answare.AnswareText;

                        if (attemptDTO.TmpQuestionDTOs[q].TmpAnswaresDTO[a].WasChecked == answare.IsItTrue)
                        {
                            studentsAnsware.IsItTrue = true;
                            points++;
                        }
                        else
                        {
                            studentsAnsware.IsItTrue = false;
                        }

                        _context.StudentsAnswares.Add(studentsAnsware);
                        await _context.SaveChangesAsync();

                        numberOfAnswares++;
                    }
                }

                Attempt attempt = await _context.Attempts.FindAsync(attemptDTO.TmpQuestionDTOs[0].TmpAnswaresDTO[0].AttemptID);
                attempt.Accuracy = points / numberOfAnswares;
                attempt.Name = "Filip";
                attempt.Surname = "Zdelar";
                attempt.Grade = (int) System.Math.Round(attempt.Accuracy * 10);
                attempt.EndTime = DateTime.Now;
                attempt.TakenTime = attempt.EndTime - attempt.StartTime;
                _context.Update(attempt);
                await _context.SaveChangesAsync();

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
