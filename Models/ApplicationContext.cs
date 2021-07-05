using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Models
{
    public class ApplicationContext : DbContext
    {
        //user-log
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }

        //challenge-log
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<ProgramLanguage> ProgramLanguages { get; set; }

        //discuss-log
        public DbSet<Discuss> Discusses { get; set; }
        public DbSet<Reply> Replies { get; set; }
                
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
