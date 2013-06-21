using Hallo.Infrastructure;
using Hallo.Models;
using Hallo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            List<Article> articleList;

            using (kobenhavnContext context = new kobenhavnContext()) {
                // TODO: if not a know kbh-user, show only public articles
                articleList = context.Articles.Where(x => x.ApprovedByEditor == true)
                    .Where(x => x.Category != 14)
                    .Where(x => x.Category2 != 14)
                    .OrderByDescending(x => x.Date)
                    .Take(12)
                    .ToList();
            }

            List<FrontPageArticle> model = new List<FrontPageArticle>();
            bool isFirst = true;
            foreach (Article a in articleList) {
                FrontPageArticle fa = new FrontPageArticle(a);
                if (isFirst) { 
                    fa.newest = true; 
                    isFirst = false; 
                }                
                model.Add(fa);
            }

            ViewBag.ShowLeft = true;
            ViewBag.ShowRight = true;
            return View(model);
        }

        public PartialViewResult Menu() {
            MenuBuilder menuBuilder = new MenuBuilder(); 
            return PartialView(menuBuilder.FrontpageMenu());
        }

        public ActionResult Article(int id) {
            ViewBag.ShowLeft = true;
            using (kobenhavnContext context = new kobenhavnContext()) {
                return View(context.Articles.Where(x => x.ArticleId == id).SingleOrDefault());
            }            
        }

        public ViewResult SignOnTest() {
            Session["Hilsen"] = "Hej";
            return View();
        }
    }
}
