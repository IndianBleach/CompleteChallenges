using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class DiscussController : Controller
    {
        private DiscussService _discussService;

        public DiscussController(ApplicationContext ctx, DiscussService discussService)
        {
            _discussService = discussService;
        }

        [Authorize]
        public async Task<IActionResult> CreateDiscuss(string discussContent, string discussName)
        {
            if (discussContent != null && discussName != null)        
                await _discussService.AddDiscuss(User.Identity.Name, discussContent, discussName);            

            List<Discuss> discusses = _discussService.GetAllDiscusses();

            return View("index", discusses);
        }

        [Authorize]
        public IActionResult CreateReply(int? discussId, string replyContent)
        {
            Discuss updatedDiscuss = _discussService.AddReplyAndGetDiscuss(User.Identity.Name, replyContent, discussId);

            if (updatedDiscuss != null)
                return View("replies", updatedDiscuss);

            return RedirectToAction("index");
        }

        public IActionResult Replies(int? discuss)
        {
            Discuss loadDiscuss = _discussService.GetDiscussWithReplies(discuss);

            if (loadDiscuss != null)
                return View("replies", loadDiscuss);

            return RedirectToAction("index");
        }

        public IActionResult Index()
        {
            List<Discuss> discusses = _discussService.GetAllDiscusses();

            return View(discusses);
        }
    }
}
