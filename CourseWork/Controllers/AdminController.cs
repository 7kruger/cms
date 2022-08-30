using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CourseWork.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        public AdminController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");
            return View(await db.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> UsersForm(string submitButton, int[] selectedUsers)
        {
            if (selectedUsers == null)
                return RedirectToAction("Index", "Admin");

            switch (submitButton)
            {
                case "Unlock":
                    return await Unlock(selectedUsers);
                case "Block":
                    return await Block(selectedUsers);
                case "Delete":
                    return await Delete(selectedUsers);
                case "AddToAdmin":
                    return await AddToAdmin(selectedUsers);
                case "RemoveFromAdmin":
                    return await RemoveFromAdmin(selectedUsers);
                default:
                    break;
            }

            return RedirectToAction("Index", "Admin");
        }

        private async Task<ActionResult> Unlock(int[] selectedUsers)
        {
            foreach (var u in db.Users.Where(user => selectedUsers.Contains(user.Id)))
            {
                u.Status = true;
                db.Users.Update(u);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }
        private async Task<ActionResult> AddToAdmin(int[] selectedUsers)
        {
            foreach (var u in db.Users.Where(user => selectedUsers.Contains(user.Id)))
            {
                u.RoleId = 1;
                db.Users.Update(u);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }
        private async Task<ActionResult> Block(int[] selectedUsers)
        {
            var isThereCurrentUser = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name && selectedUsers.Contains(u.Id));
            foreach (var u in db.Users.Where(user => selectedUsers.Contains(user.Id)))
            {
                u.Status = false;
                db.Users.Update(u);
            }
            await db.SaveChangesAsync();
            if (isThereCurrentUser != null)
                return RedirectToAction("Logout", "Account");

            return RedirectToAction("Index", "Admin");
        }
        private async Task<ActionResult> RemoveFromAdmin(int[] selectedUsers)
        {
            foreach (var u in db.Users.Where(user => selectedUsers.Contains(user.Id)))
            {
                u.RoleId = 2;
                db.Users.Update(u);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }
        private async Task<ActionResult> Delete(int[] selectedUsers)
        {
            var isThereCurrentUser = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name && selectedUsers.Contains(u.Id));
            foreach (var u in db.Users.Where(user => selectedUsers.Contains(user.Id)))
            {
                db.Users.Remove(u);
            }
            await db.SaveChangesAsync();
            if (isThereCurrentUser != null)
                return RedirectToAction("Logout", "Account");

            return RedirectToAction("Index", "Admin");
        }
    }
}
