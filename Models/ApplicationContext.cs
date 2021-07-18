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
        public DbSet<Avatar> Avatars { get; set; }
        //challenge-log
        public DbSet<ChallengeComment> ChallengeComments { get; set; }
        public DbSet<ChallengeReport> ChallengeReports { get; set; }
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
            //modelBuilder.Entity<AvatarImage>().HasOne(x => x.User).WithOne(x => x.Avatar);
            //modelBuilder.Entity<User>().HasOne(x => x.Avatar).WithOne(x => x.User);

            modelBuilder.Entity<User>().HasMany(x => x.MyChallenges).WithOne(x => x.Author);
            modelBuilder.Entity<User>().HasMany(x => x.MyDiscusses).WithOne(x => x.Author);
            modelBuilder.Entity<User>().HasMany(x => x.MySolutions).WithOne(x => x.Author);

            modelBuilder.Entity<Test>().HasOne(x => x.Challenge).WithMany(x => x.Tests);
            modelBuilder.Entity<Test>().HasOne(x => x.ProgLanguage).WithMany(x => x.Tests);

            modelBuilder.Entity<CsharpTest>();
            modelBuilder.Entity<PythonTest>();

            UserRole[] defaultRoles = new UserRole[]
            {
                new UserRole {
                Id = 1,
                Name = "admin"
                },
                new UserRole {
                Id = 2,
                Name = "user"
                },
            };

            User adminUser = new User 
            {
                Id = 1,
                Username = "admin",
                Email = "admin@gmail.com",
                Password = "admin",
                RoleId = 1 
            };

            ChallengeLevel[] defaultLevels = new ChallengeLevel[]
            {
                new ChallengeLevel()
                {
                    Challenges = new List<Challenge>(),
                    Id = 1,
                    Name = "Advanced",
                    ThemeColor = "#D9566C",
                    Score = 500
                },
                new ChallengeLevel()
                {
                    Challenges = new List<Challenge>(),
                    Id = 2,
                    Name = "Elder",
                    ThemeColor = "#B166CD",
                    Score = 325
                },
                new ChallengeLevel()
                {
                    Challenges = new List<Challenge>(),
                    Id = 3,
                    Name = "Medium",
                    ThemeColor = "#6465B8",
                    Score = 250
                },
                 new ChallengeLevel()
                {
                    Challenges = new List<Challenge>(),
                    Id = 4,
                    Name = "Beginner",
                    ThemeColor = "#F2A43C",
                    Score = 125
                },
            };

            ProgramLanguage[] defaultLanguages = new ProgramLanguage[]
            {
                new ProgramLanguage()
                {
                    Solutions = new List<Solution>(),
                    Name = "Csharp",
                    Id = 1,
                },
                new ProgramLanguage()
                {
                    Solutions = new List<Solution>(),
                    Name = "Python",
                    Id = 2,
                }
            };


            string[] tags = new string[] { "Массивы", "Сортировка", "Простые типы", "Сложные типы", "Циклы", "Ветвления", "Объекты"};
            List<Tag> defaultTags = new List<Tag>();
            for (int i = 1; i <= tags.Length; i++)
            {
                defaultTags.Add(new Tag()
                {
                    Name = tags[i-1],
                    Challenges = new List<Challenge>(),
                    Id = i
                });
            }


            modelBuilder.Entity<Avatar>().HasData(new Avatar() { Name = "default_user_avatar221.jpg", Id = 1 });
            modelBuilder.Entity<UserRole>().HasData(defaultRoles);
            modelBuilder.Entity<ChallengeLevel>().HasData(defaultLevels);
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<ProgramLanguage>().HasData(defaultLanguages);
            modelBuilder.Entity<Tag>().HasData(defaultTags);

            base.OnModelCreating(modelBuilder);
        }
    }
}
