using Hallo.Infrastructure;
using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Hallo.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            ViewBag.ShowRight = true;

            List<Article> articleList;

            using (HalloContext context = new HalloContext()) {
                ArticleCategory internalSongMission = context.Categories.FirstOrDefault(x => x.Id == 14);

                // TODO: if not a know kbh-user, show only public articles
                articleList = context.Articles
                    .Include("FrontpageImage")
                    .Where(x => x.ApprovedByEditor == true)
                    .Where(x => x.Category == null || x.Category.Id != internalSongMission.Id)
                    .Where(x => x.Category2 == null || x.Category2.Id != internalSongMission.Id)
                    .OrderByDescending(x => x.Date)
                    .Take(12)
                    .ToList();
            }

            List<FrontPageArticle> model = new List<FrontPageArticle>();
            bool isFirst = true;
            foreach (Article a in articleList) {
                FrontPageArticle fa = new FrontPageArticle(a);
                if (isFirst) {
                    fa.Newest = true;
                    isFirst = false;
                }
                model.Add(fa);
            }

            ViewBag.ShowLeft = true;
            ViewBag.ShowRight = true;
            ViewBag.ContentWidth = 801;
            ViewBag.ContentPadding = 0;
            return View(model);
        }

        public PartialViewResult Menu() {
            MenuBuilder menuBuilder = new MenuBuilder();
            return PartialView(menuBuilder.FrontpageMenu());
        }

        public ViewResult SignOnTest() {
            Session["Hilsen"] = "Hej";
            return View();
        }
    }
}
