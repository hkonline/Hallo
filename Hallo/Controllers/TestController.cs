using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class TestController : Controller {

        public ActionResult Index() {
            ViewBag.ShowLeft = true;
            ViewBag.ShowRight = true;
            ViewBag.ContentWidth = 801;
            ViewBag.ContentPadding = 0;
            return View();
        }

    }
}
