using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Components
{
    public class DiscussListViewComponent : ViewComponent
    {
        public DiscussListViewComponent()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View("DiscussList");
        }
    }
}
