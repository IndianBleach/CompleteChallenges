using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.Extensions.Configuration;

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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ChallengeLike> ChallengeLikes { get; set; }
        public DbSet<ChallengeLevel> ChallengeLevels { get; set; }
        
        //discuss-log
        public DbSet<Discuss> Discusses { get; set; }
        public DbSet<Reply> Replies { get; set; }
                
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //enable models relationship
            modelBuilder.Entity<User>().HasMany(x => x.MyChallenges).WithOne(x => x.Author);
            modelBuilder.Entity<User>().HasMany(x => x.MyDiscusses).WithOne(x => x.Author);
            modelBuilder.Entity<User>().HasMany(x => x.MySolutions).WithOne(x => x.Author);

            modelBuilder.Entity<Test>().HasOne(x => x.Challenge).WithMany(x => x.Tests);
            modelBuilder.Entity<Test>().HasOne(x => x.ProgLanguage).WithMany(x => x.Tests);

            modelBuilder.Entity<CsharpTest>();
            modelBuilder.Entity<PythonTest>();

            UserRole adminRole = new UserRole 
            {
                Id = 1,
                Name = "admin"
            };

            UserRole userRole = new UserRole
            {
                Id = 2,
                Name = "user"
            };

            User adminUser = new User 
            {
                Id = 1,
                Username = "admin",
                Email = "admin@gmail.com",
                Password = "admin",
                RoleId = adminRole.Id 
            };            

            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
