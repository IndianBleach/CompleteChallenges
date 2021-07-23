using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Services;

namespace Project.Components
{
    public class ChallengeListViewComponent : ViewComponent
    {
        private ChallengeService _challengeService;
        public ChallengeListViewComponent(ChallengeService challengeServ)
        {
            _challengeService = challengeServ;
        }

        public IViewComponentResult Invoke()
        {
            return View(_challengeService.GetAllChallenges());
        }
    }
}
