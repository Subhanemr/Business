using Business.Models;
using Business.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.DAL
{
    public class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _conf;

        public AppDbContextInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration conf)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _conf = conf;
        }

        public async Task Migration()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
        }

        public async Task InitializeAdmin()
        {
            AppUser admin = new AppUser
            {
                Name = "admin",
                Surname = "admin",
                Email = _conf["AdminSettings:Email"],
                UserName = _conf["AdminSettings:UserName"]
            };

            await _userManager.CreateAsync(admin, _conf["AdminSettings:Password"]);
            await _userManager.AddToRoleAsync(admin, UserRoles.Admin.ToString());
        }

    }
}
