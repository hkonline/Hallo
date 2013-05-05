using Hallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class InfoBoxController : Controller {

        [ChildActionOnly]
        public PartialViewResult CurrentInfo() {
            List<InfoBox> model;

            DateTime today = new DateTime(2013, 3, 10);

            using (kobenhavnContext context = new kobenhavnContext()) {
                model = context.InfoBoxes
                    .Where(x => x.dato_begynd < today)
                    .Where(x => x.dato_slut > today).ToList();
            }

            return PartialView(model);
        }

    }
}
