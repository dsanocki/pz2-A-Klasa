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
    public class TransferyController : Controller
    {
        private readonly PilkaContext _context;

        public TransferyController(PilkaContext context)
        {
            _context = context;
        }

        // GET: Transfery
        public async Task<IActionResult> Index()
        {
            var pilkaContext = _context.Transfer.Include(t => t.DruzynaDo).Include(t => t.DruzynaOd).Include(t => t.Zawodnik);
            return View(await pilkaContext.ToListAsync());
        }

        // GET: Transfery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer
                .Include(t => t.DruzynaDo)
                .Include(t => t.DruzynaOd)
                .Include(t => t.Zawodnik)
                .FirstOrDefaultAsync(m => m.IdTransferu == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // GET: Transfery/Create
        public IActionResult Create()
        {
            ViewData["DruzynaDoId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu");
            ViewData["DruzynaOdId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu");
            ViewData["ZawodnikId"] = new SelectList(_context.Zawodnik, "IdZawodnika", "Nazwisko");
            return View();
        }

        // POST: Transfery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTransferu,ZawodnikId,DruzynaOdId,DruzynaDoId,DataTransferu")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                var zawodnik = await _context.Zawodnik
                    .FirstOrDefaultAsync(z => z.IdZawodnika == transfer.ZawodnikId);

                if (zawodnik == null)
                    return NotFound();
                if (zawodnik.DruzynaId != transfer.DruzynaOdId)
                {
                    ModelState.AddModelError("", "Zawodnik nie jest w tej drużynie źródłowej.");
                }
                if (transfer.DruzynaOdId == transfer.DruzynaDoId)
                {
                    ModelState.AddModelError("", "Drużyna źródłowa musi być inna niż drużyna docelowa");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["DruzynaDoId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaDoId);
                    ViewData["DruzynaOdId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaOdId);
                    ViewData["ZawodnikId"] = new SelectList(_context.Zawodnik, "IdZawodnika", "Nazwisko", transfer.ZawodnikId);
                    return View(transfer);
                }
                zawodnik.DruzynaId = transfer.DruzynaDoId;
                _context.Add(transfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DruzynaDoId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaDoId);
            ViewData["DruzynaOdId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaOdId);
            ViewData["ZawodnikId"] = new SelectList(_context.Zawodnik, "IdZawodnika", "Nazwisko", transfer.ZawodnikId);
            return View(transfer);
        }

        // GET: Transfery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["DruzynaDoId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaDoId);
            ViewData["DruzynaOdId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaOdId);
            ViewData["ZawodnikId"] = new SelectList(_context.Zawodnik, "IdZawodnika", "Nazwisko", transfer.ZawodnikId);
            return View(transfer);
        }

        // POST: Transfery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTransferu,ZawodnikId,DruzynaOdId,DruzynaDoId,DataTransferu")] Transfer transfer)
        {
            if (id != transfer.IdTransferu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.IdTransferu))
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
            ViewData["DruzynaDoId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaDoId);
            ViewData["DruzynaOdId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", transfer.DruzynaOdId);
            ViewData["ZawodnikId"] = new SelectList(_context.Zawodnik, "IdZawodnika", "Nazwisko", transfer.ZawodnikId);
            return View(transfer);
        }

        // GET: Transfery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer
                .Include(t => t.DruzynaDo)
                .Include(t => t.DruzynaOd)
                .Include(t => t.Zawodnik)
                .FirstOrDefaultAsync(m => m.IdTransferu == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transfer = await _context.Transfer.FindAsync(id);
            if (transfer != null)
            {
                _context.Transfer.Remove(transfer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int id)
        {
            return _context.Transfer.Any(e => e.IdTransferu == id);
        }
    }
}
