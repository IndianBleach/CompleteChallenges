using Project.Interfaces;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Services
{
    public class UserActivityService : IActivityService
    {
        private ApplicationContext _ctx;
        public UserActivityService(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        //delete activity

        public async Task AddActivity(string forAuthorUsername, string titleContent)
        {
            _ctx.UserEvents.Add(new UserEvent
            {
                User = await _ctx.Users.FirstOrDefaultAsync(x =>
                    x.Username == forAuthorUsername),
                Title = titleContent,
                DateCreatedStr = DateTime.Now.ToShortDateString()
            });

            await _ctx.SaveChangesAsync();
        }

        public List<UserEvent> GetUserActivity(string forUsername)
        {
            if (forUsername != null)
            {
                List<UserEvent> userEvents = _ctx.UserEvents
                    .Where(x => x.User.Username == forUsername)
                    .ToList();

                return userEvents;
            }
            return null;
        }
    }
}
