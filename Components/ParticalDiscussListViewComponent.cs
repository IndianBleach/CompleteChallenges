using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Services;
using Project.Models;

namespace Project.Components
{
    public class ParticalDiscussListViewComponent : ViewComponent
    {
        private DiscussService _discussService;
        public ParticalDiscussListViewComponent(DiscussService serv)
        {
            _discussService = serv;
        }

        public IViewComponentResult Invoke()
        {
            Discuss[] getAllDiscuss = _discussService.GetAllDiscusses().ToArray();

            //List<Discuss> mostPopular = getAllDiscuss.OrderBy(x => x.Li)

            List<Discuss> buildTenDiscusses = new List<Discuss>();
            var endOf = getAllDiscuss.ToArray().Length >= 10 ? 10 : getAllDiscuss.ToArray().Length;
            for (int i = 0; i < endOf; i++)
            {
                buildTenDiscusses.Add(getAllDiscuss[i]);
            }

            return View(buildTenDiscusses);
        }
    }
}
