using CourseWork.Models;
using CourseWork.Models.Entities;
using CourseWork.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;
        public ProfileController(ApplicationDbContext context)
        {
            db = context;
        }
        public ActionResult Index()
        {
            var createdCollectionsCount = db.Collections.Where(c => c.Author == GetCurrentUser()).Count();
            var createdItemsCount = db.Items.Where(c => c.Author == GetCurrentUser()).Count();
            var likesCount = db.Likes.Where(l => l.UserName == GetCurrentUser()).Count();
            ViewData["CollectionsCount"] = createdCollectionsCount;
            ViewData["ItemsCount"] = createdItemsCount;
            ViewData["LikesCount"] = likesCount;
            return View();
        }


        public async Task<ActionResult> MyItems()
        {
            return View(await db.Items.Where(c => c.Author == GetCurrentUser()).ToListAsync());
        }

        [HttpGet]
        public ActionResult CreateItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateItem(CreateItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var item = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Content = model.Content,
                CollectionId = model.CollectionId,
                Date = DateTime.Now,
                Author = User.Identity.Name
            };

            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }






        
        

        public async Task<ActionResult> MyCollections()
        {
            var collections = await db.Collections.Where(c => c.Author == GetCurrentUser()).ToListAsync();
            return View(collections.OrderByDescending(c => c.Date).ToList());
        }

        [HttpGet]
        public ActionResult CreateCollection()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateCollection(CreateCollectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var collection = new Collection
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Author = GetCurrentUser(),
                Description = model.Description,
                Theme = model.Theme,
                Date = DateTime.Now,
                ImgRef = ""
            };

            await db.Collections.AddAsync(collection);
            await db.SaveChangesAsync();

            return Redirect($"/Profile/EditCollection/{collection.Id}");
        }

        [HttpGet]
        public async Task<ActionResult> EditCollection(string id)
        {
            var collection = await db.Collections.Include(i => i.Items).FirstOrDefaultAsync(c => c.Id == id);
            var cvm = new EditCollectionViewModel
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description,
                Author = collection.Author,
                Theme = collection.Theme,
                Items = collection.Items.ToList()
            };
            
            return View(cvm);
        }
        [HttpPost]
        public async Task<ActionResult> EditCollection(EditCollectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var collection = new Collection
            {
                Id = model.Id,
                Name = model.Name,
                Author = model.Author,
                Description = model.Description,
                Theme = model.Theme,
                Items = model.Items
            };

            db.Update(collection);
            await db.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCollection(string collectionId)
        {
            var collection = await db.Collections.FirstOrDefaultAsync(c => c.Id == collectionId);
            if (collection.Author != GetCurrentUser() || !User.IsInRole("admin"))
            {
                return NotFound();
            }

            db.Collections.Remove(collection);
            await db.SaveChangesAsync();
            return Redirect($"/Profile/Collection?collectionId={collectionId}");
        }

        private string GetCurrentUser() => User.Identity.Name;
    }
}
