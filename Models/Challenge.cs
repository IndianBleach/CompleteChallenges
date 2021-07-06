﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public ChallengeLevel Level { get; set; }
        public ICollection<ChallengeLike> Likes { get; set; }
        public ICollection<Solution> Solutions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<User> UsersWhoUnlocked { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public DateTime DateCreated { get; set; }

        public Challenge()
        {
            Likes = new List<ChallengeLike>();
            Solutions = new List<Solution>();
            Tests = new List<Test>();
            UsersWhoUnlocked = new List<User>();
            Tags = new List<Tag>();            
        }
    }

    public class ChallengeLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Challenge> Challenges { get; set; }
        public ChallengeLevel() =>
            Challenges = new List<Challenge>();
    }

    public class ChallengeLike 
    { 
        public int Id { get; set; }
        public User Author { get; set; }
        public Challenge OnChallenge { get; set; }
    }
}