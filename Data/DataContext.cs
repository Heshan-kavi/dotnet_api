using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Data
{
    public class DataContext : DbContext
    {
       public DataContext(DbContextOptions<DataContext> options): base(options)
       {

       }  

       protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id = 1, Name = "Fireball", Damage = 30},
                new Skill {Id = 2, Name = "Frenzy", Damage = 40},
                new Skill {Id = 3, Name = "Spear", Damage = 20}
            );
       }

       public DbSet<Character> Characters => Set<Character>();
       public DbSet<User> Users => Set<User>();
       public DbSet<Weapon> Weapons => Set<Weapon>();
       public DbSet<Skill> Skills => Set<Skill>();
    }
}