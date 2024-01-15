using Business.DAL;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Blog> blogs = await _context.Blogs.Include(x => x.Author).ToListAsync(); 
            return View(blogs);
        }
    }
}
