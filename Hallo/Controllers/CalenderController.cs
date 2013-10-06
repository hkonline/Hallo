using System;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class CalenderController : HalloController {
        public ActionResult Index() {
            ViewBag.ShowLeft = true;
            ViewBag.ShowRight = false;
            ViewBag.Headline = "Månedskalender";
            return View();
        }
    }
}
