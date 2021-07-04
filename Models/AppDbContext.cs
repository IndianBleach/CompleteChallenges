using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Models
{
    public class AppDbContext : DbContext
    {
        //user-log
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        
        //challenge-log
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<ProgramLanguage> ProgramLanguages { get; set; }
                
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
