using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Components
{
    public class ChallengeListViewComponent : ViewComponent
    {
        public ChallengeListViewComponent()
        {  
            
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
