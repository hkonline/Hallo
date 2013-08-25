using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class ImageController : HalloController {

        public ActionResult List(int id) {
            Session["CurrentArticleId"] = id;

            List<Image> list = Context.Articles
                .First(x => x.Id == id)
                .Images
                .OrderBy(x => x.OrderNr)
                .ToList();

            foreach (Image i in list) i.OrderNr = list.IndexOf(i);
            Context.SaveChanges();

            return View(list);
        }

        public ActionResult OrderUp(int id) {
            int articleId = (int)Session["CurrentArticleId"];
            Image image = Context.Images.FirstOrDefault(x => x.Id == id);
            int orderNr = (int)image.OrderNr;
            Image imageAbove = Context.Articles.FirstOrDefault(x => x.Id == articleId)
                .Images.FirstOrDefault(x => x.OrderNr == orderNr-1);
            if (image != null && imageAbove != null) {
                image.OrderNr -= 1;
                imageAbove.OrderNr += 1;
                Context.SaveChanges();
            }
            return RedirectToAction("List", new { id = articleId });
        }

        public ActionResult OrderDown(int id) {
            int articleId = (int)Session["CurrentArticleId"];
            Image image = Context.Images.FirstOrDefault(x => x.Id == id);
            int orderNr = (int)image.OrderNr;
            Image imageBelow = Context.Articles.FirstOrDefault(x => x.Id == articleId)
                .Images.FirstOrDefault(x => x.OrderNr == orderNr+1);
            if (image != null && imageBelow != null) {
                image.OrderNr += 1;
                imageBelow.OrderNr -= 1;
                Context.SaveChanges();
            }
            return RedirectToAction("List", new { id = articleId });
        }
    }
}
