using Microsoft.AspNetCore.Mvc;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Components
{
    public class DiscussListViewComponent : ViewComponent
    {
        private DiscussService _discussService;
        public DiscussListViewComponent(DiscussService discussService)
        {
            _discussService = discussService;
        }

        public IViewComponentResult Invoke()
        {
            return View(_discussService.GetAllDiscusses());
        }
    }
}
