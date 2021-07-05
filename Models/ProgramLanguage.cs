using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ProgramLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<Solution> Solutions { get; set; }
    }
}
