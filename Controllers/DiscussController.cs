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
        private ApplicationContext _ctx;

        public DiscussController(ApplicationContext ctx, DiscussService discussService)
        {
            _ctx = ctx;
            _discussService = discussService;
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? discussId)
        {
            if (discussId != null)
                await _discussService.DeleteDiscuss(discussId);

            List<Discuss> discusses = _discussService.GetAllDiscusses();

            return View("index", discusses);
        }

        [Authorize]
        public async Task<IActionResult> Create(string discussContent, string discussName, ICollection<string> tags)
        {
            if (discussContent != null && discussName != null)
                await _discussService.CreateDiscuss(User.Identity.Name, discussContent, discussName, tags);

            List<Discuss> discusses = _discussService.GetAllDiscusses();

            return View("index", discusses);
        }

        [Authorize]
        public IActionResult AddReply(int? discussId, string replyContent)
        {
            Discuss updatedDiscuss = _discussService.AddDiscussReply(User.Identity.Name, replyContent, discussId);

            if (updatedDiscuss != null)
                return View("replies", updatedDiscuss);

            return RedirectToAction("index");
        }

        public IActionResult Replies(int? discuss)
        {
            Discuss loadDiscuss = _discussService.GetDiscuss(discuss);

            if (loadDiscuss != null)
                return View("replies", loadDiscuss);

            return RedirectToAction("index");
        }

        public IActionResult Index()
        {
            List<Discuss> discusses = _discussService.GetAllDiscusses();
            ViewBag.Tags = _ctx.Tags.ToList();

            return View(discusses);
        }
    }
}
