using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HepsiDeveloper.Data.Entities;

namespace HepsiDeveloper.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserApp, RoleApp, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Urun>? Urunler { get; set; }
        public DbSet<Resim>? Resimler { get; set; }
        
        public DbSet<UserApp>? Kullanicilar { get; set; }

    }
}