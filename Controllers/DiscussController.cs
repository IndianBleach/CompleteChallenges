using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class DiscussController : Controller
    {
        private ApplicationContext _ctx;

        public DiscussController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        [Authorize]
        public async Task<IActionResult> CreateDiscuss(string discussContent, string discussName)
        {
            if (discussContent != null && discussName != null)
            {
                _ctx.Discusses.Add(new Discuss
                {
                    Author = _ctx.Users.FirstOrDefault(x =>
                        x.Username == User.Identity.Name),
                    Content = discussContent,
                    DateCreated = DateTime.Now,
                    Name = discussName,
                    Replies = new List<Reply>()
                });
                await _ctx.SaveChangesAsync();
            }

            List<Discuss> discusses = await _ctx.Discusses
                .Include(x => x.Author)
                .Include(x => x.Replies)
                .ToListAsync();

            return View("index", discusses);
        }

        [Authorize]
        public async Task<IActionResult> CreateReply(int? discussId, string replyContent)
        {
            if (discussId != null)
            {
                Discuss dis = await _ctx.Discusses
                    .Include(x => x.Replies)
                    .FirstOrDefaultAsync(x =>
                        x.Id == discussId);

                if (replyContent != null)
                {
                    _ctx.Replies.Add(new Reply
                    {
                        Author = _ctx.Users.FirstOrDefault(x =>
                            x.Username == User.Identity.Name),
                        Content = replyContent,
                        DateCreated = DateTime.Now,
                        Discuss = dis
                    });
                    await _ctx.SaveChangesAsync();
                }

                return View("replies", dis);
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Replies(int? discuss)
        {
            if (discuss != null)
            {
                Discuss findDis = await _ctx.Discusses
                    .Include(x => x.Author)
                    .Include(x => x.Replies)
                    .FirstOrDefaultAsync(x =>
                    x.Id == discuss);

                if (findDis != null) return View(findDis);
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Index()
        {
            List<Discuss> discusses = await _ctx.Discusses
                .Include(x => x.Author)
                .Include(x => x.Replies)
                .ToListAsync();

            //ViewBag.Discusses = discusses;

            return View(discusses);
        }
    }
}
