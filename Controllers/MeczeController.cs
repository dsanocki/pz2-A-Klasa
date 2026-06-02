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
    public class MeczeController : Controller
    {
        private readonly PilkaContext _context;

        public MeczeController(PilkaContext context)
        {
            _context = context;
        }

        // GET: Mecze
        public async Task<IActionResult> Index()
        {
            var pilkaContext = _context.Mecz
                .Include(m => m.Gosc)
                .Include(m => m.Gospodarz)
                .OrderByDescending(m => m.DataMeczu)
                .ThenBy(m => m.Gospodarz!.NazwaKlubu);

            return View(await pilkaContext.ToListAsync());
        }

        // GET: Mecze/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz
                .Include(m => m.Gosc)
                .Include(m => m.Gospodarz)
                .FirstOrDefaultAsync(m => m.IdMeczu == id);
            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        // GET: Mecze/Create
        public IActionResult Create()
        {
            ViewData["GoscId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu");
            ViewData["GospodarzId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu");
            return View();
        }

        // POST: Mecze/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMeczu,GospodarzId,GoscId,BramkiGospodarzy,BramkiGosci,DataMeczu")] Mecz mecz)
        {
            if (mecz.GospodarzId == mecz.GoscId)
            {
                ModelState.AddModelError("",
                    "Drużyna nie może grać sama ze sobą.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(mecz);
                await _context.SaveChangesAsync();
                await AktualizujStatystykiDruzyn();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GoscId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GoscId);
            ViewData["GospodarzId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GospodarzId);
            return View(mecz);
        }

        // GET: Mecze/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz.FindAsync(id);
            if (mecz == null)
            {
                return NotFound();
            }
            ViewData["GoscId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GoscId);
            ViewData["GospodarzId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GospodarzId);
            return View(mecz);
        }

        // POST: Mecze/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMeczu,GospodarzId,GoscId,BramkiGospodarzy,BramkiGosci,DataMeczu")] Mecz mecz)
        {
            if (id != mecz.IdMeczu)
            {
                return NotFound();
            }

            if (mecz.GospodarzId == mecz.GoscId)
            {
                ModelState.AddModelError("",
                    "Drużyna nie może grać sama ze sobą.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mecz);
                    await _context.SaveChangesAsync();
                    await AktualizujStatystykiDruzyn();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeczExists(mecz.IdMeczu))
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
            ViewData["GoscId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GoscId);
            ViewData["GospodarzId"] = new SelectList(_context.Druzyna, "IdDruzyny", "NazwaKlubu", mecz.GospodarzId);
            return View(mecz);
        }

        // GET: Mecze/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz
                .Include(m => m.Gosc)
                .Include(m => m.Gospodarz)
                .FirstOrDefaultAsync(m => m.IdMeczu == id);
            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        // POST: Mecze/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mecz = await _context.Mecz.FindAsync(id);
            if (mecz != null)
            {
                _context.Mecz.Remove(mecz);
            }

            await _context.SaveChangesAsync();
            await AktualizujStatystykiDruzyn();
            return RedirectToAction(nameof(Index));
        }

        private bool MeczExists(int id)
        {
            return _context.Mecz.Any(e => e.IdMeczu == id);
        }


        private async Task AktualizujStatystykiDruzyn()
        {
            var druzyny = await _context.Druzyna.ToListAsync();

            foreach (var d in druzyny)
            {
                d.IloscMeczy = 0;
                d.IloscZwyciestw = 0;
                d.IloscRemisow = 0;
                d.IloscPorazek = 0;

                d.BramkiZdobyte = 0;
                d.BramkiStracone = 0;

                var mecze = await _context.Mecz
                    .Where(m =>
                        m.GospodarzId == d.IdDruzyny ||
                        m.GoscId == d.IdDruzyny)
                    .ToListAsync();

                d.IloscMeczy = mecze.Count;

                foreach (var m in mecze)
                {
                    bool gospodarz =
                        m.GospodarzId == d.IdDruzyny;

                    int zdobyte = gospodarz
                        ? m.BramkiGospodarzy
                        : m.BramkiGosci;

                    int stracone = gospodarz
                        ? m.BramkiGosci
                        : m.BramkiGospodarzy;

                    d.BramkiZdobyte += zdobyte;
                    d.BramkiStracone += stracone;

                    if (zdobyte > stracone)
                        d.IloscZwyciestw++;

                    else if (zdobyte == stracone)
                        d.IloscRemisow++;

                    else
                        d.IloscPorazek++;
                }

                d.BilansBramkowy =
                    d.BramkiZdobyte - d.BramkiStracone;

                d.Punkty =
                    d.IloscZwyciestw * 3 +
                    d.IloscRemisow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
