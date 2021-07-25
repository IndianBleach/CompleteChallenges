using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Components
{
    public class ProfileSectionViewComponent : ViewComponent
    {
        public ProfileSectionViewComponent()
        {
        }

        public IViewComponentResult Invoke(ViewSection section)
        {
            if (string.IsNullOrEmpty(section.Name))
            {
                return View("activity");
            }
            else
            {
                return View(section.Name);
            }
        }        
    }
}
