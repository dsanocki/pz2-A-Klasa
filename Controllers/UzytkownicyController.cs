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
using Aklasa.Helpers;


namespace AKlasa.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UzytkownicyController : Controller
    {
        private readonly PilkaContext _context;

        public UzytkownicyController(PilkaContext context)
        {
            _context = context;
        }

        // GET: Uzytkownicy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uzytkownik.ToListAsync());
        }

        // GET: Uzytkownicy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // GET: Uzytkownicy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uzytkownicy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Haslo,CzyAdministrator")] Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                uzytkownik.HasloHash =
                    PasswordHelper.HashPassword(uzytkownik.Haslo!);

                _context.Add(uzytkownik);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(uzytkownik);
        }

        // GET: Uzytkownicy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            return View(uzytkownik);
        }

        // POST: Uzytkownicy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Login,CzyAdministrator")]
            Uzytkownik uzytkownik)
        {
            if (id != uzytkownik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dbUser = await _context.Uzytkownik
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (dbUser == null)
                {
                    return NotFound();
                }

                dbUser.Login = uzytkownik.Login;
                dbUser.CzyAdministrator = uzytkownik.CzyAdministrator;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(uzytkownik);
        }

        // GET: Uzytkownicy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // POST: Uzytkownicy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uzytkownik = await _context.Uzytkownik.FindAsync(id);

            if (User.Identity?.Name == uzytkownik?.Login)
            {
                return BadRequest(
                    "Nie możesz usunąć aktualnie zalogowanego użytkownika.");
            }
            if (uzytkownik != null)
            {
                _context.Uzytkownik.Remove(uzytkownik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UzytkownikExists(int id)
        {
            return _context.Uzytkownik.Any(e => e.Id == id);
        }
    }
}
