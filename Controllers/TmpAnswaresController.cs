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
    public class TmpAnswaresController : Controller
    {
        private readonly DBContext _context;

        public TmpAnswaresController(DBContext context)
        {
            _context = context;
        }

        // GET: TmpAnswares
        public async Task<IActionResult> Index()
        {
            return View(await _context.TmpAnswares.ToListAsync());
        }

        // GET: TmpAnswares/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmpAnsware = await _context.TmpAnswares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tmpAnsware == null)
            {
                return NotFound();
            }

            return View(tmpAnsware);
        }

        // GET: TmpAnswares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TmpAnswares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AnswareText,WasChecked,AttemptID")] TmpAnsware tmpAnsware)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tmpAnsware);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tmpAnsware);
        }

        // GET: TmpAnswares/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmpAnsware = await _context.TmpAnswares.FindAsync(id);
            if (tmpAnsware == null)
            {
                return NotFound();
            }
            return View(tmpAnsware);
        }

        // POST: TmpAnswares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AnswareText,WasChecked,AttemptID")] TmpAnsware tmpAnsware)
        {
            if (id != tmpAnsware.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tmpAnsware);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TmpAnswareExists(tmpAnsware.ID))
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
            return View(tmpAnsware);
        }

        // GET: TmpAnswares/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmpAnsware = await _context.TmpAnswares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tmpAnsware == null)
            {
                return NotFound();
            }

            return View(tmpAnsware);
        }

        // POST: TmpAnswares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tmpAnsware = await _context.TmpAnswares.FindAsync(id);
            _context.TmpAnswares.Remove(tmpAnsware);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TmpAnswareExists(long id)
        {
            return _context.TmpAnswares.Any(e => e.ID == id);
        }
    }
}
