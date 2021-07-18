using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Services
{
    public class DiscussService
    {
        private ApplicationContext _ctx;
        public DiscussService(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public Discuss GetDiscussWithReplies(int? discussId)
        {
            if (discussId != null)
            {
                Discuss findDis =  _ctx.Discusses
                    .Include(x => x.Author)
                    .Include(x => x.Replies)
                    .Include(x => x.Tags)
                    .FirstOrDefault(x =>
                    x.Id == discussId);

                if (findDis != null) return findDis;
            }
            return null;
        }

        public Discuss AddReplyHook(string authorUsername, string replyContent, int? discussId)
        {
            if (discussId != null)
            {
                Discuss findDiscuss = _ctx.Discusses
                    .Include(x => x.Replies)
                    .Include(x => x.Tags)
                    .FirstOrDefault(x =>
                        x.Id == discussId);

                if (findDiscuss != null && replyContent != null)
                {
                    _ctx.Replies.Add(new Reply
                    {
                        Author = _ctx.Users.FirstOrDefault(x =>
                            x.Username == authorUsername),
                        Content = replyContent,
                        DateCreated = DateTime.Now,
                        Discuss = findDiscuss
                    });
                }
                _ctx.SaveChanges();

                return findDiscuss;
            }
            return null;
        }

        public List<Discuss> GetAllDiscusses()
        {
            List<Discuss> all = _ctx.Discusses
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .Include(x => x.Replies)
                .ToList();

            return all;
        }

        public async Task AddDiscuss(string authorUsername, string discussContent, string discussName, ICollection<string> tags)
        {
            List<Tag> buildTagList = new List<Tag>();
            foreach (var item in tags)
            {
                buildTagList.Add(_ctx.Tags.FirstOrDefault(x => x.Name == item));
            }

            _ctx.Discusses.Add(new Discuss
            {
                Author = await _ctx.Users.FirstOrDefaultAsync(x => x.Username == authorUsername),
                Content = discussContent,
                DateCreated = DateTime.Now,
                Name = discussName,
                Replies = new List<Reply>(),
                Tags = buildTagList
            });
            await _ctx.SaveChangesAsync();
        }

    }
}
