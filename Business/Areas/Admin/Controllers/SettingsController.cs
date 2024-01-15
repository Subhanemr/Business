using Business.Areas.Admin.ViewModels;
using Business.DAL;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ICollection<Settings> items = await _context.Settings.ToListAsync();
            return View(items);
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSettingsVM create)
        {
            if (!ModelState.IsValid) return View(create);
            bool result = await _context.Settings.AnyAsync(x => x.Key.Trim().ToLower() == create.Key.ToLower().Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            Settings item = new Settings
            {
                Key = create.Key,
                Value = create.Value,
            };

            await _context.Settings.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateSettingsVM update = new UpdateSettingsVM
            {
                Key = item.Key,
                Value = item.Value,
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingsVM update)
        {
            if (!ModelState.IsValid) return View(update);
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            bool result = await _context.Settings.AnyAsync(x => x.Key.Trim().ToLower() == update.Key.ToLower().Trim() && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            item.Key = update.Key;
            item.Value = update.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            _context.Settings.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
