using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.ValidModels
{
    public class CreateChellange
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public ICollection<string> Tags { get; set; }
        public string CsharpTest { get; set; }
        public string PythonTest { get; set; }

    }
}
