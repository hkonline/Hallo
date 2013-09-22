using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class MeetingController : Controller {

        public ActionResult Streaming() {
            ViewBag.ShowLeft = true;
            return View();
        }

    }
}
