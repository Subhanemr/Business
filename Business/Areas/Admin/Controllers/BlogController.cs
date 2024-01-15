using Business.Areas.Admin.ViewModels;
using Business.DAL;
using Business.Models;
using Business.Utilities.Extentions;
using Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles ="Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Blogs.CountAsync();
            ICollection<Blog> items = await _context.Blogs.Skip((page -1)*4).Take(4)
                .Include(x => x.Author).ToListAsync();

            PaginationVM<Blog> vM = new PaginationVM<Blog>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / 4),
                Items = items
            };
            return View(vM);
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create()
        {
            CreateBlogVM create = new CreateBlogVM
            {
                Authors = await _context.Authors.ToListAsync()
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Authors = await _context.Authors.ToListAsync();
                return View(create);
            }
            bool result = await _context.Blogs.AnyAsync(x => x.Title.Trim().ToLower() == create.Title.ToLower().Trim());
            if (result)
            {
                create.Authors = await _context.Authors.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                create.Authors = await _context.Authors.ToListAsync();
                ModelState.AddModelError("Photo", "Not valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                create.Authors = await _context.Authors.ToListAsync();
                ModelState.AddModelError("Photo", "Limit size is 10MB");
                return View(create);
            }
            Blog item = new Blog
            {
                Title = create.Title,
                SubTitle = create.SubTitle,
                AuthorId = create.AuthorId,
                Img = await create.Photo.CrateFileAsync(_env.WebRootPath, "assets", "images")
            };

            await _context.Blogs.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Blog item = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateBlogVM update = new UpdateBlogVM
            {
                Title = item.Title,
                SubTitle = item.SubTitle,
                Authors = await _context.Authors.ToListAsync(),
                AuthorId = item.AuthorId,
                Img = item.Img
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBlogVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Authors = await _context.Authors.ToListAsync();
                return View(update);
            }
            Blog item = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            bool result = await _context.Blogs.AnyAsync(x => x.Title.Trim().ToLower() == update.Title.ToLower().Trim() && x.Id != id);
            if (result)
            {
                update.Authors = await _context.Authors.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            if (update.Photo != null)
            {
                if (!update.Photo.IsValid())
                {
                    update.Authors = await _context.Authors.ToListAsync();
                    ModelState.AddModelError("Photo", "Not valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    update.Authors = await _context.Authors.ToListAsync();
                    ModelState.AddModelError("Photo", "Limit size is 10MB");
                    return View(update);
                }
                item.Img.DeleteAsync(_env.WebRootPath, "assets", "images");
                item.Img = await update.Photo.CrateFileAsync(_env.WebRootPath, "assets", "images");
            }
            item.Title = update.Title;
            item.SubTitle = update.SubTitle;
            item.AuthorId = update.AuthorId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Blog item = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            item.Img.DeleteAsync(_env.WebRootPath, "assets", "images");
            _context.Blogs.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
