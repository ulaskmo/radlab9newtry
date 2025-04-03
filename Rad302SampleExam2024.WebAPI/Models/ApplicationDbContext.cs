using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Rad302SampleExam2024.WebAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ❌ Removed OnConfiguring to avoid hardcoding SQL Server
        // EF Core will now use what is injected in Program.cs (SQLite)
    }
}