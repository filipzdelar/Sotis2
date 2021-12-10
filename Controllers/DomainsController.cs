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
    public class DomainsController : Controller
    {
        private readonly DBContext _context;

        public DomainsController(DBContext context)
        {
            _context = context;
        }

        // GET: Domains
        public async Task<IActionResult> Index()
        {
            return View(await _context.Domains.ToListAsync());
        }

        // GET: Domains/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domains
                .FirstOrDefaultAsync(m => m.ID == id);
            if (domain == null)
            {
                return NotFound();
            }

            return View(domain);
        }

        // GET: Domains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Domains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Type")] Domain domain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(domain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domain);
        }

        // GET: Domains/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domains.FindAsync(id);
            if (domain == null)
            {
                return NotFound();
            }
            return View(domain);
        }

        // POST: Domains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Type")] Domain domain)
        {
            if (id != domain.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomainExists(domain.ID))
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
            return View(domain);
        }

        // GET: Domains/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domains
                .FirstOrDefaultAsync(m => m.ID == id);
            if (domain == null)
            {
                return NotFound();
            }

            return View(domain);
        }

        // POST: Domains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var domain = await _context.Domains.FindAsync(id);
            _context.Domains.Remove(domain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DomainExists(long id)
        {
            return _context.Domains.Any(e => e.ID == id);
        }
    }
}
