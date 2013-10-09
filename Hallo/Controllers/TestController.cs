using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class TestController : HalloController {

        public ActionResult Index() {
            ViewBag.ShowLeft = true;
            ViewBag.ShowRight = true;
            ViewBag.ContentWidth = 801;
            ViewBag.ContentPadding = 0;

            ViewBag.Text = "Anders Hansen (15434)";

            return View(HalloUser);
        }

    }
}
