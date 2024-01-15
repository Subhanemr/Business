using Business.Areas.Admin.ViewModels;
using Business.DAL;
using Business.Models;
using Business.Utilities.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Business.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ICollection<Author> items = await _context.Authors.Include(x => x.Blogs).ToListAsync();
            return View(items);
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorVM create)
        {
            if (!ModelState.IsValid) return View(create);
            bool result = await _context.Authors.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.ToLower().Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                ModelState.AddModelError("Photo", "Not valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                ModelState.AddModelError("Photo", "Limit size is 10MB");
                return View(create);
            }
            Author item = new Author
            {
                Name = create.Name,
                Surname = create.Surname,
                Img = await create.Photo.CrateFileAsync(_env.WebRootPath, "assets", "images")
            };

            await _context.Authors.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Author item = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateAuthorVM update = new UpdateAuthorVM
            {
                Name = item.Name,
                Surname = item.Surname,
                Img = item.Img
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateAuthorVM update)
        {
            if (!ModelState.IsValid) return View(update);
            Author item = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            bool result = await _context.Authors.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.ToLower().Trim() && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            if (update.Photo != null)
            {
                if (!update.Photo.IsValid())
                {
                    ModelState.AddModelError("Photo", "Not valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    ModelState.AddModelError("Photo", "Limit size is 10MB");
                    return View(update);
                }
                item.Img.DeleteAsync(_env.WebRootPath, "assets", "images");
                item.Img = await update.Photo.CrateFileAsync(_env.WebRootPath, "assets", "images");
            }
            item.Name = update.Name;
            item.Surname = update.Surname;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Author item = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            item.Img.DeleteAsync(_env.WebRootPath, "assets", "images");
            _context.Authors.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
