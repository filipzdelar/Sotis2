using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.OrTools.Sat;
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
    public class AttemptsController : AttemptsControllerBase, Controller
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

        public class VarArraySolutionPrinter : CpSolverSolutionCallback
        {
            public VarArraySolutionPrinter(IntVar[] variables)
            {
                variables_ = variables;
            }

            public override void OnSolutionCallback()
            {
                {
                    Console.WriteLine(String.Format("Solution #{0}: time = {1:F2} s", solution_count_, WallTime()));
                    foreach (IntVar v in variables_)
                    {
                        Console.WriteLine(String.Format("  {0} = {1}", v.ShortString(), Value(v)));
                    }
                    solution_count_++;
                }
            }

            public int SolutionCount()
            {
                return solution_count_;
            }

            private int solution_count_;
            private IntVar[] variables_;
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
