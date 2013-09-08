using HalloDal.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class HalloController : Controller {

        private HalloContext context;
        protected HalloContext Context {
            get {
                if (context == null) context = new HalloContext();
                
                return context;
            }
        }

    }
}
