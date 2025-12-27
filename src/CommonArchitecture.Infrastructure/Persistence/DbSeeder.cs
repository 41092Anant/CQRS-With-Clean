using CommonArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommonArchitecture.Infrastructure.Persistence;

public class DbSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(ApplicationDbContext context, ILogger<DbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }

            await SeedMenusAsync();
            await SeedRolePermissionsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedMenusAsync()
    {
        var defaultMenus = new List<Menu>
        {
            new() { Name = "Dashboard", Url = "/Admin/Dashboard", Icon = "bi bi-speedometer2", DisplayOrder = 1 },
            new() { Name = "Products", Url = "/Admin/Products", Icon = "bi bi-box-seam", DisplayOrder = 2 },
            new() { Name = "Role Master", Url = "/Admin/Roles", Icon = "bi bi-shield-lock", DisplayOrder = 3 },
            new() { Name = "User Master", Url = "/Admin/Users", Icon = "bi bi-people", DisplayOrder = 4 },
            new() { Name = "Menu Master", Url = "/Admin/Menus", Icon = "bi bi-list", DisplayOrder = 5 },
            new() { Name = "Role Permission", Url = "/Admin/RoleMenus", Icon = "bi bi-gear", DisplayOrder = 6 },
            new() { Name = "Hangfire Jobs", Url = "/Admin/HangfireJobs", Icon = "bi bi-clock-history", DisplayOrder = 7 }
        };

        foreach (var menu in defaultMenus)
        {
            if (!await _context.Menus.AnyAsync(m => m.Name == menu.Name))
            {
                _context.Menus.Add(menu);
            }
        }
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Menus seeded/updated successfully.");
    }

    private async Task SeedRolePermissionsAsync()
    {
        // Ensure Admin Role exists (It should be seeded by Identity or other means, but let's check basic role)
        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
             // If no admin role, maybe we should create it? 
             // Assuming "Admin" role ID 1 is standard or exists. 
             // Let's create if not exists
             adminRole = new Role { RoleName = "Admin", CreatedAt = DateTime.UtcNow };
             _context.Roles.Add(adminRole);
             await _context.SaveChangesAsync();
        }

        var menus = await _context.Menus.ToListAsync();
        
        foreach (var menu in menus)
        {
            // Check if permission already exists
            var existingPermission = await _context.RoleMenus
                .FirstOrDefaultAsync(rm => rm.RoleId == adminRole.Id && rm.MenuId == menu.Id);

            if (existingPermission == null)
            {
                _context.RoleMenus.Add(new RoleMenu
                {
                    RoleId = adminRole.Id,
                    MenuId = menu.Id,
                    CanCreate = true,
                    CanRead = true,
                    CanUpdate = true,
                    CanDelete = true,
                    CanExecute = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                // Ensure Admin always has full rights (Self-Correcting)
                bool changed = false;
                if (!existingPermission.CanCreate) { existingPermission.CanCreate = true; changed = true; }
                if (!existingPermission.CanRead) { existingPermission.CanRead = true; changed = true; }
                if (!existingPermission.CanUpdate) { existingPermission.CanUpdate = true; changed = true; }
                if (!existingPermission.CanDelete) { existingPermission.CanDelete = true; changed = true; }
                if (!existingPermission.CanExecute) { existingPermission.CanExecute = true; changed = true; }
                
                if (changed) _context.RoleMenus.Update(existingPermission);
            }
        }
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Admin permissions seeded/updated successfully.");
    }
}
