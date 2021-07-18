using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Challenge> Challenges { get; set; }
        public ICollection<Discuss> Discusses { get; set; }

        public Tag()
        {
            Challenges = new List<Challenge>();
            Discusses = new List<Discuss>();
        }
           
    }
}
