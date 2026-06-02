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
    public class ZawodnicyController : Controller
    {
        private readonly PilkaContext _context;

        public ZawodnicyController(PilkaContext context)
        {
            _context = context;
        }

        // GET: Zawodnicy
        public async Task<IActionResult> Index()
        {
            var pilkaContext = _context.Zawodnik.Include(z => z.Druzyna);
            return View(await pilkaContext.ToListAsync());
        }

        // GET: Zawodnicy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.Druzyna)
                .FirstOrDefaultAsync(m => m.IdZawodnika == id);
            if (zawodnik == null)
            {
                return NotFound();
            }

            return View(zawodnik);
        }

        // GET: Zawodnicy/Create
        public IActionResult Create()
        {
            ViewData["DruzynaId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu");
            return View();
        }

        // POST: Zawodnicy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdZawodnika,Imie,Nazwisko,IloscBramek,IloscAsyst,DruzynaId")] Zawodnik zawodnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zawodnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DruzynaId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", zawodnik.DruzynaId);
            return View(zawodnik);
        }

        // GET: Zawodnicy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik.FindAsync(id);
            if (zawodnik == null)
            {
                return NotFound();
            }
            ViewData["DruzynaId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", zawodnik.DruzynaId);
            return View(zawodnik);
        }

        // POST: Zawodnicy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdZawodnika,Imie,Nazwisko,IloscBramek,IloscAsyst,DruzynaId")] Zawodnik zawodnik)
        {
            if (id != zawodnik.IdZawodnika)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zawodnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZawodnikExists(zawodnik.IdZawodnika))
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
            ViewData["DruzynaId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", zawodnik.DruzynaId);
            return View(zawodnik);
        }

        // GET: Zawodnicy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.Druzyna)
                .FirstOrDefaultAsync(m => m.IdZawodnika == id);
            if (zawodnik == null)
            {
                return NotFound();
            }

            return View(zawodnik);
        }

        // POST: Zawodnicy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zawodnik = await _context.Zawodnik.FindAsync(id);
            if (zawodnik != null)
            {
                _context.Zawodnik.Remove(zawodnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZawodnikExists(int id)
        {
            return _context.Zawodnik.Any(e => e.IdZawodnika == id);
        }
    }
}
