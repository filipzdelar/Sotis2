using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sotis2.Data;
using Sotis2.Models;

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
    }
}
