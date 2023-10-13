using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class AppDbContext : IdentityDbContext<UserApp>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<UserApp> UsersApp { get; set; }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<TownNumber> TownsNumber { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Town>().HasData(new Town()
            {
                Id = 1,
                Name = "Villa Real",
                Detail = "Detalle de la villa",
                ImageUrl = "",
                Occupants = 5,
                SquareMeter = 50,
                Fee = 200,
                Amenity = "",
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            }, new Town()
            {
                Id = 2,
                Name = "Vista Premium de la Piscina",
                Detail = "Detalle de la villa",
                ImageUrl = "",
                Occupants = 4,
                SquareMeter = 40,
                Fee = 150,
                Amenity = "",
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }
    }
}