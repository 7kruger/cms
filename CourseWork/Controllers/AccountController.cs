using CourseWork.Models;
using CourseWork.Models.Entities;
using CourseWork.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace CourseWork.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
                if (user == null)
                {
                    user = new User 
                    {
                        Name = model.Name, 
                        Password = model.Password, 
                        RegistrationDate = DateTime.Now,
                        Status = true
                    };
                    Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователя не существует");
                    return View(user);
                }
                if (!user.Status)
                {
                    ModelState.AddModelError(string.Empty, "Вы были заблокированы");
                    return View();
                }
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
