using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Discuss
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public DateTime DateCreated { get; set; }

        public Discuss() =>
            Replies = new List<Reply>();
    }

    public class Reply
    { 
        public int Id { get; set; }
        public Discuss Discuss { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }         
    }
}
