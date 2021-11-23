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
    public class AnswaresController : Controller
    {
        private readonly DBContext _context;

        public AnswaresController(DBContext context)
        {
            _context = context;
        }

        // GET: Answares
        public async Task<IActionResult> Index()
        {
            return View(await _context.Answares.ToListAsync());
        }

        // GET: Answares/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answare = await _context.Answares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (answare == null)
            {
                return NotFound();
            }

            return View(answare);
        }

        // GET: Answares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Answares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Answare answare)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answare);
        }

        // GET: Answares/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answare = await _context.Answares.FindAsync(id);
            if (answare == null)
            {
                return NotFound();
            }
            return View(answare);
        }

        // POST: Answares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID")] Answare answare)
        {
            if (id != answare.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswareExists(answare.ID))
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
            return View(answare);
        }

        // GET: Answares/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answare = await _context.Answares
                .FirstOrDefaultAsync(m => m.ID == id);
            if (answare == null)
            {
                return NotFound();
            }

            return View(answare);
        }

        // POST: Answares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var answare = await _context.Answares.FindAsync(id);
            _context.Answares.Remove(answare);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswareExists(long id)
        {
            return _context.Answares.Any(e => e.ID == id);
        }
    }
}
