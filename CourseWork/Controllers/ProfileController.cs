using CourseWork.Models;
using CourseWork.Models.Entities;
using CourseWork.Models.ViewModels;
using CourseWork.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment _appEnvironment;
        public ProfileController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
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
        public async Task<ActionResult> CreateItem()
        {
            if (User.IsInRole("admin"))
            {
                ViewData["Collections"] = await db.Collections.ToListAsync();
                ViewData["Count"] = await db.Collections.CountAsync();
            }
            else
            {
                ViewData["Collections"] = await db.Collections.Where(c => c.Author == GetCurrentUser()).ToListAsync();
                ViewData["Count"] = await db.Collections.Where(c => c.Author == GetCurrentUser()).CountAsync();
            }
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
                Author = GetCurrentUser()
            };

            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();

            return Ok();
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
        public async Task<ActionResult> CreateCollection(CreateCollectionViewModel model,IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dbx = new DropboxService();
            string path = string.Empty;

            string extension = Path.GetExtension(image.FileName);
            if (extension == ".png" || extension == ".jpg")
            {
                path = await dbx.UploadFileAsync(image, image.FileName);
            }

            var collection = new Collection
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Author = GetCurrentUser(),
                Description = model.Description,
                Theme = model.Theme,
                Date = DateTime.Now,
                ImgRef = path
            };

            await db.Collections.AddAsync(collection);
            await db.SaveChangesAsync();

            return Redirect($"/Profile/EditCollection/{collection.Id}");
        }

        [HttpGet]
        public async Task<ActionResult> EditCollection(string id)
        {
            var collection = await db.Collections.Include(i => i.Items).FirstOrDefaultAsync(c => c.Id == id);

            if (collection == null)
            {
                return NotFound();
            }

            var items = collection.Items.ToList();
            var cvm = new EditCollectionViewModel
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description,
                Theme = collection.Theme,
                ImgRef = collection.ImgRef,
                Items = collection.Items.ToList()
            };
            
            items.AddRange(await db.Items.Where(i => string.IsNullOrWhiteSpace(i.CollectionId)).ToListAsync());
            ViewData["Items"] = items;

            return View(cvm);
        }
        [HttpPost]
        public async Task<ActionResult> EditCollection(EditCollectionViewModel model, string[] selectedItems, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var items = await db.Items.Where(i => selectedItems.Contains(i.Id)).ToListAsync();
            var collection = await db.Collections.Include(i => i.Items).FirstOrDefaultAsync(c => c.Id == model.Id);
            collection.Name = model.Name;
            collection.Description = model.Description;
            collection.Theme = model.Theme;
            collection.Items.Clear();
            collection.Items = items;

            if (model?.ImgRef != image?.FileName)
            {
                var dbx = new DropboxService();
                string path = string.Empty;
                string extension = Path.GetExtension(image.FileName);
                if (extension == ".png" || extension == ".jpg")
                {
                    path = await dbx.UploadFileAsync(image, image.FileName);
                }
                collection.ImgRef = path;
            }

            db.Collections.Update(collection);
            await db.SaveChangesAsync();

            return Redirect($"/Home/Collection/{collection.Id}");
        }

        public async Task<ActionResult> DeleteCollection(string id)
        {
            var collection = await db.Collections.FirstOrDefaultAsync(c => c.Id == id);

            if (collection == null)
            {
                return NotFound();
            }
            if (collection.Author == GetCurrentUser() || User.IsInRole("admin"))
            {
                db.Items.RemoveRange(await db.Items.Where(i => i.CollectionId == collection.Id).ToListAsync());
                db.Comments.RemoveRange(await db.Comments.Where(c => c.CollectionId == collection.Id).ToArrayAsync());
                db.Likes.RemoveRange(await db.Likes.Where(l => l.CollectionId == collection.Id).ToArrayAsync());

                db.Collections.Remove(collection);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }

        private string GetCurrentUser() => User.Identity.Name;
    }
}
