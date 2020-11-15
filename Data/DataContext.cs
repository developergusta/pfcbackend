using Microsoft.EntityFrameworkCore;
using Ticket2U.API.Models;

namespace Ticket2U.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<LotCategory> LotCategories { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cashback> Cashbacks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.
            UseNpgsql("Server=lallah.db.elephantsql.com;Port=5432;Database=myvpljle;Uid=myvpljle;Password=esQpIE8p5y-tE5o5PlQutwpKfmkpg3_N");
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder){           
            
            modelBuilder.Entity<Login>(entity => 
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });
            modelBuilder.Entity<User>(entity => 
            {
                entity.HasOne( u => u.Login ).WithOne( u => u.User ).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne( u => u.Image ).WithOne( u => u.User ).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany( u => u.Addresses ).WithOne( a => a.User ).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany( u => u.Phones ).WithOne( a => a.User ).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(u => u.Cpf).IsUnique();
            });
            modelBuilder.Entity<Event>(entity => 
            {
                entity.HasMany( u => u.Lots ).WithOne( a => a.Event ).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany( u => u.Images ).WithOne( a => a.Event ).OnDelete(DeleteBehavior.Cascade) ;
            });
            modelBuilder.Entity<Lot>(entity => 
            {
                entity.HasMany( l => l.LotCategories).WithOne( a => a.Lot).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}