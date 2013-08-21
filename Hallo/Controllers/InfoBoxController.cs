using HalloDal.Models;
using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class InfoBoxController : Controller {

        [ChildActionOnly]
        public PartialViewResult CurrentInfo() {
            List<Message> model;

            DateTime today = new DateTime(2013, 3, 10);

            using (HalloContext context = new HalloContext()) {
                model = context.Messages
                    .Where(x => x.StartDate < today)
                    .Where(x => x.EndDate > today).ToList();
            }

            return PartialView(model);
        }

    }
}
