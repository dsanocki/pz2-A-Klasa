using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aklasa.Data;
using Aklasa.Models;
using Microsoft.AspNetCore.Authorization;

namespace AKlasa.Controllers
{
    public class DruzynyController : Controller
    {
        private readonly PilkaContext _context;

        public DruzynyController(PilkaContext context)
        {
            _context = context;
        }

        // GET: Druzyny
        public async Task<IActionResult> Index()
        {
            return View(
                await _context.Druzyna
                    .OrderByDescending(d => d.Punkty)
                    .ThenByDescending(d => d.BilansBramkowy)
                    .ToListAsync()
            );
        }

        // GET: Druzyny/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna
                .FirstOrDefaultAsync(m => m.IdDruzyny == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // GET: Druzyny/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Druzyny/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDruzyny,NazwaKlubu")] Druzyna druzyna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(druzyna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(druzyna);
        }

        // GET: Druzyny/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna.FindAsync(id);
            if (druzyna == null)
            {
                return NotFound();
            }
            return View(druzyna);
        }

        // POST: Druzyny/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdDruzyny,NazwaKlubu")] Druzyna druzyna)
        {
            if (id != druzyna.IdDruzyny)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbDruzyna = await _context.Druzyna
                        .FirstOrDefaultAsync(d => d.IdDruzyny == id);

                    if (dbDruzyna == null)
                    {
                        return NotFound();
                    }

                    dbDruzyna.NazwaKlubu = druzyna.NazwaKlubu;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DruzynaExists(druzyna.IdDruzyny))
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

            return View(druzyna);
        }

        // GET: Druzyny/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna
                .FirstOrDefaultAsync(m => m.IdDruzyny == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // POST: Druzyny/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var druzyna = await _context.Druzyna.FindAsync(id);
            if (druzyna != null)
            {
                _context.Druzyna.Remove(druzyna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DruzynaExists(int id)
        {
            return _context.Druzyna.Any(e => e.IdDruzyny == id);
        }
    }
}
