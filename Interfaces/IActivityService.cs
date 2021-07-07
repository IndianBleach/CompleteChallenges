using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Project.Models;

namespace Project.Interfaces
{
    public interface IActivityService
    {
        public Task AddUserActivityEvent(string forAuthorUsername, string titleContent);
        public List<UserEvent> GetUserActivityEvents(string forUsername);
    }
}
