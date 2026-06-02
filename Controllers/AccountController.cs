using Aklasa.Data;
using Aklasa.Helpers;
using Aklasa.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Aklasa.Controllers
{
    public class AccountController : Controller
    {
        private readonly PilkaContext _context;

        public AccountController(PilkaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hash =
                PasswordHelper.HashPassword(model.Haslo);

            var user = await _context.Uzytkownik
                .FirstOrDefaultAsync(u =>
                    u.Login == model.Login &&
                    u.HasloHash == hash);

            if (user == null)
            {
                ModelState.AddModelError("",
                    "Nieprawidłowy login lub hasło");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login)
            };

            if (user.CzyAdministrator)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        "Administrator"));
            }

            var identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

            var principal =
                new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction(
                "Index",
                "Druzyny");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(
                "Index",
                "Druzyny");
        }
    }
}